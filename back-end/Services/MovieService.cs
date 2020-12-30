using Pumpkinmovies.Models;
using Pumpkinmovies.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Pumpkinmovies.Services
{
    public class MovieService
    {
        private AppDbContext context;

        public MovieService(AppDbContext context)
        {
            this.context = context;
        }

        public Movie GetMovie(string MovieID)
        {
            return context.Movie.FirstOrDefault(m => m.m_id == MovieID);
        }

        public int GetMovieNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").m_num;
        }

        public int GetPersonNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").person_num;
        }
        public int GetPictureNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").pic_num;
        }

        //检查电影是否已存在
        public bool CheckMovieExist(string IMDb)
        {
            var movie = context.Movie.FirstOrDefault(m => m.m_imdb == IMDb);
            if (movie != null)
            {
                return true;
            }
            return false;
        }

        //检查人物是否已存在
        public bool CheckPersonExist(string IMDb)
        {
            var person = context.Person.FirstOrDefault(p => p.person_imdb == IMDb);
            if (person != null)
            {
                return true;
            }
            return false;
        }

        //检查Tag是否已存在
        public bool CheckTagExist(string TagName)
        {
            var tag = context.Tag.FirstOrDefault(t => t.tag_name == TagName);
            if (tag != null)
            {
                return true;
            }
            return false;
        }
        public bool CheckMovieTagExist(string MovieID, string TagName)
        {
            var movietag = context.MovieTag.FirstOrDefault(mt => mt.m_id == MovieID && mt.tag_name == TagName);
            if (movietag != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckStarExist(string PersonID , string MovieID)
        {
            var star = context.Star.FirstOrDefault(s => s.person_id == PersonID && s.m_id == MovieID);
            if (star != null)
            {
                return true;
            }
            return false;
        }
        public bool CheckDirectorExist(string PersonID, string MovieID)
        {
            var director = context.Director.FirstOrDefault(s => s.person_id == PersonID && s.m_id == MovieID);
            if (director != null)
            {
                return true;
            }
            return false;
        }

        //通过电影ID获取平均分和评论数量
        public ViewRating GetMovieAvgRating(string MovieID)
        {
            var comments = context.Comment.ToLookup(c => c.m_id)[MovieID].ToList();           
            int commentNum = 0;
            float totalrating = 0;
            foreach (var temp in comments)
            {
                totalrating += temp.rating;
                commentNum += 1;
            }
           
            return new ViewRating
            {
                AvgRating = totalrating / commentNum,
                CommentNum = commentNum
            };
        }

        //通过电影ID查找主演
        public List<ViewPerson> GetStarByMovie(string MovieID)
        {
            var persons = context.Star.ToLookup(s => s.m_id)[MovieID].ToList();
            var returnList = new List<ViewPerson> { };

            foreach (var temp in persons)
            {
                Person tempperson = context.Person.FirstOrDefault(p => p.person_id == temp.person_id);
                var add = new ViewPerson
                {
                    PersonID = tempperson.person_id,
                    PersonArea = tempperson.person_area,
                    PersonName = tempperson.person_name
                };
                returnList.Add(add);
            }
                return returnList;
        }
        
        //通过电影ID查找导演
        public List<ViewPerson> GetDirectorByMovie(string MovieID)
        {
            var persons = context.Director.ToLookup(d => d.m_id)[MovieID].ToList();
            var returnList = new List<ViewPerson> { };

            foreach (var temp in persons)
            {
                Person tempperson = context.Person.FirstOrDefault(p => p.person_id == temp.person_id);
                var add = new ViewPerson
                {
                    PersonID = tempperson.person_id,
                    PersonArea = tempperson.person_area,
                    PersonName = tempperson.person_name
                };
                returnList.Add(add);
            }
            return returnList;
        }

        //通过电影ID查找tag
        public List<ViewTag> GetTagByMovie(string MovieID)
        {
            var tags = context.MovieTag.ToLookup(mt => mt.m_id)[MovieID].ToList();
            var returnList = new List<ViewTag> { }; 

            foreach (var temp in tags)
            {
                var add = new ViewTag
                {
                    TagName = temp.tag_name
                };
                returnList.Add(add);
            }
            return returnList;
        }

        //通过电影ID查找图片
        public List<ViewPicture> GetPictureByMovie(string MovieID)
        {
            var pictures = context.MoviePicture.ToLookup(mp => mp.m_id)[MovieID].ToList();
            var returnList = new List<ViewPicture> { };

            foreach (var temp in pictures)
            {
                Picture temppicture = context.Picture.FirstOrDefault(p => p.pic_id == temp.pic_id);
                var add = new ViewPicture
                {
                    PictureID = temppicture.pic_id,
                    PicturePath = temppicture.pic_path,
                    PictureInfo = temppicture.pic_info
                };
                returnList.Add(add);
            }
            return returnList;
        }

        //通过电影ID查找所有评论
        public List<ViewComment> GetCommentByMovie(string MovieID)
        {
            //找到评论
            var comments = context.Comment.ToLookup(mc => mc.m_id)[MovieID].ToList();
            var returnList = new List<ViewComment> { };

            foreach (var temp in comments)
            {
                //找到评论
                //Comment tempcomment = context.Comment.FirstOrDefault(c => c.c_id == temp.c_id);
                //找到联系集
                //UserComment userR = context.UserComment.FirstOrDefault(uc => uc.c_id == tempcomment.c_id);

                //找到评论者
                User tempuser = context.User.FirstOrDefault(u => u.u_id == temp.u_id);

                var add = new ViewComment
                {
                    UserName = tempuser.u_name,
                    CommentID = temp.c_id,
                    Rating = temp.rating,
                    Review = temp.review,
                    TotalLike = temp.total_like
                };
                returnList.Add(add);
            }
            return returnList;
        }

        //通过电影ID查找最佳评论
        public string GetBestComment(string MovieID)
        {
            //按赞数递减序排列
            var commentList = GetCommentByMovie(MovieID);
            if (commentList.Count != 0) 
            {
                commentList = commentList.OrderByDescending(c => c.TotalLike).ToList();
                return commentList[0].Review;
            }
            return "全新电影";
            
        }

        //通过电影ID获得详情页面
        public MovieDetail GetMovieDetail(string MovieID)
        {
            var movie = GetMovie(MovieID);

            return new MovieDetail 
            {
            //获取基本信息
            MovieID = movie.m_id,
            MovieName = movie.m_name,
            ReleaseTime = movie.release_time,
            MovieLength = movie.m_length,
            MovieArea = movie.m_area,
            MovieSummary = movie.m_summary,
            MovieIMDb = movie.m_imdb,

            //获取平均评分和评论量
            MovieRating = GetMovieAvgRating(MovieID),

            //获取导演和演员信息
            MovieDirectorList = GetDirectorByMovie(MovieID),
            MovieStarList = GetStarByMovie(MovieID),

            //获取tag
            MovieTagList = GetTagByMovie(MovieID),

            //获取图片
            MoviePictureList = GetPictureByMovie(MovieID),

            //获取所有评论
            MovieCommentList = GetCommentByMovie(MovieID),
            };
        }

        //通过电影ID获得简略页面
        public MovieBrief GetMovieBrief(string MovieID)
        {
            var movie = GetMovie(MovieID);

            return new MovieBrief
            {
                //获取基本信息
                MovieID = movie.m_id,
                MovieName = movie.m_name,
                MovieArea = movie.m_area,
                MovieLength = movie.m_length,
                ReleaseTime = movie.release_time,

                //获取平均评分和评论量
                MovieRating = GetMovieAvgRating(movie.m_id),

                //获取导演和演员信息
                MovieDirectorList = GetDirectorByMovie(movie.m_id),
                MovieStarList = GetStarByMovie(movie.m_id),

                //获取tag
                MovieTagList = GetTagByMovie(movie.m_id),

                //获取图片
                MoviePictureList = GetPictureByMovie(movie.m_id),

                //获取最佳评论
                BestComment = GetBestComment(movie.m_id)
            };
        }

        //通过tag获得电影排行，按评分降序排列
        public List<MovieBrief> GetMovieByTag(string TagName)
        {
            var returnList = new List<MovieBrief> { };
            var movieList = context.MovieTag.ToLookup(mt => mt.tag_name)[TagName].ToList();
            foreach (var temp in movieList)
            {
                returnList.Add(GetMovieBrief(temp.m_id));
            }
            if (returnList.Count != 0)
            {
                returnList = returnList.OrderByDescending(rl => rl.MovieRating.AvgRating).ToList();
            }
            return returnList;
        }

        //通过人物获得电影，保证不重复
        public List<MovieBrief> GetMovieByPerson(string PersonID)
        {
            var returnList = new List<MovieBrief> { };
            var movieIDList = new List<string> { };

            var starList = context.Star.ToLookup(s => s.person_id)[PersonID].ToList();
            var directorList = context.Director.ToLookup(d => d.person_id)[PersonID].ToList();

            foreach (var temp in starList)
            {
                movieIDList.Add(temp.m_id);
            }

            foreach (var temp in directorList)
            {
                movieIDList.Add(temp.m_id);
            }

            //保证movieID不重复
            movieIDList = movieIDList.Distinct().ToList();

            foreach (var temp in movieIDList)
            {
                returnList.Add(GetMovieBrief(temp));
            }

            return returnList;
        }

        //获得所有电影，按评分降序排列
        public List<MovieBrief> GetAllMovie()
        {
            var returnList = new List<MovieBrief> { };
            var movieList = context.Movie.ToList();
            foreach (var temp in movieList)
            {
                returnList.Add(GetMovieBrief(temp.m_id));
            }
            if (returnList.Count != 0)
            {
                returnList = returnList.OrderByDescending(rl => rl.MovieRating.AvgRating).ToList();
            }
            return returnList;
        }

    }
}
