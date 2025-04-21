using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Models
{
    class AssetAllocation
    {
        public int allocationId { get; set; }
        public int? AssetId { get; set; } 
        public int? EmployeeId { get; set; } 
        public DateTime AllocationDate { get; set; }

        public AssetAllocation() { }

        public AssetAllocation(int? assetId, int? employeeId, DateTime allocationDate)
        {
            AssetId = assetId;
            EmployeeId = employeeId;
            AllocationDate = allocationDate;
        }

        public AssetAllocation(int allocationId)
        {
            this.allocationId = allocationId;
        }
    }
}
