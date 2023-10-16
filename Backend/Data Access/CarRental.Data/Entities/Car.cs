using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Data.Entities
{
    public class Car
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Capacity { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public Decimal Price { get; set; }
        public string FuelType { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
