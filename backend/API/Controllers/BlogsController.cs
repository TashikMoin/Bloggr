﻿using API.Contracts;
using AutoMapper;
using DAL.Externals.BlogExternals;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private IBlogContract Blog_Provider { get; set; }
        private IMapper Mapper { get; set; }
        public BlogController(IBlogContract _Blog_Provider, IMapper _Mapper)
        {
            Mapper = _Mapper;
            Blog_Provider = _Blog_Provider;
        }

        [HttpGet("{id}", Name = "GetBlog")]
        public ActionResult<Blog> GetBlog(int Id)
        {
            var Blog = Blog_Provider.GetBlog(Id);
            if (Blog != null)
            {
                return Blog;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadBlog>> GetBlogs()
        {
            var Blogs = Blog_Provider.GetBlogs();
            return Ok(Mapper.Map<IEnumerable<ReadBlog>>(Blogs));
        }

        [Route("~/api/CreateBlog")]
        [HttpPost]
        public ActionResult<ReadBlog> CreateBlog(WriteBlog _New_Blog)
        {
            var New_Blog = Mapper.Map<Blog>(_New_Blog); 
            Blog_Provider.CreateBlog(New_Blog);
            Blog_Provider.SaveChanges();
            var Read_Blog = Mapper.Map<ReadBlog>(New_Blog);
            return CreatedAtRoute(nameof(GetBlog), new { Id = New_Blog.Blog_Id }, Read_Blog);
        }


        [HttpPut("{id}")]
        public ActionResult UpdateBlog(int Id, UpdateBlog UpdatedBlog)
        {
            var Blog = Blog_Provider.GetBlog(Id);
            if (Blog == null)
            {
                return NotFound();
            }
            Blog_Provider.UpdateBlog(Blog, UpdatedBlog);
            Blog_Provider.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteBlog(int Id)
        {
            var Blog = Blog_Provider.GetBlog(Id);
            if (Blog == null)
            {
                return NotFound();
            }
            Blog_Provider.DeleteBlog(Blog);
            Blog_Provider.SaveChanges();
            return NoContent();
        }



    }
}