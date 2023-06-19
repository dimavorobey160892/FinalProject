using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;

namespace FinalProject.Tests
{
    public class SampleTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public SampleTests(WebApplicationFactory<Program> factory) => _factory = factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/AvailableCars")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Contacts")]
        [InlineData("/Home/Order")]
        [InlineData("/User/Login")]
        [InlineData("/User/Registration")]
        public async Task GetSomePages(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            // if it's not 200-299, it will throw an exception
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/Home/MyQuestions")]
        [InlineData("/Home/MyOrders")]
        [InlineData("/User/Settings")]
        public async Task GetRedirectIfNotAuth(string url)
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            // Act
            var response = await client.GetAsync(url);
            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/User/Login?ReturnUrl=",
                response.Headers.Location?.OriginalString);
        }

        [Theory]
        [InlineData("/Admin/Index")]
        public async Task Get_SecurePageIsReturnedForAnAuthenticatedUser(string url)
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                    services.AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "TestScheme", _ => { }));
            })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(scheme: "TestScheme");
            //Act
            var response = await client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode();
            // if it's not 200-299, it will throw an exception
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "Test admin"),
            new(ClaimsIdentity.DefaultRoleClaimType, "Admin")
        };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }
    }
}
