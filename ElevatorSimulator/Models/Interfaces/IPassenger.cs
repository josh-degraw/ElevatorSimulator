namespace ElevatorApp.Models.Interfaces
{
    public interface IPassenger
    {
        PassengerState State { get; set; }
        int Weight { get; set; }
    }
}