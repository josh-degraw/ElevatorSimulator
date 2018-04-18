using System.Threading.Tasks;

namespace ElevatorApp.Models.Interfaces
{
    /// <summary>
    /// Indicates that this object depends on some object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object that will provide this object with information</typeparam>
    public interface ISubcriber<in T>
    {
        /// <summary>
        /// Represents whether or not this object has performed the necessary steps to subscribe to the source.
        /// </summary>
        bool Subscribed { get; }

        /// <summary>
        /// Perform the necessary steps to subscribe to the target.
        /// </summary>
        /// <param name="parent">The item this object will be subscribed to</param>
        Task Subscribe(T parent);
    }
}
