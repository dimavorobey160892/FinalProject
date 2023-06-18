using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class Order : BaseEntity
    {
        public Order() { }

        public Order(int userId, string carModel, int minYear, int maxYear, 
            int minPrice, int maxPrice, int typeOfBodyId, int minMileage, 
            int maxMileage, double? minEngineСapacity, 
            double? maxEngineСapacity, int gearboxId, int typeOfFuelId, 
            string coments, int statusId)
        {
            UserId = userId;
            CarModel = carModel;
            MinYear = minYear;
            MaxYear = maxYear;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            TypeOfBodyId = typeOfBodyId;
            MinMileage = minMileage;
            MaxMileage = maxMileage;
            MinEngineСapacity = minEngineСapacity;
            MaxEngineСapacity = maxEngineСapacity;
            GearboxId = gearboxId;
            TypeOfFuelId = typeOfFuelId;
            Coments = coments;
            StatusId = statusId;
            DateCreaded = DateTime.Now;
        }

        public int UserId { get; set; }
        public string CarModel { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int TypeOfBodyId { get; set; }
        public int MinMileage { get; set; }
        public int MaxMileage { get; set; }
        public double? MinEngineСapacity { get; set; }
        public double? MaxEngineСapacity { get; set; }
        public int GearboxId { get; set; }
        public int TypeOfFuelId { get; set; }
        public string? Coments { get; set; }
        public int StatusId { get; set; }
        public DateTime DateCreaded { get; set; }
        public DateTime? DateAnswered { get; set; }
        public string? Answer { get; set; }
        public virtual User User { get; set; }

    }
}