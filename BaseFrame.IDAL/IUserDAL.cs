using BaseFrame.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFrame.IDAL
{
    public interface IUserDAL
    {
        User GetUserById(int id);
    }
}
