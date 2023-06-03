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
        public int TypeOfEngineId { get; set; }
        public int TypeOfBodyId { get; set; }

        public string Description { get; set; }
    }
}
