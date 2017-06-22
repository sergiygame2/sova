using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using System;
using System.Net;
using System.Net.Http;

namespace Tests
{
    public class TestBrowser
    {
        private readonly TestServer _testServer;

        public TestBrowser(TestServer testServer)
        {
            _testServer = testServer;
            Cookies = new CookieContainer();
        }

        public CookieContainer Cookies { get; }

        public HttpResponseMessage Get(string relativeUrl)
        {
            return Get(new Uri(relativeUrl, UriKind.Relative));
        }

        public HttpResponseMessage Get(Uri relativeUrl)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            var response = requestBuilder.GetAsync().Result;
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        public void AddCookie(Cookie cookie)
        {
            Cookies.Add(_testServer.BaseAddress, cookie);
        }
        
        private void AddCookies(RequestBuilder requestBuilder, Uri absoluteUrl)
        {
            var cookieHeader = Cookies.GetCookieHeader(absoluteUrl);
            if (!string.IsNullOrWhiteSpace(cookieHeader))
            {
                requestBuilder.AddHeader(HeaderNames.Cookie, cookieHeader);
            }
        }

        private void UpdateCookies(HttpResponseMessage response, Uri absoluteUrl)
        {
            if (response.Headers.Contains(HeaderNames.SetCookie))
            {
                var cookies = response.Headers.GetValues(HeaderNames.SetCookie);
                foreach (var cookie in cookies)
                {
                    Cookies.SetCookies(absoluteUrl, cookie);
                }
            }
        }

        public HttpResponseMessage Post(string relativeUrl, HttpContent content)
        {
            return Post(new Uri(relativeUrl, UriKind.Relative), content);
        }

        public HttpResponseMessage Post(Uri relativeUrl, HttpContent content)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            var response = requestBuilder.And(message =>
            {
                message.Content = content;
            }).PostAsync().Result;
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        public HttpResponseMessage Put(string relativeUrl, HttpContent content)
        {
            return Put(new Uri(relativeUrl, UriKind.Relative), content);
        }

        public HttpResponseMessage Put(Uri relativeUrl, HttpContent content)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            var response = requestBuilder.And(message =>
            {
                message.Content = content;
            }).SendAsync("PUT").Result;
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        public HttpResponseMessage Delete(string relativeUrl)
        {
            return Delete(new Uri(relativeUrl, UriKind.Relative));
        }

        public HttpResponseMessage Delete(Uri relativeUrl)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            var response = requestBuilder.SendAsync("DELETE").Result;
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        public HttpResponseMessage FollowRedirect(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Moved && response.StatusCode != HttpStatusCode.Found)
            {
                return response;
            }
            var redirectUrl = new Uri(response.Headers.Location.ToString(), UriKind.RelativeOrAbsolute);
            if (redirectUrl.IsAbsoluteUri)
            {
                redirectUrl = new Uri(redirectUrl.PathAndQuery, UriKind.Relative);
            }
            return Get(redirectUrl);
        }
    }
}
