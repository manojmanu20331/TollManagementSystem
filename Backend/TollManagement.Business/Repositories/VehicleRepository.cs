using MySql.Data.MySqlClient;
using TollManagement.Data;
using TollManagement.Data.Interfaces;
using TollManagement.Models;

namespace TollManagement.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DbConnection _dbConnection;

        public VehicleRepository(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int Add(Vehicle vehicle)
        {
            using (MySqlConnection connection =
                   _dbConnection.GetConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO Vehicles
                    (
                        VehicleNumber,
                        OwnerName,
                        VehicleType
                    )
                    VALUES
                    (
                        @VehicleNumber,
                        @OwnerName,
                        @VehicleType
                    )";

                using (MySqlCommand command =
                       new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@VehicleNumber",
                        vehicle.VehicleNumber);
                    command.Parameters.AddWithValue(
                        "@OwnerName",
                        vehicle.OwnerName);

                    command.Parameters.AddWithValue(
                        "@VehicleType",
                        vehicle.VehicleType);

                    command.ExecuteNonQuery();
                    return (int)command.LastInsertedId;
                }
            }
        }

        public Vehicle GetById(int vehicleId)
        {
            const string query = @"
                SELECT VehicleId, VehicleNumber, OwnerName, VehicleType
                FROM Vehicles
                WHERE VehicleId = @VehicleId;";

            using (MySqlConnection connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", vehicleId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        return MapVehicle(reader);
                    }
                }
            }
        }

        public List<Vehicle> GetAll()
        {
            const string query = @"
                SELECT VehicleId, VehicleNumber, OwnerName, VehicleType
                FROM Vehicles
                ORDER BY VehicleId DESC;";

            var vehicles = new List<Vehicle>();
            using (MySqlConnection connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vehicles.Add(MapVehicle(reader));
                    }
                }
            }

            return vehicles;
        }

        public bool Update(Vehicle vehicle)
        {
            const string query = @"
                UPDATE Vehicles
                SET VehicleNumber = @VehicleNumber,
                    OwnerName = @OwnerName,
                    VehicleType = @VehicleType
                WHERE VehicleId = @VehicleId;";

            using (MySqlConnection connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    AddVehicleParameters(command, vehicle);
                    command.Parameters.AddWithValue("@VehicleId", vehicle.VehicleId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int vehicleId)
        {
            const string query = "DELETE FROM Vehicles WHERE VehicleId = @VehicleId;";

            using (MySqlConnection connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", vehicleId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        private static void AddVehicleParameters(MySqlCommand command, Vehicle vehicle)
        {
            command.Parameters.AddWithValue("@VehicleNumber", vehicle.VehicleNumber);
            command.Parameters.AddWithValue("@OwnerName", vehicle.OwnerName);
            command.Parameters.AddWithValue("@VehicleType", vehicle.VehicleType);
        }

        private static Vehicle MapVehicle(MySqlDataReader reader)
        {
            return new Vehicle
            {
                VehicleId = Convert.ToInt32(reader["VehicleId"]),
                VehicleNumber = reader["VehicleNumber"].ToString(),
                OwnerName = reader["OwnerName"].ToString(),
                VehicleType = reader["VehicleType"].ToString()
            };
        }
    }
}