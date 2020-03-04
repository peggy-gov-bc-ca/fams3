﻿using BcGov.Fams3.Redis.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BcGov.Fams3.Redis.Configuration;

namespace BcGov.Fams3.Redis.Test.DependencyInjection
{
    public class ServiceCollectionExtensionsTest
    {
        private IServiceCollection _services;
        private IConfiguration _configuration;
        private IConfigurationBuilder _configurationBuilder;
        private string _settings = string.Empty;
        private IConfiguration _invalidConfiguration;
        private IConfigurationBuilder _invalidConfigurationBuilder;
        private string _invalidSettings = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _settings = "{\"Logging\":{\"LogLevel\":{\"Default\": \"Information\",\"Microsoft\": \"Warning\",\"Microsoft.Hosting.Lifetime\": \"Information\"}},\"AllowedHosts\": \"*\",\"RabbitMq\": {\"Host\": \"localhost\",\"Port\": 5672,\"Username\": \"guest\",\"Password\": \"guest\"},\"Redis\": {\"Host\": \"localhost\",\"Port\": 6379},\"ProviderProfile\": {\"Name\": \"Sample\"}}";
            _services = new ServiceCollection();
            _configurationBuilder = new ConfigurationBuilder();
            _configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(_settings)));
            _configuration = _configurationBuilder.Build();

            _invalidSettings = "{\"Logging\":{\"LogLevel\":{\"Default\": \"Information\",\"Microsoft\": \"Warning\",\"Microsoft.Hosting.Lifetime\": \"Information\"}},\"AllowedHosts\": \"*\",\"RabbitMq\": {\"Host\": \"localhost\",\"Port\": 5672,\"Username\": \"guest\",\"Password\": \"guest\"},\"\": {\"Host\": \"localhost\",\"Port\": 6379},\"ProviderProfile\": {\"Name\": \"Sample\"}}";
            _invalidConfigurationBuilder = new ConfigurationBuilder();
            _invalidConfigurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(_invalidSettings)));
            _invalidConfiguration = _invalidConfigurationBuilder.Build();
        }

        [Test]
        public void should_register_services()
        {
            _services.AddCacheService(_configuration);

            Assert.IsTrue(_services.Count == 2);
        }

        [Test]
        public void invalid_config_should_throw_exception()
        {
            Assert.Throws<Exception>(() => _services.AddCacheService(_invalidConfiguration));
        }
    }
}