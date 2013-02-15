using System;
using System.Collections.Generic;
using DocaLabs.Http.Client.Utils;
using NUnit.Framework;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestFixture]
    public class CustomNameValueCollectionTests
    {
        [Test]
        public void CtorWithOrdinalIgnoreCaseComparerCorrectlyInitializesDictionary()
        {
            var sample = new CustomNameValueCollection(StringComparer.OrdinalIgnoreCase)
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "33" },
                { "key1", "44" }
            };

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsTrue(sample.ContainsKey("KEY1"));
            Assert.IsTrue(sample.ContainsKey("KEy2"));
            Assert.IsTrue(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void DefaultCtorCorrectlyInitializesDictionary()
        {
            var sample = new CustomNameValueCollection
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", "33" }
            };

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsFalse(sample.ContainsKey("KEY1"));
            Assert.IsFalse(sample.ContainsKey("KEy2"));
            Assert.IsFalse(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorWithCapacityCorrectlyInitializesDictionary()
        {
            var sample = new CustomNameValueCollection(100)
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", "33" }
            };

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsFalse(sample.ContainsKey("KEY1"));
            Assert.IsFalse(sample.ContainsKey("KEy2"));
            Assert.IsFalse(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorWithCapacityAndOrdinalIgnoreCaseComparerCorrectlyInitializesDictionary()
        {
            var sample = new CustomNameValueCollection(100, StringComparer.OrdinalIgnoreCase)
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", "33" }
            };

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsTrue(sample.ContainsKey("KEY1"));
            Assert.IsTrue(sample.ContainsKey("KEy2"));
            Assert.IsTrue(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorCopiesItemsFromAnotherDictionary()
        {
            var sample = new CustomNameValueCollection(new Dictionary<string, IList<string>>
            {
                { "key1", new List<string>{ "value1"} }, 
                { "key2", new List<string>{ "value2"} }, 
                { "key3", new List<string>{ "33" } }
            });

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsFalse(sample.ContainsKey("KEY1"));
            Assert.IsFalse(sample.ContainsKey("KEy2"));
            Assert.IsFalse(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorCopiesItemsFromAnotherDictionaryList()
        {
            var sample = new CustomNameValueCollection(new CustomNameValueCollection
            {
                { "key1", new List<string>{ "value1"} }, 
                { "key2", new List<string>{ "value2"} }, 
                { "key3", new List<string>{ "33" } }
            });

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsFalse(sample.ContainsKey("KEY1"));
            Assert.IsFalse(sample.ContainsKey("KEy2"));
            Assert.IsFalse(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorOrdinalIgnoreCaseComparerCopiesItemsFromAnotherDictionary()
        {
            var sample = new CustomNameValueCollection(new Dictionary<string, IList<string>>
            {
                { "key1", new List<string> { "value1"} }, 
                { "key2", new List<string> { "value2"} }, 
                { "key3", new List<string> { "33"} }
            }, StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsTrue(sample.ContainsKey("KEY1"));
            Assert.IsTrue(sample.ContainsKey("KEy2"));
            Assert.IsTrue(sample.ContainsKey("kEY3"));
        }

        [Test]
        public void CtorOrdinalIgnoreCaseComparerCopiesItemsFromAnotherDictionaryList()
        {
            var sample = new CustomNameValueCollection(new CustomNameValueCollection
            {
                { "key1", new List<string> { "value1"} }, 
                { "key2", new List<string> { "value2"} }, 
                { "key3", new List<string> { "33"} }
            }, StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(3, sample.Count);

            Assert.IsTrue(sample.ContainsKey("key1"));
            Assert.IsTrue(sample.ContainsKey("key2"));
            Assert.IsTrue(sample.ContainsKey("key3"));

            Assert.IsTrue(sample.ContainsKey("KEY1"));
            Assert.IsTrue(sample.ContainsKey("KEy2"));
            Assert.IsTrue(sample.ContainsKey("kEY3"));
        }
    }
}
