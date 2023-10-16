using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Data.Entities
{
    public class Rental
    {
        [Key]
        public int ID { get; set; }
        public int CarID { get; set; }

        [ForeignKey("CarID")]
        public Car Car { get; set; }
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
        public int RentDurationDays { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public bool RequestForReturn { get; set; } = false;
    }
}
