/*
 * Note about this file:
 * All credit for this code, unless explicitly declared otherwise, is from the following source: https://pastebin.com/hKQi6EHD.
 * I claim no credit for this file and its content, except for the TryRemove and AddDistinct functions
 * - Josh DeGraw
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Threading;

// ReSharper disable All

namespace ElevatorApp.Util
{
    /// <summary>
    /// <para>
    /// A version of <see cref="ObservableCollection{T}"/> that is locked so that it can be accessed by multiple threads.
    /// </para>
    /// <para>
    /// When you enumerate it (foreach), you will get a snapshot of the current contents. Also the
    /// <see cref="INotifyCollectionChanged.CollectionChanged"/> event will be called on the thread that added it if that
    /// thread is a Dispatcher (WPF/Silverlight/WinRT) thread. This means that you can update this from any thread and
    /// recieve notifications of those updates on the UI thread.
    /// </para>
    /// <para>
    /// You can't modify the collection during a callback (on the thread that recieved the callback -- other threads can
    /// do whatever they want). This is the
    /// </para>
    /// same as <see cref="T:System.Collections.ObjectModel.ObservableCollection`1"/>.
    /// </summary>
    /// <remarks>All credit for this code, unless explicitly declared otherwise, is from source: https://pastebin.com/hKQi6EHD</remarks>
    /// <typeparam name="T">The type of item in the collections</typeparam>
    [Serializable, DebuggerDisplay("Count = {Count}")]
    [SuppressMessage("ReSharper", "InheritdocInvalidUsage")]
    public sealed class AsyncObservableCollection<T> : IList<T>, IReadOnlyList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged, ISerializable
    {
        /// <summary>
        /// Tries to remove the given item from the collection
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns>True if it was successfully removed, otherwise false</returns>
        /// <remarks>Added by Josh DeGraw, but using techniques from the original source code</remarks>
        public bool TryRemove(T item)
        {
            try
            {
                this.Remove(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds an item if it does not already exist in the collection
        /// </summary>
        /// <param name="item">The item to be added to the <see cref="AsyncObservableCollection{T}"/></param>
        /// <returns>True if the item is newly added, else false</returns>
        /// <remarks>Added by Josh DeGraw, but using techniques from the original source code</remarks>
        public bool AddDistinct(T item)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();

            this._lock.EnterUpgradeableReadLock();

            try
            {
                bool contains = false;

                contains = this._collection.Contains(item);
                if (contains)
                    return false;

                this._lock.EnterWriteLock();
                try
                {
                    this._version++;
                    this._collection.Add(item);
                }
                catch
                {
                    view.waitingEvents.Clear();
                    throw;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
            finally
            {
                this._lock.ExitUpgradeableReadLock();
            }

            this.dispatchWaitingEvents(view);

            return true;
        }

        #region Unmodified from source

        // we implement IReadOnlyList<T> because ObservableCollection<T> does, and we want to mostly keep API
        // compatability... this collection is NOT read only, but neither is ObservableCollection<T>

        private readonly ObservableCollection<T> _collection;       // actual collection
        private readonly ThreadLocal<ThreadView> _threadView;       // every thread has its own view of this collection
        private readonly ReaderWriterLockSlim _lock;                // whenever accessing the collection directly, you must aquire the lock
        private volatile int _version;                              // whenever collection is changed, increment this (should only be changed from within write lock, so no atomic needed)

        /// <inheritdoc/>
        public AsyncObservableCollection()
        {
            this._collection = new ObservableCollection<T>();
            this._lock = new ReaderWriterLockSlim();
            this._threadView = new ThreadLocal<ThreadView>(() => new ThreadView(this));

            // It was a design decision to NOT implement IDisposable here for disposing the ThreadLocal instance.
            // ThreadLocal has a finalizer so it will be taken care of eventually. Since the cache itself is a weak
            // reference, the only difference between explicitly disposing of it and waiting for finalization will be ~80
            // bytes per thread of memory in the TLS table that will stay around for an extra couple GC cycles. This is a
            // tiny, tiny cost, and reduces the API complexity of this class.
        }

        /// <inheritdoc/>
        public AsyncObservableCollection(IEnumerable<T> collection)
        {
            this._collection = new ObservableCollection<T>(collection);
            this._lock = new ReaderWriterLockSlim();
            this._threadView = new ThreadLocal<ThreadView>(() => new ThreadView(this));
        }

        #region ThreadView -- every thread that acceses this collection gets a unique view of it

        /// <summary>
        /// The "view" that a thread has of this collection. One of these exists for every thread that has accesed this
        /// collection, and a new one is automatically created when a new thread accesses it. Therefore, we can assume
        /// thate everything in here is being called from the correct thread and don't need to worry about threading issues.
        /// </summary>
        private sealed class ThreadView
        {
            // These fields will always be accessed from the correct thread, so no sync issues
            public readonly List<EventArgs> waitingEvents = new List<EventArgs>();    // events waiting to be dispatched

            public bool dissalowReenterancy;                                          // don't allow write methods to be called on the thread that's executing events

            // Private stuff all used for snapshot/enumerator
            private readonly int _threadId;                                           // id of the current thread

            private readonly AsyncObservableCollection<T> _owner;                     // the collection
            private readonly WeakReference<List<T>> _snapshot;                        // cache of the most recent snapshot
            private int _listVersion;                                                 // version at which the snapshot was taken
            private int _snapshotId;                                                  // incremented every time a new snapshot is created
            private int _enumeratingCurrentSnapshot;                                  // # enumerating snapshot with current ID; reset when a snapshot is created

            public ThreadView(AsyncObservableCollection<T> owner)
            {
                this._owner = owner;
                this._threadId = Thread.CurrentThread.ManagedThreadId;
                this._snapshot = new WeakReference<List<T>>(null);
            }

            /// <summary>
            /// Gets a list that's a "snapshot" of the current state of the collection, ie it's a copy of whatever
            /// elements are currently in the collection.
            /// </summary>
            public List<T> getSnapshot()
            {
                Debug.Assert(Thread.CurrentThread.ManagedThreadId == this._threadId);

                // if we have a cached snapshot that's up to date, just use that one
                if (!this._snapshot.TryGetTarget(out List<T> list) || this._listVersion != this._owner._version)
                {
                    // need to create a new snapshot if nothing is using the old snapshot, we can clear and reuse the
                    // existing list instead of allocating a brand new list. yay for eco-friendly solutions!
                    int enumCount = this._enumeratingCurrentSnapshot;
                    this._snapshotId++;
                    this._enumeratingCurrentSnapshot = 0;

                    this._owner._lock.EnterReadLock();
                    try
                    {
                        this._listVersion = this._owner._version;
                        if (list == null || enumCount > 0)
                        {
                            // if enumCount > 0 here that means something is currently using the instance of list. we
                            // create a new list here and "strand" the old list so the enumerator can finish enumerating
                            // it in peace.
                            list = new List<T>(this._owner._collection);
                            this._snapshot.SetTarget(list);
                        }
                        else
                        {
                            // clear & reuse the old list
                            list.Clear();
                            list.AddRange(this._owner._collection);
                        }
                    }
                    finally
                    {
                        this._owner._lock.ExitReadLock();
                    }
                }
                return list;
            }

            /// <summary>
            /// Called when an enumerator is allocated (NOT when enumeration begins, because by that point we could've
            /// moved onto a new snapshot).
            /// </summary>
            /// <returns>The ID to pass into <see cref="exitEnumerator"/>.</returns>
            public int enterEnumerator()
            {
                Debug.Assert(Thread.CurrentThread.ManagedThreadId == this._threadId);
                this._enumeratingCurrentSnapshot++;
                return this._snapshotId;
            }

            /// <summary>
            /// Cleans up after an enumerator.
            /// </summary>
            /// <param name="oldId">The value that <see cref="enterEnumerator"/> returns.</param>
            public void exitEnumerator(int oldId)
            {
                // if the enumerator is being disposed from a different thread than the one that creatd it, there's no
                // way to garuntee the atomicity of this operation. if this (EXTREMELY rare) case happens, we'll ditch
                // the list next time we need to make a new snapshot. this can never happen with a regular foreach()
                if (Thread.CurrentThread.ManagedThreadId == this._threadId)
                {
                    if (this._snapshotId == oldId) this._enumeratingCurrentSnapshot--;
                }
            }
        }

        #endregion ThreadView -- every thread that acceses this collection gets a unique view of it

        #region Read methods

        /// <inheritdoc/>
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found in the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(T item)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._collection.Contains(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this._collection.Count;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._collection.IndexOf(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        #endregion Read methods

        #region Write methods -- VERY repetitive, don't say I didn't warn you

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <inheritdoc/>
        public void Add(T item)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                this._collection.Add(item);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                foreach (T item in items) this._collection.Add(item);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into
        /// the collection.
        /// </returns>
        int IList.Add(object value)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            int result;
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                result = ((IList)this._collection).Add(value);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
            return result;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public void Insert(int index, T item)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                this._collection.Insert(index, item);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            bool result;
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                result = this._collection.Remove(item);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
            return result;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                this._collection.RemoveAt(index);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <inheritdoc/>
        public void Clear()
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                this._collection.Clear();
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        /// <summary>
        /// Moves the specified old index.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
        {
            ThreadView view = this._threadView.Value;
            if (view.dissalowReenterancy)
                throwReenterancyException();
            this._lock.EnterWriteLock();
            try
            {
                this._version++;
                this._collection.Move(oldIndex, newIndex);
            }
            catch (Exception)
            {
                view.waitingEvents.Clear();
                throw;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }

            this.dispatchWaitingEvents(view);
        }

        #endregion Write methods -- VERY repetitive, don't say I didn't warn you

        #region A little bit o' both

        /// <inheritdoc/>
        public T this[int index]
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this._collection[index];
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }

            set
            {
                ThreadView view = this._threadView.Value;
                if (view.dissalowReenterancy)
                    throwReenterancyException();
                this._lock.EnterWriteLock();
                try
                {
                    this._version++;
                    this._collection[index] = value;
                }
                catch (Exception)
                {
                    view.waitingEvents.Clear();
                    throw;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }

                this.dispatchWaitingEvents(view);
            }
        }

        #endregion A little bit o' both

        #region GetEnumerator and related methods that work on snapshots

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            ThreadView view = this._threadView.Value;
            return new EnumeratorImpl(view.getSnapshot(), view);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            // don't need to worry about re-entry/other iterators here since we're at the bottom of the stack
            ((ICollection)this._threadView.Value.getSnapshot()).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public T[] ToArray()
        {
            // don't need to worry about re-entry/other iterators here since we're at the bottom of the stack
            return this._threadView.Value.getSnapshot().ToArray();
        }

        /// <inheritdoc/>
        private sealed class EnumeratorImpl : IEnumerator<T>
        {
            private readonly ThreadView _view;
            private readonly int _myId;
            private List<T>.Enumerator _enumerator;
            private bool _isDisposed;

            public EnumeratorImpl(List<T> list, ThreadView view)
            {
                this._enumerator = list.GetEnumerator();
                this._view = view;
                this._myId = view.enterEnumerator();
            }

            object IEnumerator.Current => this.Current;

            public T Current
            {
                get
                {
                    if (this._isDisposed)
                        throwDisposedException();
                    return this._enumerator.Current;
                }
            }

            public bool MoveNext()
            {
                if (this._isDisposed)
                    throwDisposedException();
                return this._enumerator.MoveNext();
            }

            public void Dispose()
            {
                if (!this._isDisposed)
                {
                    this._enumerator.Dispose();
                    this._isDisposed = true;
                    this._view.exitEnumerator(this._myId);
                }
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException("This enumerator doesn't support Reset()");
            }

            private static void throwDisposedException()
            {
                throw new ObjectDisposedException("The enumerator was disposed");
            }
        }

        #endregion GetEnumerator and related methods that work on snapshots

        #region Events

        // Because we want to hold the write lock for as short a time as possible, we enqueue events and dispatch them in
        // a group as soon as the write method is complete

        // Collection changed
        private readonly AsyncDispatcherEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs> _collectionChanged = new AsyncDispatcherEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>();

        private void onCollectionChangedInternal(object sender, NotifyCollectionChangedEventArgs args)
        {
            this._threadView.Value.waitingEvents.Add(args);
        }

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (value == null) return;
                this._lock.EnterWriteLock(); // can't add/remove event during write operation
                try
                {
                    // even though this is technically a write operation, there's no reason to check reenterancy since it
                    // won't ever call handler in fact, removing handlers in the callback could be a useful scenario
                    if (this._collectionChanged.isEmpty) // if we were empty before, the handler wasn't attached
                        this._collection.CollectionChanged += this.onCollectionChangedInternal;
                    this._collectionChanged.add(value);
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
            remove
            {
                if (value == null) return;
                this._lock.EnterWriteLock(); // can't add/remove event during write operation
                try
                {
                    // even though this is technically a write operation, there's no reason to check reenterancy since it
                    // won't ever call handler in fact, removing handlers in the callback could be a useful scenario
                    this._collectionChanged.remove(value);
                    if (this._collectionChanged.isEmpty) // if we're now empty, detatch handler
                        this._collection.CollectionChanged -= this.onCollectionChangedInternal;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        // Property changed
        private readonly AsyncDispatcherEvent<PropertyChangedEventHandler, PropertyChangedEventArgs> _propertyChanged = new AsyncDispatcherEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>();

        private void onPropertyChangedInternal(object sender, PropertyChangedEventArgs args)
        {
            this._threadView.Value.waitingEvents.Add(args);
        }

        /// <inheritdoc/>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                if (value == null) return;
                this._lock.EnterWriteLock(); // can't add/remove event during write operation
                try
                {
                    // even though this is technically a write operation, there's no reason to check reenterancy since it
                    // won't ever call handler in fact, removing handlers in the callback could be a useful scenario
                    if (this._propertyChanged.isEmpty) // if we were empty before, the handler wasn't attached
                        ((INotifyPropertyChanged)this._collection).PropertyChanged += this.onPropertyChangedInternal;
                    this._propertyChanged.add(value);
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
            remove
            {
                if (value == null) return;
                this._lock.EnterWriteLock(); // can't add/remove event during write operation
                try
                {
                    // even though this is technically a write operation, there's no reason to check reenterancy since it
                    // won't ever call handler in fact, removing handlers in the callback could be a useful scenario
                    this._propertyChanged.remove(value);
                    if (this._propertyChanged.isEmpty) // if we're now empty, detatch handler
                        ((INotifyPropertyChanged)this._collection).PropertyChanged -= this.onPropertyChangedInternal;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        private void dispatchWaitingEvents(ThreadView view)
        {
            List<EventArgs> waitingEvents = view.waitingEvents;
            try
            {
                if (waitingEvents.Count == 0) return; // fast path for no events
                if (view.dissalowReenterancy)
                {
                    // Write methods should have checked this before we got here. Since we didn't that means there's a
                    // bugg in this class itself. However, we can't dispatch the events anyways, so we'll have to throw
                    // an exception.
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    throwReenterancyException();
                }
                view.dissalowReenterancy = true;
                foreach (EventArgs args in waitingEvents)
                {
                    if (args is NotifyCollectionChangedEventArgs ccArgs)
                    {
                        this._collectionChanged.raise(this, ccArgs);
                    }
                    else
                    {
                        if (args is PropertyChangedEventArgs pcArgs)
                        {
                            this._propertyChanged.raise(this, pcArgs);
                        }
                    }
                }
            }
            finally
            {
                view.dissalowReenterancy = false;
                waitingEvents.Clear();
            }
        }

        private static void throwReenterancyException()
        {
            throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed -- don't modify the collection during callbacks from it!");
        }

        #endregion Events

        #region Methods to make interfaces happy -- most of these just foreward to the appropriate methods above

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T)value;
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => false;

        bool IList.Contains(object value)
        {
            return this.Contains((T)value);
        }

        object ICollection.SyncRoot => throw new NotSupportedException("AsyncObservableCollection doesn't need external synchronization");

        bool ICollection.IsSynchronized => false;

        void ICollection.CopyTo(Array array, int index)
        {
            // don't need to worry about re-entry/other iterators here since we're at the bottom of the stack
            ((ICollection)this._threadView.Value.getSnapshot()).CopyTo(array, index);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        #endregion Methods to make interfaces happy -- most of these just foreward to the appropriate methods above

        #region Serialization

        /// <summary>
        /// Constructor is only here for serialization, you should use the default constructor instead.
        /// </summary>
        public AsyncObservableCollection(SerializationInfo info, StreamingContext context)
            : this((T[])info.GetValue("values", typeof(T[])))
        {
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("values", this.ToArray(), typeof(T[]));
        }

        #endregion Serialization

        /// <summary>
        /// <para>
        /// Wrapper around an event so that any events added from a Dispatcher thread are invoked on that thread. This
        /// means that if the UI adds an event and that event is called on a different thread, the callback will be
        /// dispatched to the UI thread and called asynchronously. If an event is added from a non-dispatcher thread, or
        /// the event is raised from within the same thread as it was added from, it will be called normally.
        /// </para>
        /// <para>
        /// Note that this means that the callback will be asynchronous and may happen at some time in the future rather
        /// than as soon as the event is raised.
        /// </para>
        /// <para>This class is thread-safe.</para>
        /// </summary>
        /// <example>
        /// <code>
        ///  private readonly AsyncDispatcherEvent{PropertyChangedEventHandler, PropertyChangedEventArgs} _propertyChanged =
        ///        new DispatcherEventHelper{PropertyChangedEventHandler, PropertyChangedEventArgs}();
        ///
        ///     public event PropertyChangedEventHandler PropertyChanged
        ///     {
        ///         add { _propertyChanged.add(value); }
        ///         remove { _propertyChanged.remove(value); }
        ///     }
        ///
        ///     private void OnPropertyChanged(PropertyChangedEventArgs args)
        ///     {
        ///         _propertyChanged.invoke(this, args);
        ///     }
        /// </code>
        /// </example>
        /// <typeparam name="TEvent">
        /// The delagate type to wrap (ie PropertyChangedEventHandler). Must have a void delegate(object, TArgs) signature.
        /// </typeparam>
        /// <typeparam name="TArgs">Second argument of the TEvent. Must be of type EventArgs.</typeparam>
        public sealed class AsyncDispatcherEvent<TEvent, TArgs> where TEvent : class where TArgs : EventArgs
        {
            /// <summary>
            /// Type of a delegate that invokes a delegate. Okay, that sounds weird, but basically, calling this with a
            /// delegate and its arguments will call the Invoke() method on the delagate itself with those arguments.
            /// </summary>
            private delegate void InvokeMethod(TEvent @event, object sender, TArgs args);

            /// <summary>
            /// Method to invoke the given delegate with the given arguments quickly. It uses reflection once (per type)
            /// to create this, then it's blazing fast to call because the JIT knows everything is type-safe.
            /// </summary>
            private static readonly InvokeMethod _invoke;

            /// <summary>
            /// Using List{DelegateWrapper} and locking it on every access is what scrubs would do.
            /// </summary>
            private event EventHandler<TArgs> _event;

            /// <summary>
            /// Barely worth worrying about this corner case, but we need to lock on removes in case two identical
            /// non-dispatcher events are being removed at once.
            /// </summary>
            private readonly object _removeLock = new object();

            /// <summary>
            /// This is absolutely required to have a static constructor, otherwise it would be beforefieldinit which
            /// means that any type exceptions would be delayed until it's actually called. We can also do some extra
            /// checks here to make sure the types are correct.
            /// </summary>
            static AsyncDispatcherEvent()
            {
                Type tEvent = typeof(TEvent);
                Type tArgs = typeof(TArgs);
                if (!tEvent.IsSubclassOf(typeof(MulticastDelegate)))
                    throw new InvalidOperationException("TEvent " + tEvent.Name + " is not a subclass of MulticastDelegate");
                MethodInfo method = tEvent.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (method == null)
                    throw new InvalidOperationException("Could not find method Invoke() on TEvent " + tEvent.Name);
                if (method.ReturnType != typeof(void))
                    throw new InvalidOperationException("TEvent " + tEvent.Name + " must have return type of void");
                ParameterInfo[] paramz = method.GetParameters();
                if (paramz.Length != 2)
                    throw new InvalidOperationException("TEvent " + tEvent.Name + " must have 2 parameters");
                if (paramz[0].ParameterType != typeof(object))
                    throw new InvalidOperationException("TEvent " + tEvent.Name + " must have first parameter of type object, instead was " + paramz[0].ParameterType.Name);
                if (paramz[1].ParameterType != tArgs)
                    throw new InvalidOperationException("TEvent " + tEvent.Name + " must have second paramater of type TArgs " + tArgs.Name + ", instead was " + paramz[1].ParameterType.Name);
                _invoke = (InvokeMethod)method.CreateDelegate(typeof(InvokeMethod));
                if (_invoke == null)
                    throw new InvalidOperationException("CreateDelegate() returned null");
            }

            /// <summary>
            /// Adds the delegate to the event.
            /// </summary>
            public void add(TEvent value)
            {
                if (value == null)
                    return;
                this._event += (new DelegateWrapper(getDispatcherOrNull(), value)).invoke;
            }

            /// <summary>
            /// Removes the last instance of delegate from the event (if it exists). Only removes events that were added
            /// from the current dispatcher thread (if they were added from one), so make sure to remove from the same
            /// thread that added.
            /// </summary>
            public void remove(TEvent value)
            {
                if (value == null)
                    return;
                Dispatcher dispatcher = getDispatcherOrNull();
                lock (this._removeLock) // because events are intrinsically threadsafe, and dispatchers are thread-local, the only time this lock matters is when removing non-dispatcher events
                {
                    EventHandler<TArgs> evt = this._event;
                    if (evt != null)
                    {
                        Delegate[] invList = evt.GetInvocationList();
                        for (int i = invList.Length - 1; i >= 0; i--) // Need to go backwards since that's what event -= something does.
                        {
                            DelegateWrapper wrapper = (DelegateWrapper)invList[i].Target;

                            // need to use Equals instead of == for delegates
                            if (wrapper.handler.Equals(value) && wrapper.dispatcher == dispatcher)
                            {
                                this._event -= wrapper.invoke;
                                return;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Checks if any delegate has been added to this event.
            /// </summary>
            public bool isEmpty => this._event == null;

            /// <summary>
            /// Calls the event.
            /// </summary>
            public void raise(object sender, TArgs args)
            {
                try
                {
                    EventHandler<TArgs> evt = this._event;
                    evt?.Invoke(sender, args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            private static Dispatcher getDispatcherOrNull()
            {
                return Dispatcher.FromThread(Thread.CurrentThread);
            }

            private sealed class DelegateWrapper
            {
                public readonly TEvent handler;
                public readonly Dispatcher dispatcher;

                public DelegateWrapper(Dispatcher dispatcher, TEvent handler)
                {
                    this.dispatcher = dispatcher;
                    this.handler = handler;
                }

                public void invoke(object sender, TArgs args)
                {
                    try
                    {
                        if (this.dispatcher == null || this.dispatcher == getDispatcherOrNull())
                            _invoke(this.handler, sender, args);
                        else

                            // ReSharper disable once AssignNullToNotNullAttribute
                            this.dispatcher.BeginInvoke(this.handler as Delegate, DispatcherPriority.DataBind, sender, args);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        #endregion Unmodified from source
    }
}