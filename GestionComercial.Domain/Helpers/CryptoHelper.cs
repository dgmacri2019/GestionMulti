using System.Security.Cryptography;
using System.Text;

namespace GestionComercial.Domain.Helpers
{
    public class CryptoHelper
    {
        protected static readonly string EncryptionKey = "J$$sjkadgklasdgajkldUHGHJAGJ#jllll2kH332HJ!G@SASDFFF@sdffsdf@5454sdasda6@#ssdf65sf43AA241A#";

        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
                string result = string.Empty;
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        public static string Decrypt(string encrytText)
        {
            try
            {
                encrytText = encrytText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(encrytText);
                string result = string.Empty;

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        result = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
