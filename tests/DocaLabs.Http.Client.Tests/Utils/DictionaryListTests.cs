using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DocaLabs.Http.Client.Utils;
using NUnit.Framework;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestFixture]
    public class DictionaryListTests
    {
        [Test]
        public void AddFromICollectionCountAndContainsAndThisGetWorkingFine()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", new List<object> { "value1" } },
                { "key2", new List<object> { "value2" } },
                { "key3", new List<object> { 33 } }
            };

            Assert.AreEqual(3, sample.Count);

            var value = sample["key1"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value1", value[0]);

            value = sample["key2"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value2", value[0]);

            value = sample["key3"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<int>(value[0]);
            Assert.AreEqual(33, value[0]);


            sample.Add("key1", 44);

            Assert.AreEqual(3, sample.Count);

            value = sample["key1"];
            Assert.IsInstanceOf<List<object>>(value);
            Assert.AreEqual(2, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value1", value[0]);
            Assert.IsInstanceOf<int>(value[1]);
            Assert.AreEqual(44, value[1]);

            value = sample["key2"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value2", value[0]);

            value = sample["key3"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<int>(value[0]);
            Assert.AreEqual(33, value[0]);



            sample.Remove("key2");

            Assert.AreEqual(2, sample.Count);

            value = sample["key1"];
            Assert.AreEqual(2, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value1", value[0]);
            Assert.IsInstanceOf<int>(value[1]);
            Assert.AreEqual(44, value[1]);

            value = sample["key3"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<int>(value[0]);
            Assert.AreEqual(33, value[0]);


            Assert.IsFalse(sample.Remove("key2"));

            sample.Remove("key1");

            Assert.AreEqual(1, sample.Count);

            value = sample["key3"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<int>(value[0]);
            Assert.AreEqual(33, value[0]);

            sample.Remove("key3");

            Assert.AreEqual(0, sample.Count);
        }

        [Test]
        public void AddingListValueMergesWithExistingValues()
        {
            var sample = new DictionaryList<string, object>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"key3", 33},
                {"key2", new List<object> { 22 }},
                {"key1", "value12"}
            };

            Assert.AreEqual("value1", sample["key1"][0]);
            Assert.AreEqual("value12", sample["key1"][1]);

            Assert.AreEqual("value2", sample["key2"][0]);
            Assert.AreEqual(22, sample["key2"][1]);

            Assert.AreEqual(33, sample["key3"][0]);
        }

        [Test]
        public void AddsRangeOfValues()
        {
            var target = new DictionaryList<string, object>
            {
                { "key1", 11 }
            };

            var source  = new DictionaryList<string, object>
            {
                { "key1", 111 },
                { "key2", 22 },
                { "key2", 222 },
                { "key3", "3" }
            };

            target.AddRange(source);

            Assert.AreEqual(3, target.Count);
            Assert.AreEqual(2, target["key1"].Count);
            Assert.AreEqual(11, target["key1"][0]);
            Assert.AreEqual(111, target["key1"][1]);
            Assert.AreEqual(2, target["key2"].Count);
            Assert.AreEqual(22, target["key2"][0]);
            Assert.AreEqual(222, target["key2"][1]);
            Assert.AreEqual(1, target["key3"].Count);
            Assert.AreEqual("3", target["key3"][0]);
        }

        [Test]
        public void AddingNullRangeDoesNothing()
        {
            var target = new DictionaryList<string, object>
            {
                { "key1", 11 }
            };

            target.AddRange(null);

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(1, target["key1"].Count);
            Assert.AreEqual(11, target["key1"][0]);
        }

        [Test]
        public void CanAddSingleNullValues()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", (object)null }
            };

            Assert.IsNull(sample["key1"][0]);
        }

        [Test]
        public void CanAddNullValueList()
        {
            // ReSharper disable RedundantCast
            var sample = new DictionaryList<string, object>
            {
                { "key1", (IList<object>)null }
            };

            Assert.AreEqual(0, sample["key1"].Count);

            sample.Add("key1", (IList<object>)null);

            Assert.AreEqual(0, sample["key1"].Count);
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ClearWorksFine()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 }
            };

            Assert.AreEqual(3, sample.Count);

            sample.Clear();

            Assert.AreEqual(0, sample.Count);
        }

        [Test]
        public void TryGetValueWorksFine()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 }
            };

            IList<object> value;
            Assert.IsTrue(sample.TryGetValue("key1", out value));
            Assert.AreEqual("value1", value[0]);

            Assert.IsTrue(sample.TryGetValue("key2", out value));
            Assert.AreEqual("value2", value[0]);

            Assert.IsTrue(sample.TryGetValue("key3", out value));
            Assert.AreEqual(33, value[0]);

            Assert.IsFalse(sample.TryGetValue("key4", out value));
            Assert.IsNull(value);
        }

        [Test]
        public void IndexerWorksFine()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 }
            };

            Assert.AreEqual("value1", sample["key1"][0]);
            Assert.AreEqual("value2", sample["key2"][0]);
            Assert.AreEqual(33, sample["key3"][0]);

            sample["key1"] = new List<object> { 23 };

            var value = sample["key1"];
            Assert.AreEqual(1, value.Count);
            Assert.IsInstanceOf<int>(value[0]);
            Assert.AreEqual(23, value[0]);

            Assert.AreEqual("value2", sample["key2"][0]);
            Assert.AreEqual(33, sample["key3"][0]);
        }

        [Test]
        public void ListsAllKeys()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 },
                { "key1", 44 }
            };

            var keys = sample.Keys;

            Assert.AreEqual(3, keys.Count);
            Assert.AreEqual("key1", keys.ElementAt(0));
            Assert.AreEqual("key2", keys.ElementAt(1));
            Assert.AreEqual("key3", keys.ElementAt(2));
        }

        [Test]
        public void EnumeratesThroughAllElements()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 },
                { "key1", 44 }
            };

            var iterator = sample.GetEnumerator();
            VerifyEnumerator(iterator);

            iterator.Reset();
            VerifyEnumerator(iterator);
        }

        [Test]
        public void EnumeratesThroughAllElementsWithExplicitEnumerator()
        {
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 },
                { "key1", 44 }
            };

            var iterator = ((IEnumerable)sample).GetEnumerator();

            VerifyEnumerator((IEnumerator<KeyValuePair<string, IList<object>>>)iterator);
        }

        [Test]
        public void CtorWithOrdinalIgnoreCaseComparerCorrectlyInitializesDictionary()
        {
            var sample = new DictionaryList<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", 33 },
                { "key1", 44 }
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
            var sample = new DictionaryList<string, object>
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", 33 }
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
            var sample = new DictionaryList<string, object>(100)
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", 33 }
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
            var sample = new DictionaryList<string, object>(100, StringComparer.OrdinalIgnoreCase)
            {
                { "key1", "value1" }, 
                { "key2", "value2" }, 
                { "key3", 33 }
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
            var sample = new DictionaryList<string, object>(new Dictionary<string, IList<object>>
            {
                { "key1", new List<object>{ "value1"} }, 
                { "key2", new List<object>{ "value2"} }, 
                { "key3", new List<object>{ 33 } }
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
            var sample = new DictionaryList<string, object>(new DictionaryList<string, object>
            {
                { "key1", new List<object>{ "value1"} }, 
                { "key2", new List<object>{ "value2"} }, 
                { "key3", new List<object>{ 33 } }
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
        public void CtorWithAnotherDictionaryThrowsArgumentNullExceptionWhenAnotherDictionaryListIsNull()
        {
            Assert.Catch<ArgumentNullException>(
                () => new DictionaryList<string, object>((DictionaryList<string, object>) null));
        }

        [Test]
        public void CtorOrdinalIgnoreCaseComparerCopiesItemsFromAnotherDictionary()
        {
            var sample = new DictionaryList<string, object>(new Dictionary<string, IList<object>>
            {
                { "key1", new List<object> { "value1"} }, 
                { "key2", new List<object> { "value2"} }, 
                { "key3", new List<object> { 33} }
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
            var sample = new DictionaryList<string, object>(new DictionaryList<string, object>
            {
                { "key1", new List<object> { "value1"} }, 
                { "key2", new List<object> { "value2"} }, 
                { "key3", new List<object> { 33} }
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
        public void CtorWithAnotherDictionaryAndComparerThrowsArgumentNullExceptionWhenAnotherDictionaryListIsNull()
        {
            Assert.Catch<ArgumentNullException>(
                () => new DictionaryList<string, object>((DictionaryList<string, object>)null, StringComparer.OrdinalIgnoreCase));
        }

        static void VerifyEnumerator(IEnumerator<KeyValuePair<string, IList<object>>> iterator)
        {
            Assert.IsTrue(iterator.MoveNext());
            var value = iterator.Current.Value;
            Assert.AreEqual(2, value.Count);
            Assert.IsInstanceOf<string>(value[0]);
            Assert.AreEqual("value1", value[0]);
            Assert.IsInstanceOf<int>(value[1]);
            Assert.AreEqual(44, value[1]);

            Assert.IsTrue(iterator.MoveNext());
            Assert.AreEqual("key2", iterator.Current.Key);
            Assert.AreEqual(1, iterator.Current.Value.Count);
            Assert.AreEqual("value2", iterator.Current.Value[0]);

            Assert.IsTrue(iterator.MoveNext());
            Assert.AreEqual("key3", iterator.Current.Key);
            Assert.AreEqual(1, iterator.Current.Value.Count);
            Assert.AreEqual(33, iterator.Current.Value[0]);

            Assert.IsFalse(iterator.MoveNext());
            Assert.IsFalse(iterator.MoveNext());
        }
    }
}
