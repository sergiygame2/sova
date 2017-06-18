using Xunit;
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using SportApp.Models.DTO;
using SportApp.Models;

namespace Tests
{
    [Collection("IntegrationTests")]
    public class IntegrationApiTests
    {
        public TestBrowser Browser { get; }

        public IntegrationApiTests(IntegrationFixture fixture)
        {
            Console.WriteLine("IntegrationApiTests constructor");
            Browser = fixture.Browser;
        }
        
        [Fact]
        public void TestGyms()
        {
            UnatuhorizedRequestReturnsRedirect();
            ValidLogin_ReturnsOK();
            AddUpdateDeleteGymsWithComments();
        }
        
        private void UnatuhorizedRequestReturnsRedirect()
        {
            var response = Browser.Get("/Admin/Gym");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
        
        private void ValidLogin_ReturnsOK()
        {
            //content.Headers.Add("RequestVerificationToken","CfDJ8Acf8gpcQ4BOncVEbTSfIvViHXCRTD15HCAo8xS77iq6VHjUYEkSMkmoUmlXB5IPEtPVtV6vxJLN9GwCARbB5HN7xEzx7PR7zwKYRJKD0C_MreTtIpBz0vZdPhBIlwE7j-7UCrZzdyDhIpWLkp20Po-eQLrASSJYlQlCEOHdvrnyOD8yoHeC3T_oLFuLsgKwww");
            //Browser.AddCookie(new Cookie(".AspNetCore.Antiforgery.zIIeLIG2uRE", "CfDJ8Acf8gpcQ4BOncVEbTSfIvWnRRKYBUFetq3szc4DZCecVtUAWMCl3rGDzDn8tnxDpYmdBj_yANAW780P6dKtvlu8TuB_roLFQPcMszdZqOq-77GnAFkkZxlX-INOS8amEMPgh3btVDmrNgLJLOqq6f8"));
            
            // Arrange
            HttpContent content = new StringContent("{}", Encoding.UTF8, "application/json");
            var path = String.Format("account/login?username=admin@example.com&password=Password_1");
            // Act
            var response = Browser.Post(path, content);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        private void AddUpdateDeleteGymsWithComments()
        {
            var getAllResponse = Browser.Get("/api/gyms");
            getAllResponse.EnsureSuccessStatusCode();
            var data = (JsonConvert.DeserializeObject<List<Gym>>(getAllResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
            var startCount = data.Count;

            var getAllCommentsResponse = Browser.Get("/api/comments");
            getAllCommentsResponse.EnsureSuccessStatusCode();
            var comments = (JsonConvert.DeserializeObject<List<Gym>>(getAllCommentsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
            var startCommentsCount = comments.Count;
            
            Console.WriteLine("Gyms startCount = " + startCount);
            Console.WriteLine("Comments startCount = " + startCommentsCount);

            int gymId = TestingCreate();
            int commentId = 0;
            try
            {
                var gym = TestingGet(gymId);
                if (gym != null)
                {
                    var createdCommentId = TestingComments(gym.Id);
                    if (createdCommentId != null)
                        commentId = (int) createdCommentId;

                    TestingEdit(gym);

                    TestingDelete(gym.Id, startCount);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (commentId != 0)
                {
                    var deleteCommentResponse = Browser.Delete($"/api/comments/{commentId}");
                    deleteCommentResponse.EnsureSuccessStatusCode();
                }

                var deleteResponse = Browser.Delete($"/api/gyms/{gymId}");
                deleteResponse.EnsureSuccessStatusCode();
                throw;
            }
        }

        private Gym TestingGet(int gymId)
        {
            var getByIdResponse = Browser.Get($"/api/gyms/{gymId}");
            getByIdResponse.EnsureSuccessStatusCode();
            var gym = (JsonConvert.DeserializeObject<Gym>(getByIdResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
            
            var getByWrongIdResponse = Browser.Get("/api/gyms/0");
            Assert.Equal(HttpStatusCode.NotFound, getByWrongIdResponse.StatusCode);
            
            return gym;
        }
        
        private void TestingDelete(int id, int startCount)
        {
            var failDeleteResponse = Browser.Delete($"/api/gyms/0");
            Assert.Equal(HttpStatusCode.NotFound, failDeleteResponse.StatusCode);

            var deleteResponse = Browser.Delete($"/api/gyms/{id}");
            deleteResponse.EnsureSuccessStatusCode();

            var getAllResponse = Browser.Get("/api/gyms");
            getAllResponse.EnsureSuccessStatusCode();
            var data = (JsonConvert.DeserializeObject<List<Gym>>(getAllResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
            var lastCount = data.Count;

            Assert.Equal(startCount, lastCount);
        }

        private void TestingEdit(Gym gym)
        {
            var nullGymEditResponse = Browser.Put($"/api/gyms/{gym.Id}",
                new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, nullGymEditResponse.StatusCode);

            var invalidGym = TestEntities.Gyms[2];
            var invalidGymEditResponse = Browser.Put($"/api/gyms/{invalidGym.Id}",
                new StringContent(JsonConvert.SerializeObject(invalidGym), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, invalidGymEditResponse.StatusCode);

            var notExistenGym = TestEntities.NotExistenGym;
            var notExistenGymEditResponse = Browser.Put($"/api/gyms/{notExistenGym.Id}",
                new StringContent(JsonConvert.SerializeObject(notExistenGym), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, notExistenGymEditResponse.StatusCode);
            
            gym.GymName = "Updated name";
            var editResponse = Browser.Put($"/api/gyms/{gym.Id}",
                new StringContent(JsonConvert.SerializeObject(gym), Encoding.UTF8, "application/json"));
            editResponse.EnsureSuccessStatusCode();

            var getByIdResponse = Browser.Get($"/api/gyms/{gym.Id}");
            getByIdResponse.EnsureSuccessStatusCode();
            var updatedGym = (JsonConvert.DeserializeObject<Gym>(getByIdResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));

            Assert.Equal(gym.GymName, updatedGym.GymName);
        }

        private int TestingCreate()
        {
            var nullGymCreateResponse = Browser.Post("/api/gyms",
                new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, nullGymCreateResponse.StatusCode);

            var invalidGymCreateResponse = Browser.Post("/api/gyms",
                new StringContent(JsonConvert.SerializeObject(TestEntities.Gyms[2]), Encoding.UTF8,
                    "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, invalidGymCreateResponse.StatusCode);

            var createResponse = Browser.Post("/api/gyms",
                new StringContent(JsonConvert.SerializeObject(TestEntities.ValidIntegrationGym), Encoding.UTF8,
                    "application/json"));
            createResponse.EnsureSuccessStatusCode();

            return (JsonConvert.DeserializeObject<Gym>(createResponse.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult())).Id;
        }

        private int? TestingComments(int gymId)
        {
            var comment = new Comment
            {
                CommentText = "Some comment",
                Rate = 8,
                GymId = gymId,
                UserId = "0e019b31-6194-4199-8fdd-b5090c0dad75"
            };

            var createResponse = Browser.Post("/api/comments",
                new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8,
                    "application/json"));
            createResponse.EnsureSuccessStatusCode();

            var createdCommentId =
                (JsonConvert.DeserializeObject<Comment>(createResponse.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult()))?.Id;
            
            var getResponse = Browser.Get($"/api/comments");
            getResponse.EnsureSuccessStatusCode();
            var comments = (JsonConvert.DeserializeObject<List<Comment>>(getResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));

            Assert.True(comments.Any());

            try
            {
                if (createdCommentId != null)
                {
                    comment.Id = (int)createdCommentId;
                    comment.CommentText = "Updated comment text";
                    var editResponse = Browser.Put($"/api/comments/{createdCommentId}",
                        new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json"));
                    editResponse.EnsureSuccessStatusCode();

                    var getByIdResponse = Browser.Get($"/api/comments/{createdCommentId}");
                    getByIdResponse.EnsureSuccessStatusCode();
                    var updatedComment = (JsonConvert.DeserializeObject<Comment>(getByIdResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));

                    Assert.Equal(comment.CommentText, updatedComment.CommentText);
                    Assert.Equal(createdCommentId, updatedComment.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                var deleteResponse = Browser.Delete($"/api/comments/{createdCommentId}");
                deleteResponse.EnsureSuccessStatusCode();

                throw;
            }
            return createdCommentId;
        }
    }
}