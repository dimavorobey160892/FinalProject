using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public class Answer : BaseEntity
    {
        public Answer() { }
        public Answer(int adminId, string title)
        {
            Title = title;
            Date = DateTime.Now;
            AdminId = adminId;
        }

        public int AdminId { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public virtual User Admin { get; set; }
        public virtual Question Question { get; set; }
        public virtual List<AnswerMessage> Messages { get; set; }
    }
}
