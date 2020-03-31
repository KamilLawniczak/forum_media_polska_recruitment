namespace chat_app.domain
{
    public interface ISecurePasswordService
    {
        bool ChallengePassword(string userName, string password, byte[] passwordHash, byte[] salt);
        (byte[] passwordHash, byte[] salt) ComputeForUser(string userName, string password);
    }
}