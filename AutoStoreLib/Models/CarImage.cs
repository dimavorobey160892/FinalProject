using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class CarImage: BaseEntity
    {
        public CarImage() { }
        public CarImage(int carId, byte[] image)
        {
            CarId = carId;
            Image = image;
        }
        public int CarId { get; set; }
        public byte[] Image { get; set; }

        public virtual Car Car { get; set; }
    }
}
