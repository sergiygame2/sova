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
            //UnatuhorizedRequestReturnsRedirect();
            //ValidLogin_ReturnsOK();
            AddUpdateDeleteGyms();
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
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
        
        private void AddUpdateDeleteGyms()
        {
            var getAllResponse = Browser.Get("/api/gyms");
            getAllResponse.EnsureSuccessStatusCode();
            var data = (JsonConvert.DeserializeObject<List<Gym>>(getAllResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
            var startCount = data.Count;
            Console.WriteLine(startCount);
            int gymId = TestingCreate();
            try
            {
                var gym = TestingGet(gymId);
                if (gym != null)
                {
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
            gym.GymName = "Updated name";
            var editResponse = Browser.Put($"/api/gyms/{gym.Id}", new StringContent(JsonConvert.SerializeObject(gym), Encoding.UTF8, "application/json"));
            editResponse.EnsureSuccessStatusCode();

            var getByIdResponse = Browser.Get($"/api/gyms/{gym.Id}");
            getByIdResponse.EnsureSuccessStatusCode();
            var updatedGym = (JsonConvert.DeserializeObject<Gym>(getByIdResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult()));

            Assert.Equal(gym.GymName, updatedGym.GymName);
        }

        private int TestingCreate()
        {
            var createResponse = Browser.Post("/api/gyms", new StringContent(JsonConvert.SerializeObject(TestEntities.integrationGym), Encoding.UTF8, "application/json"));
            createResponse.EnsureSuccessStatusCode();
            return (JsonConvert.DeserializeObject<Gym>(createResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult())).Id;
        }
    }
}