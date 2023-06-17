using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record Question : BaseEntity
    {
        public Question() { }
        public Question(int userId, string title, int carId)
        {
            UserId = userId;
            Title = title;
            CarId = carId;
            Date = DateTime.Now;
        }

        public int UserId { get; set; }
        public string Title { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Car Car { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual List<QuestionMessage> Messages { get; set; }
    }
}