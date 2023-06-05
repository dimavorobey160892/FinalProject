using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record CarImage: BaseEntity
    {
        public int CarId { get; set; }
        public byte[] Image { get; set; }

        public virtual Car Car { get; set; }
    }
}
