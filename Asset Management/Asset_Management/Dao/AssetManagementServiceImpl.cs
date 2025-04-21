using Asset_Management.Exceptions;
using Asset_Management.Models;
using Asset_Management.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Dao
{
    public class AssetManagementServiceImpl : IAssetManagementService
    {
        public bool AddAsset(Asset asset)
        {
            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Assets (Name, Type, Serial_Number, Purchase_Date, Location, Status, Owner_Id) " +
                        "VALUES ( @Name, @Type, @Serial_Number, @Purchase_Date, @Location, @Status, @Owner_Id)", conn);

                    cmd.Parameters.AddWithValue("@Name", asset.Name);
                    cmd.Parameters.AddWithValue("@Type", asset.Type);
                    cmd.Parameters.AddWithValue("@Serial_Number", asset.SerialNumber);
                    cmd.Parameters.AddWithValue("@Purchase_Date", asset.PurchaseDate);
                    cmd.Parameters.AddWithValue("@Location", asset.Location);
                    cmd.Parameters.AddWithValue("@Status", asset.Status);
                    cmd.Parameters.AddWithValue("@Owner_Id", (object?)asset.OwnerId ?? DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                   // Console.WriteLine("Rows inserted: " + rowsAffected);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddAsset: " + ex.Message);
                return false;
            }
        }

        public bool UpdateAsset(Asset asset)
        {
            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Assets
                             SET Name = @Name,
                                 Type = @Type,
                                 Serial_Number = @Serial_Number,
                                 Purchase_Date = @Purchase_Date,
                                 Location = @Location,
                                 Status = @Status,
                                 Owner_Id = @Owner_Id
                             WHERE Asset_Id = @Asset_Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", asset.Name);
                        cmd.Parameters.AddWithValue("@Type", asset.Type);
                        cmd.Parameters.AddWithValue("@Serial_Number", asset.SerialNumber);
                        cmd.Parameters.AddWithValue("@Purchase_Date", asset.PurchaseDate);
                        cmd.Parameters.AddWithValue("@Location", asset.Location);
                        cmd.Parameters.AddWithValue("@Status", asset.Status);
                        cmd.Parameters.AddWithValue("@Owner_Id", (object)asset.OwnerId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Asset_Id", asset.AssetId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateAsset: " + ex.Message);
                return false;
            }
        }

        public bool DeleteAsset(int assetId)
        {
            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM Assets WHERE Asset_Id = @Asset_Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Asset_Id", assetId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteAsset: " + ex.Message);
                return false;
            }
        }

        public bool AllocateAsset(int assetId, int employeeId, DateTime allocationDate)
        {
            try
            {
                var allocation = new AssetAllocation(assetId, employeeId, allocationDate);

                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO AssetAllocations 
                             (Asset_Id, Employee_Id, Allocation_Date, Return_Date)
                             VALUES 
                             (@Asset_Id, @Employee_Id, @Allocation_Date, NULL)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Asset_Id", allocation.AssetId);
                        cmd.Parameters.AddWithValue("@Employee_Id", allocation.EmployeeId);
                        cmd.Parameters.AddWithValue("@Allocation_Date", allocation.AllocationDate);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AllocateAsset: " + ex.Message);
                return false;
            }
        }

        public bool DeallocateAsset(int allocationId)
        {
            try
            {
                var deallocation = new AssetAllocation(allocationId);

                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    var fetchCmd = new SqlCommand(@"
                        SELECT Asset_Id, Employee_Id, Allocation_Date, Return_Date
                        FROM AssetAllocations
                        WHERE Allocation_Id = @Allocation_Id", conn);
                    fetchCmd.Parameters.AddWithValue("@Allocation_Id", deallocation.allocationId);

                    using (var reader = fetchCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("No allocation found for the given ID.");
                            return false;
                        }

                        if (reader["Return_Date"] == DBNull.Value)
                        {
                            Console.WriteLine("Return date is already null for this allocation.");
                            return false;
                        }

                        reader.Close();

                        var updateCmd = new SqlCommand(@"
                              UPDATE AssetAllocations
                              SET Return_Date = NULL
                              WHERE Allocation_Id = @Allocation_Id", conn);
                        updateCmd.Parameters.AddWithValue("@Allocation_Id", allocationId);

                        int rows = updateCmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeallocateAsset: " + ex.Message);
                return false;
            }
        }

        public bool PerformMaintenance(int assetId, DateTime maintenanceDate, string description, decimal cost)
        {
            try
            {
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Assets WHERE asset_id = @Asset_Id", conn);
                    checkCmd.Parameters.AddWithValue("@Asset_Id", assetId);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                        throw new AssetNotFoundException($"Asset {assetId} not found.");

                    var record = new MaintenanceRecord(assetId, maintenanceDate, description, cost);

                    var cmd = new SqlCommand(@"
                INSERT INTO MaintenanceRecords (asset_id, maintenance_date, description, cost)
                VALUES (@Asset_Id, @Maintenance_Date, @Description, @Cost)", conn);

                    cmd.Parameters.AddWithValue("@Asset_Id", record.AssetId);
                    cmd.Parameters.AddWithValue("@Maintenance_Date", record.MaintenanceDate);
                    cmd.Parameters.AddWithValue("@Description", record.Description);
                    cmd.Parameters.AddWithValue("@Cost", record.Cost);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PerformMaintenance: " + ex.Message);
                throw;
            }
        }

        public bool ReserveAsset(int assetId, int employeeId, DateTime reservationDate, DateTime startDate, DateTime endDate, string status)
        {
            try
            {
                var reservation = new Reservation(assetId, employeeId, reservationDate, startDate, endDate, status);

                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Reservations
                             (Asset_Id, Employee_Id, Reservation_Date, Start_Date, End_Date, Status)
                             VALUES
                             (@Asset_Id, @Employee_Id, @Reservation_Date, @Start_Date, @End_Date, @Status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Asset_Id", reservation.AssetId);
                        cmd.Parameters.AddWithValue("@Employee_Id", reservation.EmployeeId);
                        cmd.Parameters.AddWithValue("@Reservation_Date", reservation.ReservationDate);
                        cmd.Parameters.AddWithValue("@Start_Date", reservation.StartDate);
                        cmd.Parameters.AddWithValue("@End_Date", reservation.EndDate);
                        cmd.Parameters.AddWithValue("@Status", reservation.Status);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ReserveAsset: " + ex.Message);
                return false;
            }
        }


        public bool WithdrawReservation(int reservationId)
        {
            try
            {
                var reservation = new Reservation(reservationId);
                using (SqlConnection conn = DBConnUtil.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Reservations
                             SET Status = 'canceled'
                             WHERE Reservation_Id = @ReservationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationId", reservationId);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in WithdrawReservation: " + ex.Message);
                return false;
            }
        }

        public bool AssetExists(int assetId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Assets WHERE asset_id = @Asset_Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Asset_Id", assetId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool AllocationExists(int allocationId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM AssetAllocations WHERE allocation_id = @Allocation_Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Allocation_Id", allocationId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool ReservationExists(int reservationId)
        {
            using (var conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                using var cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations WHERE reservation_id = @Id",
                    conn);
                cmd.Parameters.AddWithValue("@Id", reservationId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public Employee ViewEmployee(int employeeId)
        {
            using (var conn = DBConnUtil.GetConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Employees WHERE Employee_Id = @EmployeeId", conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee(
                            (int)reader["Employee_Id"],
                            reader["Name"].ToString(),
                            reader["Department"].ToString(),
                            reader["Email"].ToString(),
                            new string('*', reader["Password"].ToString().Length)
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Asset ViewAsset(int assetId)
        {
            using (SqlConnection conn = DBConnUtil.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Assets WHERE Asset_Id = @AssetId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssetId", assetId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int? ownerId = reader.IsDBNull(reader.GetOrdinal("Owner_Id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Owner_Id"));

                            return new Asset(
                                (int)reader["Asset_Id"],
                                reader["Name"].ToString(),
                                reader["Type"].ToString(),
                                reader["Serial_Number"].ToString(),
                                (DateTime)reader["Purchase_Date"],
                                reader["Location"].ToString(),
                                reader["Status"].ToString(),
                                ownerId
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public DateTime? GetLastMaintenanceDate(int assetId)
        {
            using (var conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            SELECT TOP 1 Maintenance_Date 
            FROM MaintenanceRecords 
            WHERE Asset_Id = @Asset_Id 
            ORDER BY Maintenance_Date DESC", conn);
                cmd.Parameters.AddWithValue("@Asset_Id", assetId);

                var result = cmd.ExecuteScalar();
                return result == DBNull.Value ? (DateTime?)null : (DateTime?)result;
            }
        }

    }
}
