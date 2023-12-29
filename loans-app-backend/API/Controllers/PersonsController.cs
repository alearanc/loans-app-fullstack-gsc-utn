using API.DataAccess;
using API.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly AppDbContext context;
        public PersonsController(AppDbContext context) => this.context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll()
        {
            return await this.context.Persons.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var person = await this.context.FindAsync<Person>(id);

            if (person == null) return NotFound();

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Create(Person person)
        {
            this.context.Persons.Add(person);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Person person)
        {
            this.context.Update(person);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await this.context.FindAsync<Person>(id);

            if (person == null) return this.NotFound();

            this.context.Remove(person);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
