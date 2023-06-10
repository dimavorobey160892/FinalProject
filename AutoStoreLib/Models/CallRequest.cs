using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record CallRequest: BaseEntity
    {
        public CallRequest() { }
        public CallRequest(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Date = DateTime.Now;
            IsCompleted = false;
        }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
    }
}
