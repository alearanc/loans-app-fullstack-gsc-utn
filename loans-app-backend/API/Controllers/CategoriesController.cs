using API.DataAccess;
using API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext context;
        public CategoriesController(AppDbContext context) => this.context = context;

        //  GET api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            return await this.context.Categories.ToListAsync();
        }

        //  GET api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await this.context.FindAsync<Category>(id);

            if (category == null) return NotFound();

            return category;
        }

        //  POST api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> Create(Category category)
        {
            this.context.Categories.Add(category);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        //  PUT api/categories
        [HttpPut]
        public async Task<ActionResult> Update(Category category)
        {
            this.context.Update(category);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        //  DELETE api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await this.context.FindAsync<Category>(id);

            if (category == null) return this.NotFound();

            this.context.Remove(category);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
