using BloggingDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloggingAPI.Models
{
    public class BlogViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Location { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BlogBindingModel
    {
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
        public string Location { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BlogModelMapper
    {
        public static BlogViewModel GetBlogView(Blog blog)
        {
            var blogViewModel = new BlogViewModel()
            {
                Id = blog.Id,
                Title = blog.Title,
                Body = blog.Body,
                Location = blog.Location,
                CreatedBy = blog.CreatedBy,
                CreatedOn = blog.CreatedOn,
                UpdatedBy = blog.UpdatedBy,
                UpdatedOn = blog.UpdatedOn,
                IsActive = blog.IsActive
            };
            return blogViewModel;
        }
    }
}