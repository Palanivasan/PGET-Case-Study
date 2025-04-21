using Asset_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Dao
{
        public interface IAssetManagementService
        {
            bool AddAsset(Asset asset);

            bool UpdateAsset(Asset asset);

            bool DeleteAsset(int assetId);

            bool AllocateAsset(int assetId, int employeeId, DateTime allocationDate);

            bool DeallocateAsset(int allocate_id);

            bool PerformMaintenance(int assetId, DateTime maintenanceDate, string description, decimal cost);

            bool ReserveAsset(int assetId, int employeeId, DateTime reservationDate, DateTime startDate, DateTime endDate, string status);

            bool WithdrawReservation(int reservationId);

            public Asset ViewAsset(int assetId);

            public Employee ViewEmployee(int employeeId);
        }
}
