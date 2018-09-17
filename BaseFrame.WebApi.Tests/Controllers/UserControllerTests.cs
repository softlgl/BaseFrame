using BaseFrame.Common.Autofac;
using BaseFrame.Entity;
using BaseFrame.IService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFrame.WebApi.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private readonly IUserService _userService = AutofacContainer.Resolve<IUserService>();

        [TestMethod]
        public void GetTest()
        {
            User user = _userService.GetUserById(1);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void PostTest()
        {
            Assert.Fail();
        }
    }
}