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
        public Question(int userId, string title, string text)
        {
            UserId = userId;
            Title = title;
            Text = text;
        }

        public int UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CarId { get; set; }
        public virtual User User { get; set; }
        public virtual Car Car { get; set; }
    }
}