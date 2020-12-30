using System.Collections.Generic;

namespace Pumpkinmovies.ViewModels
{
    public class MovieDetail
    {
        public string MovieID { get; set; }
        public string MovieName { get; set; }
        public string ReleaseTime { get; set; }
        public int MovieLength { get; set; }
        public string MovieArea { get; set; }
        public string MovieSummary { get; set; }
        public string MovieIMDb { get; set; }
        public ViewRating MovieRating { get; set; }
        public List<ViewComment> MovieCommentList { get; set; }
        public List<ViewTag> MovieTagList { get; set; }
        public List<ViewPerson> MovieDirectorList { get; set; }
        public List<ViewPerson> MovieStarList { get; set; }
        public List<ViewPicture> MoviePictureList { get; set; }
    }

    public class ViewPerson
    {
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string PersonArea { get; set; }
    }
    public class ViewComment
    {
        public string CommentID { get; set; }
        public string UserName { get; set; }
        public float Rating { get; set; }
        public string Review { get; set; }
        public int TotalLike { get; set; }
    }
    public class ViewTag
    {
        public string TagName { get; set; }
    }
    public class ViewPicture
    {
        public string PictureID { get; set; }
        public string PicturePath { get; set; }
        public string PictureInfo { get; set; }
    }
    public class ViewRating
    {
        public float AvgRating { get; set; }
        public int CommentNum { get; set; }
    }
}
