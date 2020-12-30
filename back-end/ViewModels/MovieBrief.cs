using System.Collections.Generic;

namespace Pumpkinmovies.ViewModels
{
    public class MovieBrief
    {
        public string MovieID { get; set; }
        public string MovieName { get; set; }
        public string ReleaseTime { get; set; }
        public int MovieLength { get; set; }
        public string MovieArea { get; set; }
        public ViewRating MovieRating { get; set; }
        public string BestComment { get; set; }
        public List<ViewTag> MovieTagList { get; set; }
        public List<ViewPerson> MovieDirectorList { get; set; }
        public List<ViewPerson> MovieStarList { get; set; }
        public List<ViewPicture> MoviePictureList { get; set; }
    }

}
