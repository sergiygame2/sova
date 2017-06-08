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
    public class CommentApiTests
    {
        [Fact]
        public void GetAllCommentReturnsJsonResult()
        {
            // Arrange
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            commentsRepo.Setup(repo => repo.GetAll()).Returns(TestEntities.Comments);
            services.Setup(service => service.Filter<Comment>(TestEntities.Comments, "", null)).Returns(TestEntities.Comments);
            services.Setup(service => service.Sort<Comment>(TestEntities.Comments, "", "", null)).Returns(TestEntities.Comments);
            services.Setup(service => service.Partition<Comment>(TestEntities.Comments, 0, 0)).Returns(TestEntities.Comments);
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller.Get();

            // Assert
            Assert.IsType(typeof(JsonResult), response);
            Assert.NotNull(((JsonResult)response).Value);
        }

        [Fact]
        public void GetCommentByValidIdReturnscomment()
        {
            // Arrange
            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(comment.Id)).Returns(comment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var response = controller.Get(comment.Id);

            //Assert
            Assert.IsType(typeof(ObjectResult), response);
            Assert.IsType(typeof(Comment), ((ObjectResult)response).Value);
            var actualcomment = (Comment)((ObjectResult)response).Value;
            Assert.Equal(comment, actualcomment);
        }

        [Fact]
        public void GetCommentByWrongIdReturns404()
        {
            // Arrange
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            // Act
            var response = controller.Get(20000);

            // Assert
            Assert.IsType(typeof(JsonResult), response);
            Assert.Equal(404, controller.Response.StatusCode);
        }
        [Fact]
        public void AddNullCommentReturnsBadRequest()
        {
            // Arrange 
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Post(null);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), postResponse);
        }
        [Fact]
        public void AddInvalidCommentReturnsBadRequest()
        {
            // Arrange 

            var comment = TestEntities.Comments[2];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(comment.Id)).Returns(comment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Post(comment);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), postResponse);
        }
        [Fact]
        public void AddValidCommentReturnsObjectResult()
        {
            // Arrange 

            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(comment.Id)).Returns(comment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Post(comment);

            // Assert
            Assert.IsType(typeof(CreatedAtRouteResult), postResponse);
            Assert.IsType(typeof(Comment), ((ObjectResult)postResponse).Value);
            var actualcomment = (Comment)((ObjectResult)postResponse).Value;
            Assert.Equal(comment.Id, actualcomment.Id);

        }
        [Fact]
        public void UpdateValidCommentValidIdReturnsNoContent()
        {
            // Arrange 
            
            var updatedcomment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(updatedcomment.Id)).Returns(updatedcomment);
            commentsRepo.Setup(repo => repo.Edit(updatedcomment)).Returns(updatedcomment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var putResponse = controller.Put(updatedcomment.Id, updatedcomment);

            // Assert
            Assert.IsType(typeof(NoContentResult), putResponse);
        }
        [Fact]
        public void UpdateNullCommentReturnsBadRequest()
        {
            // Arrange 
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object);

            // Act
            var putResponse = controller.Put(1, null);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), putResponse);
        }
        [Fact]
        public void UpdateValidCommentWrongIdBadRequest()
        {
            // Arrange 
            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Edit(comment)).Returns(comment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            // Act
            var putResponse = controller.Put(404, comment);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), putResponse);
        }
        [Fact]
        public void UpdateInvalidCommentReturnsBadRequest()
        {
            // Arrange 

            var comment = TestEntities.Comments[2];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Edit(comment)).Returns(comment);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var postResponse = controller.Put(comment.Id, comment); 

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), postResponse);
        }

        [Fact]
        public void UpdateValidCommentNullRepoResultReturns404()
        {
            // Arrange 
            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            //Act
            var putResponse = controller.Put(comment.Id, comment);

            //Assert
            Assert.IsType(typeof(JsonResult), putResponse);
            Assert.Equal(404, controller.Response.StatusCode);
        }

        //Delete
        [Fact]
        public void DeleteValidCommentValidIdReturnsNoContent()
        {
            // Arrange 
            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(comment.Id)).Returns(comment);
            commentsRepo.Setup(repo => repo.Delete(comment)).Returns(true);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };

            // Act
            var deelteResponse = controller.Delete(comment.Id);

            // Assert
            Assert.IsType(typeof(NoContentResult), deelteResponse);
        }
        [Fact]
        public void DeleteNullCommentWrongIdReturns404()
        {
            // Arrange 
            var commentsRepo = new Mock<ICommentRepository>();
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            // Act
            var deelteResponse = controller.Delete(404);

            // Assert
            Assert.Equal(404, controller.Response.StatusCode);
            Assert.IsType(typeof(JsonResult), deelteResponse);
        }
        [Fact]
        public void DeleteValidCommentValidIdErrorDuringReturnsBadRequest()
        {
            // Arrange 
            var comment = TestEntities.Comments[0];
            var commentsRepo = new Mock<ICommentRepository>();
            commentsRepo.Setup(repo => repo.Get(comment.Id)).Returns(comment);
            commentsRepo.Setup(repo => repo.Delete(comment)).Returns(false);
            var services = new Mock<IPaginationUtilities>();
            var controller = new CommentApiController(commentsRepo.Object, services.Object) { ControllerContext = { HttpContext = new DefaultHttpContext() } };
            
            // Act
            var deelteResponse = controller.Delete(comment.Id);

            // Assert
            Assert.IsType(typeof(BadRequestObjectResult), deelteResponse);
        }

    }
}
