using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : Controller
    {
		private HelloAppContext context;

		public BlogController(HelloAppContext context, IConfiguration configuration)
        {
			this.context = context;
			var cs = configuration.GetConnectionString("HelloApp");
		}

        [HttpGet]
        public IEnumerable<Blog> Get()
        {
			return context.Blog.ToList();
        }

		[HttpGet("{id}")]
		public Blog Get(long id)
		{
			return context.Blog.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
		}
	}
}
