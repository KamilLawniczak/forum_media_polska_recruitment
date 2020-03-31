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
                        .HasIndex (i => i.Name)
                        .IsUnique ();

            modelBuilder.Entity<ChatUser> ()
                        .Property (p => p.Id)
                        .ValueGeneratedNever ();

            modelBuilder.Entity<ChatUser> ()
                        .Property (p => p.Name)
                        .HasMaxLength (100)
                        .IsRequired ();

            modelBuilder.Entity<ChatUser> ()
                        .Property (p => p.PasswordHash)
                        .HasMaxLength (64)
                        .IsRequired ();

            modelBuilder.Entity<ChatUser> ()
                        .Property (p => p.PasswordSalt)
                        .HasMaxLength (255)
                        .IsRequired ();
        }
    }
}
