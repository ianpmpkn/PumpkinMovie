using Microsoft.Extensions.Options;
using Pumpkinmovies.Models;
using Pumpkinmovies.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pumpkinmovies.Services
{
    public class CommentService
    {
        private AppDbContext context;
        public CommentService(AppDbContext context)
        {
            this.context = context;
        }

        public Comment GetComment(string CommentID)
        {
            return context.Comment.FirstOrDefault(c => c.c_id == CommentID);
        }
        public int GetCommentNum()
        {
            return context.TotalInfo.Find("pumpkinmovies").c_num;
        }

        public LikeComment GetLike(string UserId, string CommentID)
        {
            return context.LikeComment.FirstOrDefault(lc => lc.u_id == UserId && lc.c_id == CommentID);
        }
    }
}
