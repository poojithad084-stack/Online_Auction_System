using CategoryService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CategoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        CategoryContext context;
        public CategoryController(CategoryContext ctx)
        {
            context = ctx;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return context.Category.ToList();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{CategoryName}")]
        public Category Get(string CategoryName)
        {
            return context.Category.Find(CategoryName);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post([FromBody] Category value)
        {
            context.Category.Add(value);
            context.SaveChanges();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{CategoryName}")]
        public void Put(string CategoryName, [FromBody] Category value)
        {
            var category = context.Category.Find(CategoryName);
            if (category != null)
            {
                category.CategoryName = value.CategoryName;
                category.Description = value.Description;
                category.status = value.status;
                category.DisplayOrder = value.DisplayOrder;
                category.CreatedDate = value.CreatedDate;
                category.UpdatedDate = value.UpdatedDate;
                context.SaveChanges();
            }
        }

        // DELETE api/<CategoryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
