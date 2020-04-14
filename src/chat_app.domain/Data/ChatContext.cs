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
        public DbSet<PublicMessage> PublicMessages { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            ChatUserModel (modelBuilder);
            PublicMessageModel (modelBuilder);
            PrivateMessageModel (modelBuilder);
        }

        private static void PrivateMessageModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrivateMessage> ()
                        .HasKey (k => k.Id);

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.Id)
                        .ValueGeneratedNever ();

            modelBuilder.Entity<PrivateMessage> ()
                        .HasIndex (i => i.SenderId);

            modelBuilder.Entity<PrivateMessage> ()
                        .HasIndex (i => i.ReceiverId);

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.SenderId)
                        .IsRequired ();

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.ReceiverId)
                        .IsRequired ();

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.Sended)
                        .IsRequired ();

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.Received)
                        .IsRequired ();

            modelBuilder.Entity<PrivateMessage> ()
                        .Property (p => p.Text)
                        .IsRequired ();

        }

        private static void PublicMessageModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublicMessage> ()
                                    .HasKey (k => k.Id);

            modelBuilder.Entity<PublicMessage> ()
                        .Property (p => p.Id)
                        .ValueGeneratedNever ();

            modelBuilder.Entity<PublicMessage> ()
                        .HasIndex (i => i.When);

            modelBuilder.Entity<PublicMessage> ()
                        .Property (p => p.SenderId)
                        .IsRequired ();

            modelBuilder.Entity<PublicMessage> ()
                        .Property (p => p.When)
                        .IsRequired ();

            modelBuilder.Entity<PublicMessage> ()
                        .Property (p => p.Text)
                        .IsRequired ();
        }

        private static void ChatUserModel(ModelBuilder modelBuilder)
        {
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
