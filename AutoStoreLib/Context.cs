﻿using Microsoft.EntityFrameworkCore;
using AutoStoreLib.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AutoStoreLib
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<CallRequest> CallRequests { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
               .HasOne(user => user.Role)
               .WithMany(role => role.Users)
               .HasForeignKey(user => user.RoleId);

            builder.Entity<CarImage>()
                .HasOne(img => img.Car)
                .WithMany(car => car.Images)
                .HasForeignKey(img => img.CarId);

            builder.Entity<Question>()
            .HasOne(question => question.User)
            .WithMany(user => user.Questions)
            .HasForeignKey(question => question.UserId);

            builder.Entity<Question>()
                .HasOne(question => question.Car)
                .WithMany(car => car.Questions)
                .HasForeignKey(question => question.CarId);

            //redo
            builder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId);

            builder.Entity<Role>().HasData(new Role(1, "User"), new Role(2, "Admin"));
        }
    }
}