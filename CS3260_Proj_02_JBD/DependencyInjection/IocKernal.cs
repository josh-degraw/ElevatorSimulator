using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Core;
using ElevatorApp.GUI.ViewModels;
using Ninject;
using Ninject.Modules;

namespace ElevatorApp.GUI
{
    public static class IocKernel
    {
        private static StandardKernel _kernel;

        public static T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public static void Initialize(params INinjectModule[] modules)
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel(modules);
            }
        }
    }

    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<ElevatorButtonViewModel>().ToSelf().InTransientScope(); // Create new instance every time
        }
    }
}
