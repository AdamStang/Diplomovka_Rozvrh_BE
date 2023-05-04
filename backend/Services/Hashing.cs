using backend.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services
{
    public class Hashing
    {
        public static byte[] ToMD5(string str)
        {
            var md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return bytes;
        }

        public static byte[] ToSHA256(string str)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            return bytes;
        }

        public static string ToHash(string str, HashMethodEnum? method = HashMethodEnum.MD5)
        {
            var bytes = new byte[32];
            switch (method) 
            {
                case HashMethodEnum.MD5:
                    bytes = ToMD5(str);
                    break;
                case HashMethodEnum.SHA256:
                    bytes = ToSHA256(str);
                    break;
                default:
                    bytes = ToMD5(str);
                    break;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString());
            }
            return sb.ToString();
        }
    }
}
