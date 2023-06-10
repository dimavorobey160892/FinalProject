using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record Answer : BaseEntity
    {
        public Answer() { }
        public Answer(int userId, int adminId, int questionId, int carId, string title)
        {
            UserId = userId;
            Title = title;
            Date = DateTime.Now;
            AdminId = adminId;
            QuestionId = questionId;
            CarId = carId;
        }

        public int UserId { get; set; }
        public int AdminId { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
        public virtual Car Car { get; set; }
        public virtual List<AnswerMessage> Messages { get; set; }
    }
}
