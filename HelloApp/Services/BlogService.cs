using HelloApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HelloApp.Services
{
	interface IBlogService
	{
		Blog Get(long id);
	}
	public class BlogService : IBlogService
	{
		private HelloAppContext context;

		public BlogService(HelloAppContext context)
		{
			this.context = context;
		}

		public Blog Get(long id)
		{
			return context.Blog.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
		}
	}
}
