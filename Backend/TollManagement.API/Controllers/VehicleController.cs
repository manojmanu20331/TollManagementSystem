using Microsoft.AspNetCore.Mvc;
using TollManagement.Business.Interfaces;
using TollManagement.Models;

namespace TollManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    public IActionResult Add(Vehicle vehicle)
    {
        _vehicleService.AddVehicle(vehicle);
        return Ok(vehicle);
    }

    [HttpGet]
    public ActionResult<List<Vehicle>> GetAll()
    {
        return Ok(_vehicleService.GetAllVehicles());
    }

    [HttpGet("{vehicleId:int}")]
    public ActionResult<Vehicle> GetById(int vehicleId)
    {
        var vehicle = _vehicleService.GetVehicleById(vehicleId);
        return vehicle is null ? NotFound() : Ok(vehicle);
    }

    [HttpPut("{vehicleId:int}")]
    public IActionResult Update(int vehicleId, Vehicle vehicle)
    {
        vehicle.VehicleId = vehicleId;
        _vehicleService.UpdateVehicle(vehicle);
        return NoContent();
    }

    [HttpDelete("{vehicleId:int}")]
    public IActionResult Delete(int vehicleId)
    {
        _vehicleService.DeleteVehicle(vehicleId);
        return NoContent();
    }
}