using backend.Models.Enums;
using System.Security.Authentication;

namespace backend.Services
{
    public interface IHashing
    {
        string ToHash(string str, HashMethodEnum? method = HashMethodEnum.MD5);
    }
}