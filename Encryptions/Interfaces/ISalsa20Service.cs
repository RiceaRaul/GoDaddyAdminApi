namespace Encryptions.Interfaces
{
    public interface ISalsa20Service
    {
        string Encrypt(string target);
        string Decrypt(string target);
    }
}
