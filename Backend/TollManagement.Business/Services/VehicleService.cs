using TollManagement.Data.Interfaces;
using TollManagement.Business.Interfaces;
using TollManagement.Models;
using TollManagement.Data.Repositories;

namespace TollManagement.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Add(vehicle);
        }

        public List<Vehicle> GetAllVehicles()
        {
            return _vehicleRepository.GetAll();
        }

        public Vehicle GetVehicleById(int id)
        {
            return _vehicleRepository.GetById(id);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Update(vehicle);
        }

        public void DeleteVehicle(int id)
        {
            _vehicleRepository.Delete(id);
        }
    }
}