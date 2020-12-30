using System.Collections.Generic;

namespace Pumpkinmovies.ViewModels
{
    public class NewComment
    {
        public string MovieID { get; set; }
        public string UserID { get; set; }
        public float MovieRating { get; set; }
        public string MovieReview { get; set; }
    }
}
