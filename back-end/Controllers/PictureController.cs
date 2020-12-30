using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pumpkinmovies.Models;
using Pumpkinmovies.Services;
using Pumpkinmovies.ViewModels;

namespace Pumpkinmovies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureController:Controller
    {
        private AppDbContext context;
        private PictureService service;

        public PictureController(AppDbContext context)
        {
            this.context = context;
            this.service = new PictureService(context);
        }

        [Route("AddMoviePicture")]
        [HttpPost]
        public IActionResult AddMoviePicture([FromBody]NewMoviePicture newMoviePicture)
        {
            //新建图片信息
            var picture = new Picture {
                pic_id = (service.GetPictureNum() + 1).ToString(),
                pic_path = newMoviePicture.PicturePath,
                pic_info = newMoviePicture.PictureInfo
            };
            context.Picture.Attach(picture);
            context.SaveChanges();

            //新建联系集
            var moviePicture = new MoviePicture {
                m_id = newMoviePicture.MovieID,
                pic_id = picture.pic_id
            };
            context.MoviePicture.Attach(moviePicture);
            context.SaveChanges();

            return Ok(new
            {
                Success = true,
                MovieID = moviePicture.m_id,
                PictureID = moviePicture.pic_id,
                PicturePath = picture.pic_path,
                msg = "Movie Picture Added"
            });
        }

        [Route("AddPersonPicture")]
        [HttpPost]
        public IActionResult AddPersonPicture([FromBody] NewPersonPicture newPersonPicture)
        {
            //新建图片信息
            var picture = new Picture
            {
                pic_id = (service.GetPictureNum() + 1).ToString(),
                pic_path = newPersonPicture.PicturePath,
                pic_info = newPersonPicture.PictureInfo
            };
            context.Picture.Attach(picture);
            context.SaveChanges();

            //新建联系集
            var personPicture = new PersonPicture
            {
                person_id = newPersonPicture.PersonID,
                pic_id = picture.pic_id
            };
            context.PersonPicture.Attach(personPicture);
            context.SaveChanges();

            return Ok(new
            {
                Success = true,
                MovieID = personPicture.person_id,
                PictureID = personPicture.pic_id,
                PicturePath = picture.pic_path,
                msg = "Person Picture Added"
            });
        }

        [Route("AddMoviePicture")]
        [HttpPost]

    }
}
