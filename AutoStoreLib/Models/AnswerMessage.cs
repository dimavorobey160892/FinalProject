﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record AnswerMessage : BaseEntity
    {
        public AnswerMessage() { }
        public AnswerMessage(int userId, string text, int answerId)
        {
            UserId = userId;
            Text = text;
            Date = DateTime.Now;
            AnswerId = answerId;
        }

        public string Text { get; set; }
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
