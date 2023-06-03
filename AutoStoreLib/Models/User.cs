﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoreLib.Models
{
    public record User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Question> Questions { get; set; }


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
        }
    }
}