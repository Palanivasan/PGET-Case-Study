using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Models
{
    class Reservation
    {
        public int ReservationId { get; set; }
        public int? AssetId { get; set; } 
        public int? EmployeeId { get; set; } 
        public DateTime ReservationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public Reservation() { }

        public Reservation(int assetId, int employeeId, DateTime reservationDate, DateTime startDate, DateTime endDate, string status)
        {
            AssetId = assetId;
            EmployeeId = employeeId;
            ReservationDate = reservationDate;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }

        public Reservation(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
