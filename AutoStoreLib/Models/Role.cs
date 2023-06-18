using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }

        public virtual IEnumerable<User> Users { get; set;}

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
