namespace ElevatorApp.Models.Interfaces
{
    public interface IMasterSubscriber
    {
        void Subscribe(ElevatorMasterController masterController);
    }

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
        void Subscribe(T parent);
    }
}
