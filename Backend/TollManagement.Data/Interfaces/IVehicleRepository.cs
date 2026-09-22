using System.Collections.Generic;
using TollManagement.Models;

namespace TollManagement.Data.Interfaces
{
    public interface IVehicleRepository
    {
        int Add(Vehicle vehicle);

        Vehicle GetById(int vehicleId);

        List<Vehicle> GetAll();

        bool Update(Vehicle vehicle);

        bool Delete(int vehicleId);
    }
}
