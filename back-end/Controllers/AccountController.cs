using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pumpkinmovies.Models;
using Pumpkinmovies.Services;
using Pumpkinmovies.ViewModels;

namespace Pumpkinmovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private AppDbContext context;
        private AccountService service;

        public AccountController(AppDbContext context)
        {
            this.context = context;
            this.service = new AccountService(context);
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] RegisterUser RUser)
        {
            if (!service.CheckExist(RUser.UserName))
            {
                //新建用户
                var user = new User { };
                user.u_id = (service.GetUserNum() + 1).ToString();
                user.u_name = RUser.UserName;
                user.u_password = RUser.Password;
                user.u_email = RUser.Email;
                user.u_type = "GM";
                context.User.Add(user);
                context.SaveChanges();

                //全局信息
                var totalinfo = context.TotalInfo.Find("pumpkinmovies");
                totalinfo.u_num += 1;
                context.TotalInfo.Attach(totalinfo);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    UserID = user.u_id,
                    UserName = user.u_name,
                    msg = "User Created"
                });
            }
            else
            {//该账号密码存在
                return Ok(new
                {
                    Success = false,
                    msg = "Same UserName"
                });
            }
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser LUser)
        {
            if (service.CheckPass(LUser.UserName, LUser.Password))
            {//登录成功
                var user = context.User
                    .FirstOrDefault(u => u.u_name == LUser.UserName && u.u_password == LUser.Password);

                return Ok(new
                {
                    Success = true,
                    UserID = user.u_id,
                    UserName = user.u_name,
                    msg = "Login"
                });
            }
            else
            {//账号或密码错误
                return Ok(new
                {
                    Success = false,
                    msg = "Wrong UserName or Password"
                });
            }
        }

        [Route("Test")]
        [HttpPost]
        public IActionResult Test([FromBody] RegisterUser RUser)
        {
            service.GetUserNum();
            return Ok(new
            {
                Success = true,
            });
        }
    }
}
