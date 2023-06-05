using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record Car: BaseEntity
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int TypeOfBodyId { get; set; }
        public int Mileage { get; set; }
        public int EngineId { get; set; }
        public int GearboxId { get; set; }
        public int TypeOfFuelId { get; set; }
        public string Description { get; set; }
        public string Vin { get; set; }

        public virtual IEnumerable<CarImage> Images { get; set; }
        public virtual IEnumerable<Question> Questions { get; set; }

    }
}
