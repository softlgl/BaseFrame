using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseFrame.Entity;
using BaseFrame.IDAL;
using BaseFrame.IService;

namespace BaseFrame.Service
{
    public class UserService:IUserService
    {
        public IUserDAL UserDAL { get; set; }
        public User GetUserById(int id)
        {
            return UserDAL.GetUserById(id);
        }
    }
}
