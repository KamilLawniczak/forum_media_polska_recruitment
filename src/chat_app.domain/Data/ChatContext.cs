using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace chat_app.domain.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext([NotNull] DbContextOptions options) : base (options)
        {
        }

        public ChatContext()
        {
        }

        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            modelBuilder.Entity<ChatUser> ()
                        .HasKey (k => k.Id);

            modelBuilder.Entity<ChatUser> ()
                        .HasIndex (i => i.Name);

            modelBuilder.Entity<ChatUser> ()
                        .Property (b => b.Name).HasMaxLength (100);

            modelBuilder.Entity<ChatUser> ()
                        .Property (b => b.PasswordHash).HasMaxLength (64);

            modelBuilder.Entity<ChatUser> ()
                        .Property (b => b.PasswordSalt).HasMaxLength (255);
        }
    }
}
