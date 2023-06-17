using AutoStoreLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record User : BaseEntity
    {
        public User()
        {
        }

        public User(string firstName, string lastName, int age,
            string gender, string? address, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Gender = gender;
            Address = address;
            Email = email;
            Password = password;
            RoleId = (int)RolesEnum.User;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Order> AnsweredOrders { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual Role Role { get; set; }
        public virtual List<QuestionMessage> QuestionMessages { get; set; }
        public virtual List<AnswerMessage> AnswerMessages { get; set; }

    }
}