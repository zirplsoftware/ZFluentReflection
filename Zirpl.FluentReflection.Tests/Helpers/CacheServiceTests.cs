using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zirpl.FluentReflection.Tests
{
    [TestFixture]
    public class CacheServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            new CacheService().Clear();
        }

        [Test, Sequential]
        public void TestSet(
            [Values("key1", "key2", "key2")]String key,
            [Values(1,2,3)]int value)
        {
            new CacheService().Set(key, value);
            ((int) new CacheService().Get(key)).Should().Be(value);
        }

        [Test, Sequential]
        public void TestGet(
            [Values("key1", "key2")]String key,
            [Values(true, false)]bool preSet,
            [Values(1, null)]int? expectedValue)
        {
            if (preSet)
            {
                new CacheService().Set(key, expectedValue);
            }
            var value = new CacheService().Get(key);
            if (expectedValue == null)
            {
                value.Should().BeNull();
            }
            else
            {
                value.Should().NotBeNull();
                value.Should().Be(expectedValue);
            }
        }

        [Test]
        public void TestClear()
        {
            new CacheService().Set("key1", 1);
            new CacheService().Clear();
            new CacheService().Get("key1").Should().BeNull();
        }
    }
}
