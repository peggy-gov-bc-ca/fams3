﻿using NUnit.Framework;
using BcGov.Fams3.Redis.Configuration;


namespace BcGov.Fams3.Redis.Test.Configuration
{
    public class RedisConfigurationTest
    {
        [Test]
        public void With_no_param_should_set_default_redis_config()
        {
            var sut = new RedisConfiguration();
            Assert.AreEqual("localhost", sut.Host);
            Assert.AreEqual(6379, sut.Port);
        }

        [Test]
        public void With_param_should_configure_redis()
        {
            var sut = new RedisConfiguration()
            {
                Host = "host",
                Port = 6666,
            };

            Assert.AreEqual("host", sut.Host);
            Assert.AreEqual(6666, sut.Port);
        }
    }
}
