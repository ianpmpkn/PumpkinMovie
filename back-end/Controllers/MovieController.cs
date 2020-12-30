using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pumpkinmovies.Models;
using Pumpkinmovies.Services;
using Pumpkinmovies.ViewModels;

namespace Pumpkinmovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController :Controller
    {
        private AppDbContext context;
        private MovieService service;
        public MovieController(AppDbContext context)
        {
            this.context = context;
            service = new MovieService(context);
        }

        /// <summary>
        /// 获取电影详情页信息
        /// </summary>
        /// <param name="MovieID"></param>
        /// <returns></returns>
        [Route("MovieDetail")]
        [HttpGet]
        public IActionResult MovieDetail(string MovieID)
        {
            return Ok(new
            {
                Success = true,
                MovieDetail = service.GetMovieDetail(MovieID),
                msg = "Operation Done"
            });
        }

        /// <summary>
        /// 通过电影名字模糊搜索
        /// </summary>
        /// <param name="MovieName"></param>
        /// <returns></returns>
        [Route("SearchName")]
        [HttpGet]
        public IActionResult SearchName(string MovieName)
        {
            var returnList = new List<MovieBrief> {};

            //模糊查询
            var movieList = context.Movie.Where(m => m.m_name.Contains(MovieName)).ToList();

            foreach (var temp in movieList)
            {
                returnList.Add(service.GetMovieBrief(temp.m_id));
            }
            return Ok(new
            {
                Success = true,
                MovieList = returnList,
                msg = "Operation Done"
            });
        }

        /// <summary>
        /// 获取一个Tag的电影，按评分降序排列
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        [Route("TagRank")]
        [HttpGet]
        public IActionResult TagRank(string TagName)
        {
            return Ok(new
            {
                Success = true,
                TagList = service.GetMovieByTag(TagName),
                msg = "Operation Done"
            });
        }

        /// <summary>
        /// 获取一个人物参与的电影，不重复
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        [Route("PersonRank")]
        [HttpGet]
        public IActionResult PersonRank(string PersonID)
        {
            return Ok(new
            {
                Success = true,
                TagList = service.GetMovieByPerson(PersonID),
                msg = "Operation Done"
            });
        }

        [Route("Special")]
        [HttpGet]
        public IActionResult Special(string UserID)
        {
            var tagList = context.UserPrefer.ToLookup(up => up.u_id)[UserID].ToList();
            var movieList = new List<MovieBrief> { };
            if (tagList.Count() == 0)
            {
                movieList = service.GetAllMovie();
            }
            else
            {
                movieList = service.GetMovieByTag(tagList[0].tag_name);
            }
            return Ok(new
            {
                Success = true,
                SpecialList = movieList,
                msg = "Operation Done"
            });
        }

        [Route("AddMovie")]
        [HttpPost]
        public IActionResult AddMovie([FromBody]NewMovie newMovie)
        {
            if (!service.CheckMovieExist(newMovie.MovieIMDb))
            {
                //新建电影
                var movie = new Movie { };
                movie.m_id = (service.GetMovieNum() + 1).ToString();
                movie.m_name = newMovie.MovieName;
                movie.release_time = newMovie.ReleaseTime;
                movie.m_length = newMovie.MovieLength;
                movie.m_area = newMovie.MovieArea;
                movie.m_summary = newMovie.MovieSummary;
                movie.m_imdb = newMovie.MovieIMDb;

                context.Movie.Add(movie);
                context.SaveChanges();

                //全局信息
                var totalinfo = context.TotalInfo.Find("pumpkinmovies");
                totalinfo.m_num += 1;
                context.TotalInfo.Attach(totalinfo);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    MovieID = movie.m_id,
                    msg = "Movie Added"
                });
            }
            else
            {//电影重复
                return Ok(new
                {
                    Success = false,
                    msg = "Same IMDb"
                });
            }
        }

        [Route("AddTag")]
        [HttpPost]
        public IActionResult AddTag(string TagName)
        {
            //添加新tag本身
            if (!service.CheckTagExist(TagName))
            {
                var tag = new Tag { tag_name = TagName };

                context.Tag.Add(tag);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    msg = "Tag Added"
                });
            }
            //tag已存在
            else
                return Ok(new
                {
                    Success = false,
                    msg = "Tag Existed"
                });
        }

        [Route("AddMovieTag")]
        [HttpPost]
        public IActionResult AddMovieTag([FromBody]NewMovieTag newMovieTag)
        {
            //tag不存在
            if (!service.CheckTagExist(newMovieTag.TagName))
            {
                return Ok(new
                {
                    Success = false,
                    msg = "Tag doesn't exist"
                });
                
            }
            else
            {   //tag存在且未被添加到该电影
                if (!service.CheckMovieTagExist(newMovieTag.MovieID, newMovieTag.TagName)) 
                {
                    var movietag = new MovieTag
                    {
                        m_id = newMovieTag.MovieID,
                        tag_name = newMovieTag.TagName
                    };

                    context.MovieTag.Add(movietag);
                    context.SaveChanges();

                    return Ok(new
                    {
                        Success = true,
                        msg = "MovieTag added"
                    });
                }
                //该电影已经有了这个tag
                else
                    return Ok(new
                    {
                        Success = false,
                        msg = "Movie already have this tag"
                    });
            }
               
        }

        [Route("AddStar")]
        [HttpPost]
        public IActionResult AddStar([FromBody] NewPerson newPerson)
        {
            if (!service.CheckPersonExist(newPerson.PersonIMDb))
            {
                //新建人物
                var person = new Person { };
                person.person_id = (service.GetPersonNum() + 1).ToString();
                person.person_name = newPerson.PersonName;
                person.person_area = newPerson.PersonArea;
                person.person_imdb = newPerson.PersonIMDb;

                context.Person.Add(person);
                context.SaveChanges();

                //创建联系集
                var star = new Star { };
                star.m_id = newPerson.MovieID;
                star.person_id = person.person_id;

                context.Star.Add(star);
                context.SaveChanges();

                //全局信息
                var totalinfo = context.TotalInfo.Find("pumpkinmovies");
                totalinfo.person_num += 1;
                context.TotalInfo.Attach(totalinfo);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    MovieID = newPerson.MovieID,
                    PersonID = person.person_id,
                    msg = "New person, star Added"
                });
            }
            else
            {
                //人物已存在
                var person = context.Person.FirstOrDefault(p => p.person_imdb == newPerson.PersonIMDb);
                if (!service.CheckStarExist(person.person_id, newPerson.MovieID))
                {
                    //添加到该电影主演
                    var star = new Star { };
                    star.m_id = newPerson.MovieID;
                    star.person_id = person.person_id;

                    context.Star.Add(star);
                    context.SaveChanges();

                    return Ok(new
                    {
                        Success = true,
                        MovieID = newPerson.MovieID,
                        PersonID = person.person_id,
                        msg = "Person exists, star added"
                    });
                }
                else
                    return Ok(new
                    {
                        Success = false,
                        MovieID = newPerson.MovieID,
                        PersonID = person.person_id,
                        msg = "Star exists"
                    });
            }
        }

        [Route("AddDirector")]
        [HttpPost]
        public IActionResult AddDirector([FromBody] NewPerson newPerson)
        {
            if (!service.CheckPersonExist(newPerson.PersonIMDb))
            {
                //新建人物
                var person = new Person { };
                person.person_id = (service.GetPersonNum() + 1).ToString();
                person.person_name = newPerson.PersonName;
                person.person_area = newPerson.PersonArea;
                person.person_imdb = newPerson.PersonIMDb;

                context.Person.Add(person);
                context.SaveChanges();

                //创建联系集
                var director = new Director { };
                director.m_id = newPerson.MovieID;
                director.person_id = person.person_id;

                context.Director.Add(director);
                context.SaveChanges();

                //全局信息
                var totalinfo = context.TotalInfo.Find("pumpkinmovies");
                totalinfo.person_num += 1;
                context.TotalInfo.Attach(totalinfo);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    MovieID = newPerson.MovieID,
                    PersonID = person.person_id,
                    msg = "New person, director Added"
                });
            }
            else
            {
                //人物已存在
                var person = context.Person.FirstOrDefault(p => p.person_imdb == newPerson.PersonIMDb);
                if (!service.CheckDirectorExist(person.person_id, newPerson.MovieID))
                {
                    //添加到该电影导演
                    var director = new Director { };
                    director.m_id = newPerson.MovieID;
                    director.person_id = person.person_id;

                    context.Director.Add(director);
                    context.SaveChanges();

                    return Ok(new
                    {
                        Success = true,
                        MovieID = newPerson.MovieID,
                        PersonID = person.person_id,
                        msg = "Person exists, director added"
                    });
                }
                else
                    return Ok(new
                    {
                        Success = false,
                        MovieID = newPerson.MovieID,
                        PersonID = person.person_id,
                        msg = "Director exists"
                    });
            }
        }
        
    }
}
