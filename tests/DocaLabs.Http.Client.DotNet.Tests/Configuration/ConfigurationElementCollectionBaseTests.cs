using System;
using System.Collections;
using System.Configuration;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_configuration_element_collection_base_is_newed_using_default_constructor
    {
        static CaseSensetiveCollectionTestContext _context;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new CaseSensetiveCollectionTestContext();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _context.Collection = new TestConfigurationElementCollection();
        }

        [TestMethod]
        public void it_should_create_with_add_remove_clear_collection_type()
        {
            _context.Collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.AddRemoveClearMap);
        }

        [TestMethod]
        public void it_should_return_empty_element_name()
        {
            _context.Collection.GetElementName().ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_behave_like_case_sensetive_collection()
        {
            _context.Verify();
        }
    }

    [TestClass]
    public class when_configuration_element_collection_base_is_newed_using_constructor_with_element_name
    {
        static CaseSensetiveCollectionTestContext _context;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new CaseSensetiveCollectionTestContext();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _context.Collection = new TestConfigurationElementCollection("childrenNameValues");
        }

        [TestMethod]
        public void it_should_create_with_add_remove_clear_collection_type()
        {
            _context.Collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.AddRemoveClearMap);
        }

        [TestMethod]
        public void it_should_return_specified_element_name()
        {
            _context.Collection.GetElementName().ShouldEqual("childrenNameValues");
        }

        [TestMethod]
        public void it_should_behave_like_case_sensetive_collection()
        {
            _context.Verify();
        }
    }

    [TestClass]
    public class when_configuration_element_collection_base_is_newed_using_constructor_with_collection_type_and_element_name
    {
        static CaseSensetiveCollectionTestContext _context;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new CaseSensetiveCollectionTestContext();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _context.Collection = new TestConfigurationElementCollection(ConfigurationElementCollectionType.BasicMap, "childrenNameValues2");
        }

        [TestMethod]
        public void it_should_return_specified_collection_type()
        {
            _context.Collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.BasicMap);
        }

        [TestMethod]
        public void it_should_return_specified_element_name()
        {
            _context.Collection.GetElementName().ShouldEqual("childrenNameValues2");
        }

        [TestMethod]
        public void it_should_behave_like_case_sensetive_collection()
        {
            _context.Verify();
        }
    }

    [TestClass]
    public class when_configuration_element_collection_base_is_newed_using_constructor_with_collection_type_and_element_name_and_case_insensetive_comparer
    {
        static CaseInsensetiveCollectionTestContext _context;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new CaseInsensetiveCollectionTestContext();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _context.Collection = new TestConfigurationElementCollection(ConfigurationElementCollectionType.BasicMap, "childrenNameValues3", new Comparer());
        }

        [TestMethod]
        public void it_should_return_specified_collection_type()
        {
            _context.Collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.BasicMap);
        }

        [TestMethod]
        public void it_should_return_specified_element_name()
        {
            _context.Collection.GetElementName().ShouldEqual("childrenNameValues3");
        }

        [TestMethod]
        public void it_should_behave_like_case_insensetive_collection()
        {
            _context.Verify();
        }

        class Comparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare((string)x, (string)y, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    class CaseSensetiveCollectionTestContext
    {
        public TestConfigurationElementCollection Collection { get; set; }

        public void Verify()
        {
            Collection.ShouldBeEmpty();
            Collection.GetThrowOnDuplicate().ShouldBeTrue();

            var e02 = new TestElement { Name = "N", Value = "Hello!" };

            var e1 = new TestElement { Name = "n", Value = "v" };
            var e2 = new TestElement { Name = "N", Value = "V" };
            var e3 = new TestElement { Name = "nn", Value = "vv" };
            var e4 = new TestElement { Name = "NN", Value = "VV" };

            Collection.Add(e1);
            Collection.Add(e2);
            Collection.Add(e3);
            Collection.Add(e4);

            var exception = Catch.Exception(() => Collection.Add(new TestElement { Name = "n", Value = "VVVVVVVVVVVVVVVV" }));
            exception.ShouldNotBeNull();

            Collection.Count.ShouldEqual(4);
            Collection.AllKeys.ShouldContainOnly("n", "N", "nn", "NN");
            Collection.ShouldContain(e1, e2, e3, e4);
            Collection["n"].ShouldBeTheSameAs(e1);
            Collection[0].ShouldBeTheSameAs(e1);
            Collection["N"].ShouldBeTheSameAs(e2);
            Collection[1].ShouldBeTheSameAs(e2);
            Collection["nn"].ShouldBeTheSameAs(e3);
            Collection[2].ShouldBeTheSameAs(e3);
            Collection["NN"].ShouldBeTheSameAs(e4);
            Collection[3].ShouldBeTheSameAs(e4);

            Collection.IndexOf(e1).ShouldEqual(0);
            Collection.IndexOf(e2).ShouldEqual(1);
            Collection.IndexOf(e3).ShouldEqual(2);
            Collection.IndexOf(e4).ShouldEqual(3);

            Collection["N"] = e02;
            Collection.ShouldContain(e1, e02, e3, e4);

            Collection[Collection.IndexOf(e02)] = e2;
            Collection.ShouldContain(e1, e2, e3, e4);

            Collection.Remove("N");

            Collection.Count.ShouldEqual(3);
            Collection.AllKeys.ShouldContainOnly("n", "nn", "NN");
            Collection.ShouldContain(e1, e3, e4);

            Collection.Remove(e3);

            Collection.Count.ShouldEqual(2);
            Collection.AllKeys.ShouldContainOnly("n", "NN");
            Collection.ShouldContain(e1, e4);

            Collection.RemoveAt(0);

            Collection.Count.ShouldEqual(1);
            Collection.AllKeys.ShouldContainOnly("NN");
            Collection.ShouldContain(e4);

            Collection.Clear();
            Collection.ShouldBeEmpty();
        }
    }

    class CaseInsensetiveCollectionTestContext
    {
        public TestConfigurationElementCollection Collection { get; set; }

        public void Verify()
        {
            Collection.ShouldBeEmpty();
            Collection.GetThrowOnDuplicate().ShouldBeTrue();

            var e02 = new TestElement { Name = "N2", Value = "Hello!" };

            var e1 = new TestElement { Name = "n", Value = "v" };
            var e2 = new TestElement { Name = "N2", Value = "V" };
            var e3 = new TestElement { Name = "nn2", Value = "vv" };
            var e4 = new TestElement { Name = "NN3", Value = "VV" };

            Collection.Add(e1);
            Collection.Add(e2);
            Collection.Add(e3);
            Collection.Add(e4);

            var exception = Catch.Exception(() => Collection.Add(new TestElement { Name = "N", Value = "VVVVVVVVVVVVV" }));
            exception.ShouldNotBeNull();

            Collection.Count.ShouldEqual(4);
            Collection.AllKeys.ShouldContainOnly("n", "N2", "nn2", "NN3");
            Collection.ShouldContain(e1, e2, e3, e4);
            Collection["n"].ShouldBeTheSameAs(e1);
            Collection[0].ShouldBeTheSameAs(e1);
            Collection["N2"].ShouldBeTheSameAs(e2);
            Collection[1].ShouldBeTheSameAs(e2);
            Collection["nn2"].ShouldBeTheSameAs(e3);
            Collection[2].ShouldBeTheSameAs(e3);
            Collection["NN3"].ShouldBeTheSameAs(e4);
            Collection[3].ShouldBeTheSameAs(e4);

            Collection.IndexOf(e1).ShouldEqual(0);
            Collection.IndexOf(e2).ShouldEqual(1);
            Collection.IndexOf(e3).ShouldEqual(2);
            Collection.IndexOf(e4).ShouldEqual(3);

            Collection["N2"] = e02;
            Collection.ShouldContain(e1, e02, e3, e4);

            Collection[Collection.IndexOf(e02)] = e2;
            Collection.ShouldContain(e1, e2, e3, e4);

            Collection.Remove("N2");

            Collection.Count.ShouldEqual(3);
            Collection.AllKeys.ShouldContainOnly("n", "nn2", "NN3");
            Collection.ShouldContain(e1, e3, e4);

            Collection.Remove(e3);

            Collection.Count.ShouldEqual(2);
            Collection.AllKeys.ShouldContainOnly("n", "NN3");
            Collection.ShouldContain(e1, e4);

            Collection.RemoveAt(0);

            Collection.Count.ShouldEqual(1);
            Collection.AllKeys.ShouldContainOnly("NN3");
            Collection.ShouldContain(e4);

            Collection.Clear();
            Collection.ShouldBeEmpty();
        }
    }

    class TestConfigurationElementCollection : ConfigurationElementCollectionBase<string, TestElement>
    {
        public TestConfigurationElementCollection()
        {
        }

        public TestConfigurationElementCollection(string elementName)
            : base(elementName)
        {
        }

        public TestConfigurationElementCollection(ConfigurationElementCollectionType collectionType, string elementName)
            : base(collectionType, elementName)
        {
        
        }

        public TestConfigurationElementCollection(ConfigurationElementCollectionType collectionType, string elementName, IComparer comparer)
            : base(collectionType, elementName, comparer)
        {
            
        }

        public string GetElementName()
        {
            return ElementName;
        }

        public bool GetThrowOnDuplicate()
        {
            return ThrowOnDuplicate;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestElement) element).Name;
        }

        /// <summary>
        /// Creates a new instance of the element.
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<TestElement>();
        }
    }

    class TestElement : ConfigurationElement
    {
        const string NameProperty = "name";
        const string ValueProperty = "value";

        [ConfigurationProperty(NameProperty, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)base[NameProperty]); }
            set { base[NameProperty] = value; }
        }

        [ConfigurationProperty(ValueProperty, IsRequired = true)]
        public string Value
        {
            get { return ((string)base[ValueProperty]); }
            set { base[ValueProperty] = value; }
        }
    }
}
