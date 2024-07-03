﻿using Microsoft.Extensions.Configuration;

namespace Web.Api.Test
{
    [TestFixture]
    public class AppSettingsTests
    {
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
        }

        [Test]
        public void ShouldContainSqlServerConnectionString()
        {
            // Arrange
            var connectionString = _configuration.GetConnectionString("SqlServerConnection");

            // Assert
            Assert.IsNotNull(connectionString);
            Assert.IsFalse(string.IsNullOrWhiteSpace(connectionString));
        }

      
        [Test]
        public void ShouldContainCapLogSqlServerConnectionString()
        {
            // Arrange
            var connectionString = _configuration.GetConnectionString("CapLogSqlServerConnection");

            // Assert
            Assert.IsNotNull(connectionString);
            Assert.IsFalse(string.IsNullOrWhiteSpace(connectionString));
        }

        [Test]
        public void ShouldContainSeriLogConnectionString()
        {
            // Arrange
            var connectionString = _configuration.GetConnectionString("SeriLogConnection");

            // Assert
            Assert.IsNotNull(connectionString);
            Assert.IsFalse(string.IsNullOrWhiteSpace(connectionString));
        }

        [Test]
        public void ShouldContainASPNETCORE_ENVIRONMENT()
        {
            // Arrange
            var environment = _configuration["ASPNETCORE_ENVIRONMENT"];

            // Assert
            Assert.IsNotNull(environment);
            Assert.IsFalse(string.IsNullOrWhiteSpace(environment));
        }

        [Test]
        public void ShouldContainLaunchBrowser()
        {
            // Arrange
            var launchBrowser = _configuration["launchBrowser"];

            // Assert
            Assert.IsNotNull(launchBrowser);
            Assert.IsFalse(string.IsNullOrWhiteSpace(launchBrowser));
        }

        [Test]
        public void ShouldContainASPNETCORE_HTTP_PORTS()
        {
            // Arrange
            var httpPorts = _configuration["ASPNETCORE_HTTP_PORTS"];

            // Assert
            Assert.IsNotNull(httpPorts);
            Assert.IsFalse(string.IsNullOrWhiteSpace(httpPorts));
        }


        [Test]
        public void ShouldContainRabbitMQSettings()
        {
            // Arrange
            var host = _configuration["RabbitMQ:Host"];
            var username = _configuration["RabbitMQ:Username"];
            var password = _configuration["RabbitMQ:Password"];
            var port = _configuration["RabbitMQ:Port"];

            // Assert
            Assert.IsNotNull(host);
            Assert.IsNotNull(username);
            Assert.IsNotNull(password);
            Assert.IsNotNull(port);

            Assert.IsFalse(string.IsNullOrWhiteSpace(host));
            Assert.IsFalse(string.IsNullOrWhiteSpace(username));
            Assert.IsFalse(string.IsNullOrWhiteSpace(password));
            Assert.IsFalse(string.IsNullOrWhiteSpace(port));
        }

        [Test]
        public void ShouldContainSerilogSettings()
        {
            // Arrange
            var minimumLevel = _configuration["Serilog:MinimumLevel:Default"];

            // Assert
            Assert.IsNotNull(minimumLevel);
            Assert.IsFalse(string.IsNullOrWhiteSpace(minimumLevel));
        }


        [Test]
        public void ShouldContainSerilogSeqUrl()
        {
            // Arrange
            var serilogSeqUrl = _configuration["SerilogSeqUrl"];

            // Assert
            Assert.IsNotNull(serilogSeqUrl);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serilogSeqUrl));
        }

        [Test]
        public void ShouldContainKestrelSettings()
        {
            // Arrange
            var kestrelSettings = _configuration.GetSection("ASPNETCORE_Kestrel");
            var certificatesSection = kestrelSettings.GetSection("Certificates");
            var defaultCertificatePassword = certificatesSection["Default:Password"];
            var defaultCertificatePath = certificatesSection["Default:Path"];

            // Assert
            Assert.IsNotNull(kestrelSettings);
            Assert.IsNotNull(certificatesSection);
            Assert.IsNotNull(defaultCertificatePassword);
            Assert.IsNotNull(defaultCertificatePath);

            Assert.IsFalse(string.IsNullOrWhiteSpace(defaultCertificatePassword));
            Assert.IsFalse(string.IsNullOrWhiteSpace(defaultCertificatePath));
        }

        [Test]
        public void ShouldContainLoggingSettings()
        {
            // Arrange
            var loggingSection = _configuration.GetSection("Logging");
            var logLevelDefault = loggingSection["LogLevel:Default"];
            var logLevelMicrosoftAspNetCore = loggingSection["LogLevel:Microsoft.AspNetCore"];

            // Assert
            Assert.IsNotNull(loggingSection);
            Assert.IsNotNull(logLevelDefault);
            Assert.IsNotNull(logLevelMicrosoftAspNetCore);

            Assert.IsFalse(string.IsNullOrWhiteSpace(logLevelDefault));
            Assert.IsFalse(string.IsNullOrWhiteSpace(logLevelMicrosoftAspNetCore));
        }
    }
}
