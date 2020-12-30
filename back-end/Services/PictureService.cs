using Pumpkinmovies.Models;
using System.Linq;

namespace Pumpkinmovies.Services
{
    public class PictureService
    {
        private AppDbContext context;

        public PictureService(AppDbContext context)
        {
            this.context = context;
        }
        public int GetPictureNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").pic_num;
        }

    }
}
