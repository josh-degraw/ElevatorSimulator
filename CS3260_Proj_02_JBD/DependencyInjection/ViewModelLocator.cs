using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorApp.GUI.ViewModels
{
    public class ViewModelLocator
    {
        public ButtonPanelControlViewModel ButtonPanelControlViewModel => IocKernel.Get<ButtonPanelControlViewModel>();

    }
}
