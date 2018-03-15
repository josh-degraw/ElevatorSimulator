using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ElevatorApp.Util
{
    [Obsolete("Use "+ nameof(AsyncObservableCollection<T>) + " instead.")]
    public class ObservableConcurrentQueue<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<T>
    {
        private readonly ConcurrentQueue<T> _items;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableConcurrentQueue()
        {
            _items = new ConcurrentQueue<T>();
        }

        private void onChange(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Count)));
        }

        public ObservableConcurrentQueue(IEnumerable<T> collection)
        {
            List<T> list = collection.ToList();
            _items = new ConcurrentQueue<T>(list);

            this.onChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems: list));
            
        }
        
        public void Enqueue(T item)
        {
            _items.Enqueue(item);
            this.onChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem: item));
            
        }

        public bool TryDequeue(out T val)
        {
            bool succeeded = _items.TryDequeue(out val);
            if (succeeded)
            {
                this.onChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, val));
            }

            return succeeded;

        }

        public bool TryPeek(out T val)
        {
            return _items.TryPeek(out val);
        }

        #region Interface implementation

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }


        void ICollection<T>.Add(T item)
        {
            this.Enqueue(item);
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return _items.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        public int Count => _items?.Count ?? 0;
        public bool IsReadOnly => false;
        #endregion
    }
}
