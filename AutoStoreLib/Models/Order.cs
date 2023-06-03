using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record Order : BaseEntity
    {
        public Order() { }
        public Order(int userId, string info)
        {
            UserId = userId;
            Info = info;
        }

        public int UserId { get; set; }
        public string Info { get; set; }
        public virtual User User { get; set; }

    }
}