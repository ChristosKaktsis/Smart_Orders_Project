using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
   public class User
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
