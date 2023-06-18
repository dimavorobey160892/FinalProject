using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class Car: BaseEntity
    {
        public Car() { }

        public Car(string name, int year, int price, int typeOfBodyId, 
                   int mileage, double engineСapacity, int gearboxId, 
                   int typeOfFuelId, string description, string vin)
        {
            Name = name;
            Year = year;
            Price = price;
            TypeOfBodyId = typeOfBodyId;
            Mileage = mileage;
            EngineСapacity = engineСapacity;
            GearboxId = gearboxId;
            TypeOfFuelId = typeOfFuelId;
            Description = description;
            Vin = vin;
        }

        public string Name { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int TypeOfBodyId { get; set; }
        public int Mileage { get; set; }
        public double EngineСapacity { get; set; }
        public int GearboxId { get; set; }
        public int TypeOfFuelId { get; set; }
        public string Description { get; set; }
        public string Vin { get; set; }
        public int StatusId { get; set; }

        public virtual IEnumerable<CarImage> Images { get; set; }
        public virtual IEnumerable<Question> Questions { get; set; }

    }
}
