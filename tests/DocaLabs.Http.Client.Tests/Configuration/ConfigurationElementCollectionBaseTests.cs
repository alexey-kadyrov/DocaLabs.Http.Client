using System;
using System.Collections;
using System.Configuration;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ConfigurationElementCollectionBase<,>))]
    class when_configuration_element_collection_base_is_newed_using_default_constructor : case_sensetive_collection_test_context
    {
        Because of =
            () => collection = new TestConfigurationElementCollection();

        It should_create_with_add_remove_clear_collection_type =
            () => collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.AddRemoveClearMap);

        It should_return_empty_element_name =
            () => collection.GetElementName().ShouldBeEmpty();

        It should_behave_like_case_sensetive_collection =
            () => Verify();
    }

    [Subject(typeof(ConfigurationElementCollectionBase<,>))]
    class when_configuration_element_collection_base_is_newed_using_constructor_with_element_name : case_sensetive_collection_test_context
    {
        Because of =
            () => collection = new TestConfigurationElementCollection("childrenNameValues");

        It should_create_with_add_remove_clear_collection_type =
            () => collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.AddRemoveClearMap);

        It should_return_specified_element_name =
            () => collection.GetElementName().ShouldEqual("childrenNameValues");

        It should_behave_like_case_sensetive_collection =
            () => Verify();
    }

    [Subject(typeof(ConfigurationElementCollectionBase<,>))]
    class when_configuration_element_collection_base_is_newed_using_constructor_with_collection_type_and_element_name : case_sensetive_collection_test_context
    {
        Because of =
            () => collection = new TestConfigurationElementCollection(ConfigurationElementCollectionType.BasicMap, "childrenNameValues2");

        It should_return_specified_collection_type =
            () => collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.BasicMap);

        It should_return_specified_element_name =
            () => collection.GetElementName().ShouldEqual("childrenNameValues2");

        It should_behave_like_case_sensetive_collection =
            () => Verify();
    }

    [Subject(typeof(ConfigurationElementCollectionBase<,>))]
    class when_configuration_element_collection_base_is_newed_using_constructor_with_collection_type_and_element_name_and_case_insensetive_comparer : case_insensetive_collection_test_context
    {
        Because of =
            () => collection = new TestConfigurationElementCollection(ConfigurationElementCollectionType.BasicMap, "childrenNameValues3", new Comparer());
        It should_return_specified_collection_type =
            () => collection.CollectionType.ShouldEqual(ConfigurationElementCollectionType.BasicMap);

        It should_return_specified_element_name =
            () => collection.GetElementName().ShouldEqual("childrenNameValues3");

        It should_behave_like_case_insensetive_collection =
            () => Verify();

        class Comparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare((string)x, (string)y, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    class case_sensetive_collection_test_context
    {
        protected static TestConfigurationElementCollection collection;

        protected static void Verify()
        {
            collection.ShouldBeEmpty();
            collection.GetThrowOnDuplicate().ShouldBeTrue();

            var e02 = new TestElement { Name = "N", Value = "Hello!" };

            var e1 = new TestElement { Name = "n", Value = "v" };
            var e2 = new TestElement { Name = "N", Value = "V" };
            var e3 = new TestElement { Name = "nn", Value = "vv" };
            var e4 = new TestElement { Name = "NN", Value = "VV" };

            collection.Add(e1);
            collection.Add(e2);
            collection.Add(e3);
            collection.Add(e4);

            var exception = Catch.Exception(() => collection.Add(new TestElement { Name = "n", Value = "VVVVVVVVVVVVVVVV" }));
            exception.ShouldNotBeNull();

            collection.Count.ShouldEqual(4);
            collection.AllKeys.ShouldContainOnly("n", "N", "nn", "NN");
            collection.ShouldContain(e1, e2, e3, e4);
            collection["n"].ShouldBeTheSameAs(e1);
            collection[0].ShouldBeTheSameAs(e1);
            collection["N"].ShouldBeTheSameAs(e2);
            collection[1].ShouldBeTheSameAs(e2);
            collection["nn"].ShouldBeTheSameAs(e3);
            collection[2].ShouldBeTheSameAs(e3);
            collection["NN"].ShouldBeTheSameAs(e4);
            collection[3].ShouldBeTheSameAs(e4);

            collection.IndexOf(e1).ShouldEqual(0);
            collection.IndexOf(e2).ShouldEqual(1);
            collection.IndexOf(e3).ShouldEqual(2);
            collection.IndexOf(e4).ShouldEqual(3);

            collection["N"] = e02;
            collection.ShouldContain(e1, e02, e3, e4);

            collection[collection.IndexOf(e02)] = e2;
            collection.ShouldContain(e1, e2, e3, e4);

            collection.Remove("N");

            collection.Count.ShouldEqual(3);
            collection.AllKeys.ShouldContainOnly("n", "nn", "NN");
            collection.ShouldContain(e1, e3, e4);

            collection.Remove(e3);

            collection.Count.ShouldEqual(2);
            collection.AllKeys.ShouldContainOnly("n", "NN");
            collection.ShouldContain(e1, e4);

            collection.RemoveAt(0);

            collection.Count.ShouldEqual(1);
            collection.AllKeys.ShouldContainOnly("NN");
            collection.ShouldContain(e4);

            collection.Clear();
            collection.ShouldBeEmpty();
        }
    }

    class case_insensetive_collection_test_context
    {
        protected static TestConfigurationElementCollection collection;

        protected static void Verify()
        {
            collection.ShouldBeEmpty();
            collection.GetThrowOnDuplicate().ShouldBeTrue();

            var e02 = new TestElement { Name = "N2", Value = "Hello!" };

            var e1 = new TestElement { Name = "n", Value = "v" };
            var e2 = new TestElement { Name = "N2", Value = "V" };
            var e3 = new TestElement { Name = "nn2", Value = "vv" };
            var e4 = new TestElement { Name = "NN3", Value = "VV" };

            collection.Add(e1);
            collection.Add(e2);
            collection.Add(e3);
            collection.Add(e4);

            var exception = Catch.Exception(() => collection.Add(new TestElement { Name = "N", Value = "VVVVVVVVVVVVV" }));
            exception.ShouldNotBeNull();

            collection.Count.ShouldEqual(4);
            collection.AllKeys.ShouldContainOnly("n", "N2", "nn2", "NN3");
            collection.ShouldContain(e1, e2, e3, e4);
            collection["n"].ShouldBeTheSameAs(e1);
            collection[0].ShouldBeTheSameAs(e1);
            collection["N2"].ShouldBeTheSameAs(e2);
            collection[1].ShouldBeTheSameAs(e2);
            collection["nn2"].ShouldBeTheSameAs(e3);
            collection[2].ShouldBeTheSameAs(e3);
            collection["NN3"].ShouldBeTheSameAs(e4);
            collection[3].ShouldBeTheSameAs(e4);

            collection.IndexOf(e1).ShouldEqual(0);
            collection.IndexOf(e2).ShouldEqual(1);
            collection.IndexOf(e3).ShouldEqual(2);
            collection.IndexOf(e4).ShouldEqual(3);

            collection["N2"] = e02;
            collection.ShouldContain(e1, e02, e3, e4);

            collection[collection.IndexOf(e02)] = e2;
            collection.ShouldContain(e1, e2, e3, e4);

            collection.Remove("N2");

            collection.Count.ShouldEqual(3);
            collection.AllKeys.ShouldContainOnly("n", "nn2", "NN3");
            collection.ShouldContain(e1, e3, e4);

            collection.Remove(e3);

            collection.Count.ShouldEqual(2);
            collection.AllKeys.ShouldContainOnly("n", "NN3");
            collection.ShouldContain(e1, e4);

            collection.RemoveAt(0);

            collection.Count.ShouldEqual(1);
            collection.AllKeys.ShouldContainOnly("NN3");
            collection.ShouldContain(e4);

            collection.Clear();
            collection.ShouldBeEmpty();
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
