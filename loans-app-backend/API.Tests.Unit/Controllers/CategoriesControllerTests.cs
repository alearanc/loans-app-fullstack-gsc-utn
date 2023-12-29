using API.Controllers;
using API.DataAccess;
using API.Domain;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Unit.Controllers
{
    internal class CategoriesControllerTests : IDisposable
    {
        private readonly SqliteConnection connection = new("Filename=:memory:");
        private readonly AppDbContext context;
        private readonly CategoriesController sut;

        public CategoriesControllerTests()
        {
            this.connection.Open();

            var options = new DbContextOptionsBuilder()
                .UseSqlite(this.connection)
                .Options;

            this.context = new AppDbContext(options);

            this.sut = new CategoriesController(this.context);
        }

        public void Dispose() => this.connection.Dispose();

        public class TheMethod_GetAll : CategoriesControllerTests
        {
            [Fact]
            public async Task Should_return_all_three_init_categories()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<IEnumerable<Category>> actual = await this.sut.GetAll();

                // assert
                actual.Value.Should().HaveCount(3);
            }

        }

        public class TheMethod_GetById : CategoriesControllerTests
        {
            [Fact]
            public async Task Should_return_category_with_Id_equals_two()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<Category?> actual = await this.sut.GetById(2);

                // assert
                actual.Value.Should().NotBeNull();

                Category categoryTwo = actual.Value!;

                using (new AssertionScope())
                {
                    categoryTwo.Id.Should().Be(2);
                    categoryTwo.Description.Should().Be("computación");
                    categoryTwo.CreationDate.Should().HaveYear(1993).And.HaveMonth(9).And.HaveDay(12);
                }
            }

            [Fact]
            public async Task Should_return_NotFound_when_category_does_not_exists()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult<Category?> actual = await this.sut.GetById(100);

                // assert
                actual.Result.Should().BeOfType<NotFoundResult>();
                actual.Value.Should().BeNull();
            }
        }

        public class TheMethod_Delete : CategoriesControllerTests
        {
            [Fact]
            public async Task Should_remove_category_with_Id_equals_one()
            {
                // arrange
                await this.InitAsync();

                // act
                ActionResult actual = await this.sut.Delete(1);

                // assert
                actual.Should().BeOfType<NoContentResult>();

                var categoryOne = await this.context.FindAsync<Category>(1);
                categoryOne.Should().BeNull();
            }
        }


        private async Task InitAsync()
        {
            await this.context.AddRangeAsync(
                new Category()
                {
                    Description = "libros",
                    CreationDate = new DateOnly(1995, 6, 2)
                },
                new Category()
                {
                    Description = "computación",
                    CreationDate = new DateOnly(1993, 9, 12),
                },
                new Category()
                {
                    Description = "audio",
                    CreationDate = new DateOnly(2001, 2, 27),
                });

            await this.context.SaveChangesAsync();
        }
    }
}
