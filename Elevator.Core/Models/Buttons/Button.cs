using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Core.Interfaces;

namespace ElevatorApp.Core.Models
{
    public abstract class Button<T> : ModelBase, IButton<T>
    {
        public abstract string Label { get; }

        protected bool _active;
        public bool Active
        {
            get => _active;
            protected set => SetProperty(ref _active, value);
        }

        private event EventHandler<T> _onPushed;
        public virtual event EventHandler<T> OnPushed
        {
            add
            {
                Logger.LogEvent($"Adding eventHandler: {this.Id}-- {this.GetHashCode()}");
                _onPushed += value;
            }
            remove
            {
                _onPushed -= value;
            }
        }
        

        public virtual event EventHandler<T> OnActionCompleted;
        public virtual event EventHandler OnActivated;
        public virtual event EventHandler OnDeactivated;

        protected T actionArgs;

        public string Id { get; }
        protected Button(string id)
        {
            this.Id = id;
            this.OnActivated += _setActiveTrue;
            this.OnDeactivated += _setActiveFalse;
        }

        protected void _setActiveTrue(object sender, EventArgs e) => this.Active = true;
        protected void _setActiveFalse(object sender, EventArgs e) => this.Active = false;


        protected void Pushed(object sender, T args)
        {
            this._onPushed?.Invoke(this, args);
            actionArgs = args;
            OnActivated?.Invoke(sender, EventArgs.Empty);
        }

        protected void ActionCompleted(object sender, T args)
        {
            this.OnActionCompleted?.Invoke(this, args);
            this.actionArgs = default;
            OnDeactivated?.Invoke(sender, EventArgs.Empty);
        }

        public void ActionCompleted()
        {
            ActionCompleted(this, this.actionArgs);
        }

        public abstract void Push();

        public override string ToString()
        {
            return this.Label;
        }

        
    }
}
