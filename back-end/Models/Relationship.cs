using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pumpkinmovies.Models
{
    public class MovieTag
    {
        [Key, Column(Order = 1)]
        public string m_id { get; set; }
        [Key, Column(Order = 2)]
        public string tag_name { get; set; }
    }
    public class MoviePicture
    {
        [Key, Column(Order = 1)]
        public string m_id { get; set; }
        [Key, Column(Order = 2)]
        public string pic_id { get; set; }
    }
    public class PersonPicture
    {
        [Key, Column(Order = 1)]
        public string person_id { get; set; }
        [Key, Column(Order = 2)]
        public string pic_id { get; set; }
    }
    public class Director
    {
        [Key, Column(Order = 1)]
        public string m_id { get; set; }
        [Key, Column(Order = 2)]
        public string person_id { get; set; }
    }
    public class Star
    {
        [Key, Column(Order = 1)]
        public string m_id { get; set; }
        [Key, Column(Order = 2)]
        public string person_id { get; set; }
    }

    public class LikeComment
    {
        [Key, Column(Order = 1)]
        public string u_id { get; set; }
        [Key, Column(Order = 2)]
        public string c_id { get; set; }
    }
    public class UserPrefer
    {
        [Key, Column(Order = 1)]
        public string u_id { get; set; }
        [Key, Column(Order = 2)]
        public string tag_name { get; set; }
        public float fit { get; set; }
    }

}
