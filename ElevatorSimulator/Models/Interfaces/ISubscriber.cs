using System.Threading.Tasks;

namespace ElevatorApp.Models.Interfaces
{
    public interface ISubcriber<in T>
    {
        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe
        /// </summary>
        bool Subscribed { get; }

        /// <summary>
        /// Perform the necessary steps to subscribe to the target.
        /// </summary>
        /// <param name="parent">The item this object will be subscribed to</param>
        Task Subscribe(T parent);
    }
}
