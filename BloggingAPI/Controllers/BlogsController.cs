using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BloggingAPI.Models;
using BloggingDataAccess;
using Microsoft.AspNet.Identity;

namespace BloggingAPI.Controllers
{
    [Authorize]
    public class BlogsController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (BloggingDBEntities entities = new BloggingDBEntities())
                {
                    List<BlogViewModel> blogs = new List<BlogViewModel>();
                    var entity = entities.Blogs.ToList();
                    foreach (Blog blog in entity)
                    {
                        blogs.Add(BlogModelMapper.GetBlogView(blog));
                    }
                    return Ok(blogs);
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                using (BloggingDBEntities entities = new BloggingDBEntities())
                {
                    var entity = entities.Blogs.FirstOrDefault(b => b.Id == id);
                    if (entity != null)
                    {
                        return Ok(entity);
                    }
                    else
                    {
                        return Content(HttpStatusCode.NotFound, "Blog with Id = " + id.ToString() + " not Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        public IHttpActionResult Post([FromBody] BlogBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model != null)
                {
                    var blog = new Blog() { Title = model.Title, Body = model.Body, Location = model.Location};
                    blog.CreatedBy = User.Identity.GetUserName();
                    blog.CreatedOn = DateTime.Now;
                    if (blog.IsActive != true)
                        blog.IsActive = false;
                    using (BloggingDBEntities entities = new BloggingDBEntities())
                    {
                        entities.Blogs.Add(blog);
                        entities.SaveChanges();
                        var blogViewModel = BlogModelMapper.GetBlogView(blog);
                        return Created(new Uri(Request.RequestUri + blog.Id.ToString()), blogViewModel);
                    }
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Data is required");
                }
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        public IHttpActionResult Put(int id, [FromBody] BlogBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model != null)
                {
                    using (BloggingDBEntities entities = new BloggingDBEntities())
                    {
                        var entity = entities.Blogs.FirstOrDefault(b => b.Id == id);
                        if (entity == null)
                        {
                            return Content(HttpStatusCode.NotFound, "Blog with id = " + id.ToString() + " not found to update");
                        }
                        else
                        {
                            entity.Title = model.Title;
                            entity.Body = model.Body;
                            entity.Location = model.Location;
                            entity.UpdatedOn = DateTime.Now;
                            entity.UpdatedBy = User.Identity.GetUserName();
                            if (model.IsActive != null)
                                entity.IsActive = model.IsActive;
                            entities.SaveChanges();
                            var blogViewModel = BlogModelMapper.GetBlogView(entity);
                            return Ok(blogViewModel);
                        }
                    }
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Data is required");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        public IHttpActionResult Delete(int id)
        {
            try
            {
                using (BloggingDBEntities entities = new BloggingDBEntities())
                {
                    var entity = entities.Blogs.FirstOrDefault(b => b.Id == id);
                    if (entity == null)
                    {
                        return Content(HttpStatusCode.NotFound, "Blog with id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Blogs.Remove(entity);
                        entities.SaveChanges();
                        return Ok();
                    }
                }
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
