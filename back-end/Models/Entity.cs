using System.ComponentModel.DataAnnotations;

namespace Pumpkinmovies.Models
{
    public class Movie
    {
        [Key]
        public string m_id { get; set; }
        public string m_name { get; set; }
        public string release_time { get; set; }
        public int m_length { get; set; }
        public string m_area { get; set; }
        public string m_summary { get; set; }
        public string m_imdb { get; set; }
    }
    public class User
    {
        [Key]
        public string u_id { get; set; }
        public string u_name { get; set; }
        public string u_email { get; set; }
        public string u_password { get; set; }
        public string u_type { get; set; }
    }
    public class Comment
    {
        [Key]
        public string c_id { get; set; }
        public string u_id { get; set; }
        public string m_id { get; set; }
        public float rating { get; set; }
        public string review { get; set; }
        public int total_like { get; set; }
    }
    public class Tag
    {
        [Key]
        public string tag_name { get; set; }
    }

    public class Picture
    {
        [Key]
        public string pic_id { get; set; }
        public string pic_path { get; set; }
        public int pic_width { get; set; }
        public int pic_height { get; set; }
        public string pic_info { get; set; }
    }

    public class Person
    {
        [Key]
        public string person_id { get; set; }
        public string person_name { get; set; }
        public string person_area { get; set; }
        public string person_imdb { get; set; }
    }
    public class TotalInfo
    {
        [Key]
        public string table_name { get; set; }
        public int u_num { get; set; }
        public int m_num { get; set; }
        public int c_num { get; set; }
        public int pic_num { get; set; }
        public int person_num { get; set; }
    }
}