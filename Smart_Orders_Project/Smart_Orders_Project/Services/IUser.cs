using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.Services
{
   public interface IUser<T>
    {
        Task<T> GetUser();
        Task<T> GetUserFromDB(string username, string password);
    }
}
