using System;
using System.Linq;
using chat_app.domain.Data;
using chat_app.domain;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using Xunit;


namespace chat_app.domain.test.User
{
    public class ChatUserServiceTests
    {
        private const int HASH_LENGTH = 64;
        private const int SALT_LENGTH = 255;

        [Fact]
        public void AddNewUser_Adds_Valid_User_Record_To_Database()
        {
            const string USER = "IAmUser";
            const string PASSWORD = "password";
            byte[] passwordHash = Enumerable.Range (1, HASH_LENGTH).Select (i => (byte)i).ToArray ();
            byte[] salt = Enumerable.Range (1, SALT_LENGTH).Select (i => (byte)i).ToArray ();


            var securePasswordService = Substitute.For<ISecurePasswordService> ();
            securePasswordService.ComputeForUser (default, default)
                                 .ReturnsForAnyArgs ((passwordHash, salt));


            var sut = new ChatUserService (() => GetChatContext (), securePasswordService);
            sut.AddNewUser (USER, PASSWORD).GetAwaiter ().GetResult ();

            using (var db = GetChatContext ())
            {
                var result = db.ChatUsers.SingleOrDefault (x => x.Name == USER);
                result.ShouldNotBeNull ();
                result.PasswordHash.ShouldBe (passwordHash);
                result.PasswordSalt.ShouldBe (salt);
                result.Id.ShouldNotBe (Guid.Empty);
            }

        }

        [Fact]
        public void TryLogIn_Returns_False_Result_When_User_Not_In_Database()
        {
            const string USER = "ImNotHere";
            const string PASSWORD = "notImportant";

            var sut = new ChatUserService (() => GetChatContext (), Substitute.For<ISecurePasswordService> ());
            var result = sut.TryLogIn (USER, PASSWORD).GetAwaiter ().GetResult ();

            result.success.ShouldBeFalse ();
            result.user.ShouldBeNull ();
        }

        [Fact]
        public void TryLogIn_Returns_False_Result_When_User_Is_In_Database_But_Password_Is_Wrong()
        {
            const string USER = "LogMePlease";
            const string PASSWORD = "GOOD";
            byte[] passwordHash = Enumerable.Range (1, HASH_LENGTH).Select (i => (byte)i).ToArray ();
            byte[] salt = Enumerable.Range (1, SALT_LENGTH).Select (i => (byte)i).ToArray ();


            var securePasswordService = Substitute.For<ISecurePasswordService> ();
            securePasswordService.ComputeForUser (default, default)
                                 .ReturnsForAnyArgs ((passwordHash, salt));

            var chatService = new ChatUserService (() => GetChatContext (), securePasswordService);
            chatService.AddNewUser (USER, PASSWORD).GetAwaiter ().GetResult ();

            ChatUser inDbUser;
            using (var db = GetChatContext ())
            {
                inDbUser = db.ChatUsers.SingleOrDefault (x => x.Name == USER);
            }

            inDbUser.ShouldNotBeNull ();
            inDbUser.Name.ShouldBe (USER);
            inDbUser.PasswordHash.ShouldBe (passwordHash);
            inDbUser.PasswordSalt.ShouldBe (salt);
            inDbUser.Id.ShouldNotBe (Guid.Empty);


            securePasswordService.ChallengePassword (default, default, default, default)
                                 .ReturnsForAnyArgs (false);

            var sut = new ChatUserService (() => GetChatContext (), securePasswordService);
            var result = sut.TryLogIn (USER, PASSWORD).GetAwaiter ().GetResult ();

            result.success.ShouldBeFalse ();
            result.user.ShouldBeNull ();
        }

        [Fact]
        public void TryLogIn_Returns_True_Result_When_User_Is_In_Database_And_Password_Is_Correct()
        {
            const string USER = "IWillLogIn";
            const string PASSWORD = "GOOD";
            byte[] passwordHash = Enumerable.Range (1, HASH_LENGTH).Select (i => (byte)i).ToArray ();
            byte[] salt = Enumerable.Range (1, SALT_LENGTH).Select (i => (byte)i).ToArray ();


            var securePasswordService = Substitute.For<ISecurePasswordService> ();
            securePasswordService.ComputeForUser (default, default)
                                 .ReturnsForAnyArgs ((passwordHash, salt));

            var chatService = new ChatUserService (() => GetChatContext (), securePasswordService);
            chatService.AddNewUser (USER, PASSWORD).GetAwaiter ().GetResult ();

            ChatUser inDbUser;
            using (var db = GetChatContext ())
            {
                inDbUser = db.ChatUsers.SingleOrDefault (x => x.Name == USER);
            }

            inDbUser.ShouldNotBeNull ();
            inDbUser.Name.ShouldBe (USER);
            inDbUser.PasswordHash.ShouldBe (passwordHash);
            inDbUser.PasswordSalt.ShouldBe (salt);
            inDbUser.Id.ShouldNotBe (Guid.Empty);


            securePasswordService.ChallengePassword (default, default, default, default)
                                 .ReturnsForAnyArgs (true);

            var sut = new ChatUserService (() => GetChatContext (), securePasswordService);
            var result = sut.TryLogIn (USER, PASSWORD).GetAwaiter ().GetResult ();

            result.success.ShouldBeTrue ();
            result.user.ShouldNotBeNull ();
            ReferenceEquals (inDbUser, result.user).ShouldBeFalse ();
            result.user.Id.ShouldBe (inDbUser.Id);
            result.user.Name.ShouldBe (inDbUser.Name);
            result.user.PasswordHash.ShouldBe (inDbUser.PasswordHash);
            result.user.PasswordSalt.ShouldBe (inDbUser.PasswordSalt);
        }


        [Fact]
        public void Throws_When_Trying_To_Add_User_With_Same_Name_More_Than_Once()
        {
            const string USER = "User";

            using (var db = GetChatContext ())
            {
                db.ChatUsers.Add (
                    new ChatUser
                    {
                        Id = Guid.Empty,
                        Name = USER,
                        PasswordHash = new byte[64],
                        PasswordSalt = new byte[255]
                    });

                db.SaveChanges ();
            }

            var sut = new ChatUserService (() => GetChatContext (), Substitute.For<ISecurePasswordService> ());

            Assert.Throws<ArgumentException> (() => sut.AddNewUser (USER, "").GetAwaiter ().GetResult ());
        }

        private static ChatContext GetChatContext()
        {
            var options = new DbContextOptionsBuilder<ChatContext> ()
                .UseInMemoryDatabase (databaseName: "Chat_Db")
                .Options;

            return new ChatContext (options);
        }
    }
}
