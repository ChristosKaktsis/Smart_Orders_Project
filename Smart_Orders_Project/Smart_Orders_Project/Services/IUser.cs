using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.Services
{
   public interface IUser<T>
    {
        Task<T> GetUser();
        Task<T> GetUserFromDB(string username, string password);
    }
}
