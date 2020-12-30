using Pumpkinmovies.Models;
using System.Linq;

namespace Pumpkinmovies.Services
{
    public class AccountService
    {
        private AppDbContext context;
        //private JwtSetting _jwtSetting;

        public AccountService(AppDbContext context)
        {
            this.context = context;
            //_jwtSetting = options.Value;
        }

        //查找用户名是否存在
        public bool CheckExist(string userName)
        {
            var user = context.User.FirstOrDefault(u => u.u_name == userName);
            if (user != null)
            {
                return true;
            }
            return false;
        }
        //判断密码是否正确
        public bool CheckPass(string userName, string password)
        {
            var user = context.User.FirstOrDefault(u => u.u_name == userName && u.u_password == password);
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public int GetUserNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").u_num;
        }
    }
}
