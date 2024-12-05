using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatMeister.Data
{
    internal class ChatMeisterDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet <Chat> Chats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=;" +
                "database=csd_chatmeister",
                ServerVersion.Parse("10.4.17-mariadb")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Alice",
                    Email = "alice@example.com"
                },
                new User
                {
                    Id = 2,
                    Username = "Bob",
                    Email = "bob@example.com"
                },
                new User
                {
                    Id = 3,
                    Username = "Anonymous User",
                    Email = "anonymous@example.com"
                }
            );

            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    Id = 1,
                    ChatId = 1,
                    UserId = 1,
                    Content = "Hello everyone!",
                    SentAt = DateTime.Parse("09:15 27-06-2023")
                },
                new Message
                {
                    Id = 2,
                    ChatId = 1,
                    UserId = 2,
                    Content = "Hey Alice!",
                    SentAt = DateTime.Parse("09:17 27-06-2023")
                },
                new Message
                {
                    Id = 3,
                    ChatId = 2,
                    UserId = 1,
                    Content = "We're doing great!",
                    SentAt = DateTime.Parse("10:32 27-07-2023")
                }
            );

            modelBuilder.Entity<Chat>().HasData(
                new Chat
                {
                    Id = 1,
                    Name = "Chatroom 1"
                },
                new Chat
                {
                    Id = 2,
                    Name = "Chatroom" 
                }
            );
        }
    }
}
