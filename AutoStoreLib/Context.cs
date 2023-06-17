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
        public DbSet<CallRequest> CallRequests { get; set; }        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionMessage> QuestionMessages { get; set; }
        public DbSet<AnswerMessage> AnswerMessages { get; set; }


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

            builder.Entity<Answer>()
                .HasMany(answer => answer.Messages)
                .WithOne(message => message.Answer)
                .HasForeignKey(message => message.AnswerId);

            builder.Entity<Answer>()
                .HasOne(answer => answer.Question)
                .WithOne(question => question.Answer)
                .HasForeignKey<Answer>(answer => answer.QuestionId);

            builder.Entity<Answer>()
                .HasOne(answer => answer.Admin)
                .WithMany(admin => admin.Answers)
                .HasForeignKey(answer => answer.AdminId);

            builder.Entity<Question>()
                .HasMany(question => question.Messages)
                .WithOne(message => message.Question)
                .HasForeignKey(message => message.QuestionId);

            builder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId);

            builder.Entity<Order>()
                .HasOne(answeredOrder => answeredOrder.Admin)
                .WithMany(admin => admin.AnsweredOrders)
                .HasForeignKey(answeredOrder => answeredOrder.AdminId);

            builder.Entity<QuestionMessage>()
                .HasOne(message => message.User)
                .WithMany( user => user.QuestionMessages)
                .HasForeignKey(message => message.UserId);

            builder.Entity<AnswerMessage>()
                .HasOne(message => message.User)
                .WithMany(user => user.AnswerMessages)
                .HasForeignKey(message => message.UserId);

            builder.Entity<Role>().HasData(new Role(1, "User"), new Role(2, "Admin"));

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}