using BaseFrame.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFrame.IService
{
    public interface IUserService
    {
        User GetUserById(int id);
    }
}
