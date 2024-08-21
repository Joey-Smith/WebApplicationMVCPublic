using WebApplicationMVC.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WebApplicationTest.Tests
{
    [TestClass]
    public class EmailTest
    {
        private readonly EmailSender _emailSenderSuccess;
        private readonly EmailSender _emailSenderFail;
        private readonly IOptions<EmailConfiguration> _emailConfiguration;

        public EmailTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<EmailConfiguration>()
                .AddEnvironmentVariables()
                .Build();

            var emailConfigurationSuccess = new EmailConfiguration();
            var emailConfigurationFail = new EmailConfiguration();

            config.GetSection("EmailConfiguration").Bind(emailConfigurationSuccess);
            config.GetSection("EmailConfiguration").Bind(emailConfigurationFail);

            emailConfigurationFail.Password = "password";

            _emailSenderSuccess = new EmailSender(Options.Create(emailConfigurationSuccess));
            _emailSenderFail = new EmailSender(Options.Create(emailConfigurationFail));
        }

        [TestMethod]
        public void SendEmailSuccessTest()
        {
            Assert.AreEqual(Task.FromResult(0), _emailSenderSuccess.SendEmailAsync("joey.m.smith@outlook.com", "test", "test"));
        }

        [TestMethod]
        public void SendEmailFailTest()
        {
            Assert.AreNotEqual(Task.FromResult(0), _emailSenderFail.SendEmailAsync("joey.m.smith@outlook.com", "test", "test"));
        }
    }
}