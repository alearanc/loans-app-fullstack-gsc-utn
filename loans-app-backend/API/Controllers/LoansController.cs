using API.DataAccess;
using API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly AppDbContext context;
        public LoansController(AppDbContext context) => this.context = context;

        //  GET api/loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetAll()
        {
            return await this.context.Loans.ToListAsync();
        }

        //  GET api/loans/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetById(int id)
        {
            var loan = await this.context.FindAsync<Loan>(id);

            if (loan == null) return NotFound();

            return loan;
        }

        //  POST api/loans
        [HttpPost]
        public async Task<ActionResult<Loan>> Create(Loan loan)
        {
            this.context.Attach(loan.Person);
            this.context.Attach(loan.Thing);
            this.context.Loans.Add(loan);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
        }

        //  PUT api/loans
        [HttpPut]
        public async Task<ActionResult> Update(Loan loan)
        {
            this.context.Update(loan);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        //  DELETE api/loans/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var loan = await this.context.FindAsync<Loan>(id);

            if (loan == null) return this.NotFound();

            this.context.Remove(loan);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
