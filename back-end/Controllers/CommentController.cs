using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pumpkinmovies.Models;
using Pumpkinmovies.Services;
using Pumpkinmovies.ViewModels;

namespace Pumpkinmovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController:Controller
    {
        private AppDbContext context;
        private CommentService Cservice;
        private MovieService Mservice;
        public CommentController(AppDbContext context)
        {
            this.context = context;
            Cservice = new CommentService(context);
            Mservice = new MovieService(context);
        }

        [Route("Comment")]
        [HttpPost]
        public IActionResult Comment([FromBody] NewComment newComment)
        {
            //创建新评论
            var comment = new Comment { };
            comment.c_id = (Cservice.GetCommentNum() + 1).ToString();
            comment.u_id = newComment.UserID;
            comment.m_id = newComment.MovieID;
            comment.rating = newComment.MovieRating;
            comment.review = newComment.MovieReview;
            comment.total_like = 0;

            context.Comment.Add(comment);
            context.SaveChanges();

            //更新评论者偏好
            var tagList = Mservice.GetTagByMovie(newComment.MovieID);
            foreach (var temp in tagList)
            {
                var UserPrefer = context.UserPrefer.FirstOrDefault(up => up.u_id == comment.u_id && up.tag_name == temp.TagName);
                if (UserPrefer != null)
                {
                    //偏好更新
                    UserPrefer.fit += comment.rating;

                    context.UserPrefer.Attach(UserPrefer);
                    context.SaveChanges();
                }
                else
                {
                    //首次获得偏好值
                    var newUserPrefer = new UserPrefer
                    {
                        u_id = comment.u_id,
                        tag_name = temp.TagName,
                        fit = comment.rating
                    };

                    context.UserPrefer.Add(newUserPrefer);
                    context.SaveChanges();
                }
            }

            //更新全局信息
            var totalinfo = context.TotalInfo.Find("pumpkinmovies");
            totalinfo.c_num += 1;
            context.TotalInfo.Attach(totalinfo);
            context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Comment = comment,
                msg = "Operation Done"
            });
        }

        [Route("Like")]
        [HttpPost]
        public IActionResult Like(string UserID, string CommentID)
        {
            //首先查找是否有现有的点赞
            var like = Cservice.GetLike(UserID, CommentID);
            var comment = Cservice.GetComment(CommentID);

            //该用户已经赞了这个评论
            if (like != null)
            {
                //取消赞
                context.LikeComment.Remove(like);
                comment.total_like--;
                context.Comment.Attach(comment);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    msg = "DisLiked"
                });
            }
            //创建新的赞
            else
            {
                like = new LikeComment
                {
                    c_id = CommentID,
                    u_id = UserID
                };

                context.LikeComment.Add(like);
                comment.total_like = +1;
                context.Attach(comment);
                context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    msg = "Liked"
                });
            }
        }
    }
}
