using System;
using Asset_Management.Models;
using Asset_Management.Dao;
using Asset_Management.Exceptions;

namespace Asset_Management
{
    public class AssetManagementMenu
    {
        private readonly AssetManagementServiceImpl service;
        public AssetManagementMenu(AssetManagementServiceImpl service)
        {
            this.service = service;
        }

        public void AddAsset()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Type: ");
            string type = Console.ReadLine();
            Console.Write("Serial Number: ");
            string serial = Console.ReadLine();
            Console.Write("Purchase Date (yyyy-MM-dd): ");
            DateTime purchaseDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Location: ");
            string location = Console.ReadLine();
            Console.Write("Status: ");
            string status = Console.ReadLine();
            Console.Write("Owner ID (or press Enter to skip): ");
            string ownerInput = Console.ReadLine();
            int? ownerId = string.IsNullOrEmpty(ownerInput) ? null : int.Parse(ownerInput);

            var asset = new Asset(name, type, serial, purchaseDate, location, status, ownerId);
            Console.WriteLine(service.AddAsset(asset) ? "Asset added." : "Failed to add asset.");
        }

        public void UpdateAsset()
        {
            Console.Write("Enter Asset ID to update: ");
            int updateId = int.Parse(Console.ReadLine());

            try
            {
                if (!service.AssetExists(updateId))
                    throw new AssetNotFoundException($"Asset with ID {updateId} not found.");

                Console.Write("New Name: ");
                string newName = Console.ReadLine();
                Console.Write("New Type: ");
                string newType = Console.ReadLine();
                Console.Write("New Serial Number: ");
                string newSerial = Console.ReadLine();
                Console.Write("New Purchase Date (yyyy-MM-dd): ");
                DateTime newPurchase = DateTime.Parse(Console.ReadLine());
                Console.Write("New Location: ");
                string newLoc = Console.ReadLine();
                Console.Write("New Status: ");
                string newStatus = Console.ReadLine();
                Console.Write("New Owner ID (or press Enter to skip): ");
                string newOwnerInput = Console.ReadLine();
                int? newOwner = string.IsNullOrEmpty(newOwnerInput) ? null : int.Parse(newOwnerInput);

                Asset updated = new Asset(updateId, newName, newType, newSerial, newPurchase, newLoc, newStatus, newOwner);
                bool result = service.UpdateAsset(updated);
                Console.WriteLine(result ? "Updated." : "Update failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void DeleteAsset()
        {
            try
            {
                Console.Write("Enter Asset ID to delete: ");
                int delId = int.Parse(Console.ReadLine());

                if (!service.AssetExists(delId))
                    throw new AssetNotFoundException($"Asset with ID {delId} not found.");

                bool result = service.DeleteAsset(delId);
                Console.WriteLine(result ? "Deleted." : "Deletion failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void AllocateAsset()
        {
            try
            {
                Console.Write("Asset ID: ");
                int allocAsset = int.Parse(Console.ReadLine());

                if (!service.AssetExists(allocAsset))
                    throw new AssetNotFoundException($"Asset with ID {allocAsset} not found.");

                Console.Write("Employee ID: ");
                int allocEmp = int.Parse(Console.ReadLine());

                Console.Write("Allocation Date (yyyy-MM-dd): ");
                DateTime allocDate = DateTime.Parse(Console.ReadLine());

                bool result = service.AllocateAsset(allocAsset, allocEmp, allocDate);
                Console.WriteLine(result ? "Allocated." : "Allocation failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void DeallocateAsset()
        {
            try
            {
                Console.Write("Allocation ID: ");
                int allocationId = int.Parse(Console.ReadLine());

                if (!service.AllocationExists(allocationId))
                    throw new AssetNotFoundException($"Allocation with ID {allocationId} not found.");

                bool result = service.DeallocateAsset(allocationId);
                Console.WriteLine(result ? "Deallocated." : "Deallocation failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void PerformMaintenance()
        {
            try
            {
                Console.Write("Asset ID: ");
                int maintId = int.Parse(Console.ReadLine());

                if (!service.AssetExists(maintId))
                    throw new AssetNotFoundException($"Asset with ID {maintId} not found.");

                DateTime? lastMaintenanceDate = service.GetLastMaintenanceDate(maintId);

                if (lastMaintenanceDate.HasValue && lastMaintenanceDate.Value < DateTime.Now.AddYears(-2))
                {
                    throw new AssetNotMaintainException($"Asset with ID {maintId} has not been maintained for over 2 years.");
                }

                Console.Write("Maintenance Date (yyyy-MM-dd): ");
                DateTime maintDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Issue Description: ");
                string issue = Console.ReadLine();

                Console.Write("Cost: ");
                decimal cost = decimal.Parse(Console.ReadLine());

                bool result = service.PerformMaintenance(maintId, maintDate, issue, cost);
                Console.WriteLine(result ? "Maintenance logged." : "Failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (AssetNotMaintainException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }


        public void ReserveAsset()
        {
            try
            {
                Console.Write("Asset ID: ");
                int resAsset = int.Parse(Console.ReadLine());

                if (!service.AssetExists(resAsset))
                    throw new AssetNotFoundException($"Asset with ID {resAsset} not found.");

                Console.Write("Employee ID: ");
                int resEmp = int.Parse(Console.ReadLine());

                Console.Write("Reservation Date (yyyy-MM-dd): ");
                DateTime resDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.Write("End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Status: ");
                string resStatus = Console.ReadLine();

                bool result = service.ReserveAsset(resAsset, resEmp, resDate, startDate, endDate, resStatus);
                Console.WriteLine(result ? "Reserved." : "Reservation failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void WithdrawReservation()
        {
            try
            {
                Console.Write("Reservation ID: ");
                int cancelId = int.Parse(Console.ReadLine());

                if (!service.ReservationExists(cancelId))
                    throw new AssetNotFoundException($"Reservation with ID {cancelId} not found.");

                bool result = service.WithdrawReservation(cancelId);
                Console.WriteLine(result ? "Reservation cancelled." : "Failed.");
            }
            catch (AssetNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void ViewEmployee()
        {
            Console.Write("Enter Employee ID: ");
            int empId = int.Parse(Console.ReadLine());

            var employee = service.ViewEmployee(empId);  

            if (employee != null)
            {
                Console.WriteLine("\n- - - Employee Details - - -");
                Console.WriteLine($"ID: {employee.EmployeeId}");
                Console.WriteLine($"Name: {employee.Name}");
                Console.WriteLine($"Department: {employee.Department}");
                Console.WriteLine($"Email: {employee.Email}");
               // Console.WriteLine($"Password: {employee.Password}");
            }
            else
            {
                Console.WriteLine("No employee exists.");
            }
        }

        public void ViewAssetDetails()
        {
            Console.Write("Enter Asset ID: ");
            if (int.TryParse(Console.ReadLine(), out int assetId))
            {
                Asset asset = service.ViewAsset(assetId);
                if (asset != null)
                {
                    Console.WriteLine("Asset Details:");
                    Console.WriteLine($"ID: {asset.AssetId}");
                    Console.WriteLine($"Name: {asset.Name}");
                    Console.WriteLine($"Type: {asset.Type}");
                    Console.WriteLine($"Serial Number: {asset.SerialNumber}");
                    Console.WriteLine($"Purchase Date: {asset.PurchaseDate:yyyy-MM-dd}");
                    Console.WriteLine($"Location: {asset.Location}");
                    Console.WriteLine($"Status: {asset.Status}");
                    Console.WriteLine("Owner: " + (asset.OwnerId.HasValue ? asset.OwnerId.Value.ToString() : "None"));
                }
                else
                {
                    Console.WriteLine("No asset found with the provided ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Asset ID.");
            }
        }
    }
}
