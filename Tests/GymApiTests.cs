using System;
using Moq;
using Xunit;
using SportApp.Models;
using SportApp.Controllers;
using SportApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SportApp.Services;
using Newtonsoft.Json;

namespace Tests
{
    public class GymApiTests
    {
        [Fact]
        public void GetAllReturnsJsonResult()
        {
            // Arrange
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            gymsRepo.Setup(repo => repo.GetAll()).Returns(TestEntities.Gyms);
            services.Setup(service => service.Filter<Gym>(TestEntities.Gyms, "", null)).Returns(TestEntities.Gyms);
            services.Setup(service => service.Sort<Gym>(TestEntities.Gyms, "", "", null)).Returns(TestEntities.Gyms);
            services.Setup(service => service.Partition<Gym>(TestEntities.Gyms, 0, 0)).Returns(TestEntities.Gyms);
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller.Get();

            // Assert
            Assert.IsType(typeof(ObjectResult), response);
            Assert.NotNull(((ObjectResult)response).Value);
        }

        [Fact]
        public void GetGymByValidIdReturnsGym()
        {
            // Arrange
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(gym.Id)).Returns(gym);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller.Get(gym.Id);

            //Assert
            Assert.IsType(typeof(ObjectResult), response);
            Assert.IsType(typeof(Gym), ((ObjectResult)response).Value);
            var actualGym = (Gym)((ObjectResult)response).Value;
            Assert.Equal(gym, actualGym);
        }

        [Fact]
        public void GetGymByWrongIdReturns404()
        {
            // Arrange
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            // Act
            var response = controller.Get(20000);

            // Assert
            Assert.IsType(typeof(JsonResult), response);
            Assert.Equal(404, controller.Response.StatusCode);
        }
        [Fact]
        public void AddNullReturnsBadRequest()
        {
            // Arrange 
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Post(null);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), postResponse);
        }
        [Fact]
        public void AddInvalidGymReturnsBadRequest()
        {
            // Arrange 
            var gym = TestEntities.Gyms[2];
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            controller.ModelState.AddModelError("", "Error");

            // Act
            var postResponse = controller.Post(gym);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), postResponse);
        }
        [Fact]
        public void AddValidGymReturnsObjectResult()
        {
            // Arrange
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(gym.Id)).Returns(gym);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Post(gym);

            // Assert
            Assert.IsType(typeof(CreatedAtRouteResult), postResponse);
            Assert.IsType(typeof(Gym), ((ObjectResult)postResponse).Value);
            var actualGym = (Gym)((ObjectResult)postResponse).Value;
            Assert.Equal(gym, actualGym);
        }

        // Update
        [Fact]
        public void UpdateValidGymValidIdReturnsNoContent()
        {
            // Arrange 
            var updatedGym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(updatedGym.Id)).Returns(updatedGym);
            gymsRepo.Setup(repo => repo.Edit(updatedGym)).Returns(updatedGym);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var putResponse = controller.Put(updatedGym.Id, updatedGym);

            // Assert
            Assert.IsType(typeof(NoContentResult), putResponse);
        }
        [Fact]
        public void UpdateNullReturnsBadRequest()
        {
            // Arrange 
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object);

            // Act
            var putResponse = controller.Put(1, null);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), putResponse);
        }
        [Fact]
        public void UpdateValidGymWrongIdBadRequest()
        {
            // Arrange 
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Edit(gym)).Returns(gym);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            
            // Act
            var putResponse = controller.Put(404, gym);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), putResponse);
        }
        [Fact]
        public void UpdateInvalidGymReturnsBadRequest()
        {
            // Arrange 
            var gym = TestEntities.Gyms[2];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(gym.Id)).Returns(gym);
            gymsRepo.Setup(repo => repo.Edit(gym)).Returns(gym);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            controller.ModelState.AddModelError("", "Error");

            // Act
            var putResponse = controller.Put(gym.Id, gym);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), putResponse);
        }

        [Fact]
        public void UpdateValidGymNullRepoResultReturns404()
        {
            // Arrange 
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            //Act
            var putResponse = controller.Put(gym.Id, gym);

            //Assert
            Assert.IsType(typeof(JsonResult), putResponse);
            Assert.Equal(404, controller.Response.StatusCode);
        }

        //Delete
        [Fact]
        public void DeleteValidGymValidIdReturnsNoContent()
        {
            // Arrange 
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(gym.Id)).Returns(gym);
            gymsRepo.Setup(repo => repo.Delete(gym)).Returns(true);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var deelteResponse = controller.Delete(gym.Id);

            // Assert
            Assert.IsType(typeof(NoContentResult), deelteResponse);
        }
        [Fact]
        public void DeleteNullGymWrongIdReturns404()
        {
            // Arrange 
            var gymsRepo = new Mock<IGymRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
           
            // Act
            var deelteResponse = controller.Delete(404);

            // Assert
            Assert.Equal(404, controller.Response.StatusCode);
            Assert.IsType(typeof(JsonResult), deelteResponse);
        }
        [Fact]
        public void DeleteValidGymValidIdErrorDuringReturnsBadRequest()
        {
            // Arrange 
            var gym = TestEntities.Gyms[0];
            var gymsRepo = new Mock<IGymRepository>();
            gymsRepo.Setup(repo => repo.Get(gym.Id)).Returns(gym);
            gymsRepo.Setup(repo => repo.Delete(gym)).Returns(false);
            var services = new Mock<IPaginationUtilities>();
            var controller = new GymApiController(gymsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            // Act
            var deelteResponse = controller.Delete(gym.Id);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), deelteResponse);
        }

    }
}
