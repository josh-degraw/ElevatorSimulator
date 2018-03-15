//using System;
//using System.Windows.Input;

//namespace ElevatorApp.Util
//{
//    public class DelegateCommand : ICommand
//    {
//        private readonly Action _execute;
//        private readonly Func<bool> _canExecute;

//        public event EventHandler CanExecuteChanged;

//        public DelegateCommand(Action execute, Func<bool> canExecute = null)
//        {
//            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
//            this._canExecute = canExecute ?? (() => true);
//        }

//        public bool CanExecute(object parameter) => _canExecute();
//        public void Execute(object parameter) => _execute();


//        public static DelegateCommand<T> Create<T>(Action<T> execute, Func<bool> canExecute = null) => new DelegateCommand<T>(execute, canExecute);

//        public static DelegateCommand Create(Action execute, Func<bool> canExecute = null) => new DelegateCommand(execute, canExecute);

//        public static implicit operator DelegateCommand(Action execute) => new DelegateCommand(execute);
//    }

//    public class DelegateCommand<T> : ICommand
//    {
//        private readonly Action<T> _execute;
//        private readonly Func<bool> _canExecute;

//        public event EventHandler CanExecuteChanged;

//        public DelegateCommand(Action<T> execute, Func<bool> canExecute = null)
//        {
//            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
//            this._canExecute = canExecute ?? (() => true);
//        }

//        public bool CanExecute(object parameter) => _canExecute();

//        public void Execute(object parameter)
//        {
//            if (parameter is T arg)
//            {
//                _execute(arg);
//            }
//            else
//                throw new NotSupportedException($"Invalid parameter type: {parameter.GetType()}. Expected {typeof(T)}");
//        }

//        public static implicit operator DelegateCommand<T>(Action<T> execute) => DelegateCommand.Create(execute);
//    }
//}