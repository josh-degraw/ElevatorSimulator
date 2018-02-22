using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorApp.Core.Models;

namespace ElevatorApp.Core.Interfaces
{
    public interface IMasterSubscriber
    {
        void Subscribe(ElevatorMasterController masterController);
    }

    public interface ISubcriber<T>
    {
        void Subscribe(T parent);
    }
}
