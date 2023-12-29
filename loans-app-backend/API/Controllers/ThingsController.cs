using API.DataAccess;
using API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThingsController : ControllerBase
    {
        private readonly AppDbContext context;
        public ThingsController(AppDbContext context) => this.context = context;

        //  GET api/things
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Thing>>> GetAll()
        {
            return await this.context.Things.ToListAsync();
        }

        //  GET api/things/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Thing>> GetById(int id)
        {
            var thing = await this.context.FindAsync<Thing>(id);

            if (thing == null) return NotFound();

            return thing;
        }

        //  POST api/things
        [HttpPost]
        public async Task<ActionResult<Thing>> Create(Thing thing)
        {
            this.context.Attach(thing.Category);
            this.context.Things.Add(thing);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = thing.Id }, thing);
        }

        //  PUT api/things/
        [HttpPut]
        public async Task<ActionResult> Update(Thing thing)
        {
            this.context.Update(thing);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        //  DELETE api/things/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var thing = await this.context.FindAsync<Thing>(id);

            if (thing == null) return this.NotFound();

            this.context.Remove(thing);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
