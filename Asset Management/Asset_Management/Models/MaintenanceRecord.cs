using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Models
{
    class MaintenanceRecord
    {
        public int? AssetId { get; set; } 
        public DateTime MaintenanceDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public MaintenanceRecord() { }

        public MaintenanceRecord(int assetId, DateTime maintenanceDate, string description, decimal cost)
        {
            AssetId = assetId;
            MaintenanceDate = maintenanceDate;
            Description = description;
            Cost = cost;
        }
    }
}
