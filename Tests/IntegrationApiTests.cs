using Xunit;
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using SportApp.Models.DTO;

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
            ValidLogin_ReturnsOK();
            
            UnatuhorizedRequestReturnsRedirect();
            AddUpdateDeleteGyms();
        }
        
        private void UnatuhorizedRequestReturnsRedirect()
        {
            var response = Browser.Get("/Admin/Gym");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            HttpContent content = new StringContent("{}", Encoding.UTF8, "application/json");

            response = Browser.Post("/Admin/Gym/Create", content);
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
            
        }
    }
}