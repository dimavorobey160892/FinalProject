using Microsoft.EntityFrameworkCore;
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
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
               .HasOne(user => user.Role)
               .WithMany(role => role.Users)
               .HasForeignKey(user => user.RoleId);

            //redo
            builder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId);

            builder.Entity<Question>()
                .HasOne(question => question.User)
                .WithMany(user => user.Questions)
                .HasForeignKey(order => order.UserId);

           

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=shopDb;" +
        //    "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
        //    "Trust Server Certificate=False;Application Intent=ReadWrite;" +
        //    "Multi Subnet Failover=False");
        //}
    }
}