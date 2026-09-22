using System.Collections.Generic;
using TollManagement.Models;

namespace TollManagement.Business.Interfaces
{
    public interface IVehicleService
    {
        void AddVehicle(Vehicle vehicle);

        List<Vehicle> GetAllVehicles();

        Vehicle GetVehicleById(int id);

        void UpdateVehicle(Vehicle vehicle);

        void DeleteVehicle(int id);
    }
}
