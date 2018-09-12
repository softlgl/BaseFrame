using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseFrame.Entity;
using BaseFrame.IDAL;

namespace BaseFrame.DAL
{
    public class UserDAL:IUserDAL
    {
        public User GetUserById(int id)
        {
            return new User();
        }
    }
}
