using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class QuestionMessage : BaseEntity
    {
        public QuestionMessage() { }
        public QuestionMessage(int userId, string text)
        {
            UserId = userId;
            Text = text;
            Date = DateTime.Now;
        }

        public string Text { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
    }
}
