using System;
using Asset_Management.Dao;

namespace Asset_Management
{
    class AssetManagementApp
    {
        static void Main(string[] args)
        {
            var service = new AssetManagementServiceImpl();
            var menu = new AssetManagementMenu(service);
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n- - - - - Asset Management - - - - -");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Update Asset");
                Console.WriteLine("3. Delete Asset");
                Console.WriteLine("4. Allocate Asset");
                Console.WriteLine("5. Deallocate Asset");
                Console.WriteLine("6. Perform Maintenance");
                Console.WriteLine("7. Reserve Asset");
                Console.WriteLine("8. Withdraw Reservation");
                Console.WriteLine("9. View Employee Details");
                Console.WriteLine("10. View Asset Details");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": 
                            menu.AddAsset(); 
                            break;
                        case "2": 
                            menu.UpdateAsset(); 
                            break;
                        case "3": 
                            menu.DeleteAsset(); 
                            break;
                        case "4": 
                            menu.AllocateAsset(); 
                            break;
                        case "5": 
                            menu.DeallocateAsset(); 
                            break;
                        case "6": 
                            menu.PerformMaintenance(); 
                            break;
                        case "7": 
                            menu.ReserveAsset(); 
                            break;
                        case "8": 
                            menu.WithdrawReservation(); 
                            break;
                        case "9":
                            menu.ViewEmployee();
                            break;
                        case "10":
                            menu.ViewAssetDetails();
                            break;
                        case "0":
                            Console.WriteLine("Thank you.");
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
