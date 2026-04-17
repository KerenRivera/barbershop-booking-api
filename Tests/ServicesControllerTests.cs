using Microsoft.EntityFrameworkCore;
using Barbershop_booking.Controllers;
using Barbershop_booking.Data;
using Barbershop_booking.Models;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Tests
{
    public class ServicesControllerTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenNoServices()
        {
            var context = GetInMemoryContext();
            var controller = new ServicesController(context);

            var result = controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            var services = result.Value as List<Service>;
            Assert.Empty(services);
        }

        [Fact]
        public void GetAll_ReturnsServices_WhenServicesExist()
        {
            var context = GetInMemoryContext();
            context.Services.Add(new Service { Name = "Corte", Description = "Corte clasico", Price = 500, DurationMinutes = 30 });
            context.SaveChanges();
            var controller = new ServicesController(context);

            var result = controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            var services = result.Value as List<Service>;
            Assert.Single(services);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var context = GetInMemoryContext();
            var controller = new ServicesController(context);

            var result = controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetById_ReturnsService_WhenExists()
        {
            var context = GetInMemoryContext();
            context.Services.Add(new Service { Id = 1, Name = "Corte", Description = "Test", Price = 500, DurationMinutes = 30 });
            context.SaveChanges();
            var controller = new ServicesController(context);

            var result = controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            var service = result.Value as Service;
            Assert.Equal("Corte", service.Name);
        }

        [Fact]
        public void Create_AddsService()
        {
            var context = GetInMemoryContext();
            var controller = new ServicesController(context);
            var service = new Service { Name = "Barba", Description = "Recorte", Price = 300, DurationMinutes = 20 };

            var result = controller.Create(service);

            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(context.Services);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var context = GetInMemoryContext();
            var controller = new ServicesController(context);

            var result = controller.Update(99, new Service { Name = "Test" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            var context = GetInMemoryContext();
            var controller = new ServicesController(context);

            var result = controller.Delete(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_RemovesService_WhenExists()
        {
            var context = GetInMemoryContext();
            context.Services.Add(new Service { Id = 1, Name = "Corte", Description = "Test", Price = 500, DurationMinutes = 30 });
            context.SaveChanges();
            var controller = new ServicesController(context);

            var result = controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Services);
        }
    }
}