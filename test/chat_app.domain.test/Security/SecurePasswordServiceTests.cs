using Xunit;
using Shouldly;
using chat_app.domain;


namespace chat_app.domain.test.Security
{
    public class SecurePasswordServiceTests
    {
        [Fact]
        public void When_The_Same_UserName_and_Password_Provided_Will_Allways_Compute_Different_Hash()
        {
            const string USER_NAME = "IAmUser";
            const string PASSWORD = "ThePassword123@";

            var sut = new SecurePasswordService ();

            var hash1 = sut.ComputeForUser (USER_NAME, PASSWORD).passwordHash;
            var hash2 = sut.ComputeForUser (USER_NAME, PASSWORD).passwordHash;

            ReferenceEquals (hash1, hash2).ShouldBeFalse ();
            hash1.Length.ShouldBe (hash2.Length);
            hash1.ShouldNotBe (hash2); // compares elements
        }

        [Fact]
        public void Challenges_Created_Hash_With_Result_True()
        {
            const string USER_NAME = "IAmUser";
            const string PASSWORD = "ThePassword123@";

            var sut = new SecurePasswordService ();

            var hashAndSalt = sut.ComputeForUser (USER_NAME, PASSWORD);
            var result = sut.ChallengePassword (USER_NAME, PASSWORD, hashAndSalt.passwordHash, hashAndSalt.salt);

            result.ShouldBeTrue ();
        }

        [Fact]
        public void Challenges_Invalid_Password_With_Hash_With_Result_False()
        {
            const string USER_NAME = "IAmUser";
            const string PASSWORD = "ThePassword123@";
            const string INVALID_PASSWORD = "KABOOM";

            var sut = new SecurePasswordService ();

            var hashAndSalt = sut.ComputeForUser (USER_NAME, PASSWORD);
            var result = sut.ChallengePassword (USER_NAME, INVALID_PASSWORD, hashAndSalt.passwordHash, hashAndSalt.salt);

            result.ShouldBeFalse ();
        }

        [Fact]
        public void Two_Users_With_Same_Password_Will_Have_Different_Hashes()
        {
            const string USER_NAME = "IAmUser";
            const string USER_NAME_2 = "IAmAnotherUser";
            const string PASSWORD = "ThePassword123@";

            var sut = new SecurePasswordService ();

            var hash1 = sut.ComputeForUser (USER_NAME, PASSWORD).passwordHash;
            var hash2 = sut.ComputeForUser (USER_NAME_2, PASSWORD).passwordHash;

            ReferenceEquals (hash1, hash2).ShouldBeFalse ();
            hash1.Length.ShouldBe (hash2.Length);
            hash1.ShouldNotBe (hash2); // compares elements
        }
    }
}
