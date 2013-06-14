using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    // ReSharper disable AssignNullToNotNullAttribute

    [Subject(typeof(CollectionExtensions))]
    class when_adding_values_to_empty_collection
    {
        static NameValueCollection target_collection;
        static IEnumerable<string> values;

        Establish context = () =>
        {
            target_collection = new NameValueCollection();
            values = new [] { "value11", "value12" };
        };

        Because of =
            () => target_collection.Add("key-1", values);

        It should_contain_all_added_values_with_the_specified_key =
            () => target_collection.GetValues("key-1").ShouldContainOnly("value11", "value12");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_values_to_collection_with_exiting_values
    {
        static NameValueCollection target_collection;
        static IEnumerable<string> values;

        Establish context = () =>
        {
            target_collection = new NameValueCollection
            {
                { "key-0", "value0" },
                { "key-1", "value1" }
            };
            values = new[] { "value11", "value12" };
        };

        Because of =
            () => target_collection.Add("key-1", values);

        It should_contain_all_added_values_and_pre_exiting_with_the_same_key =
            () => target_collection.GetValues("key-1").ShouldContainOnly("value11", "value12", "value1");

        It should_still_contain_all_exiting_values_with_different_key =
            () => target_collection.GetValues("key-0").ShouldContainOnly("value0");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_null_collection_of_values
    {
        static NameValueCollection target_collection;

        Establish context = () =>
        {
            target_collection = new NameValueCollection
            {
                { "key-0", "value0" },
                { "key-0", "value1" }
            };
        };

        Because of =
            () => target_collection.Add("key-1", ((IEnumerable<string>)null));

        It should_not_add_new_key =
            () => target_collection.AllKeys.ShouldContainOnly("key-0");

        It should_still_contain_all_exiting_values =
            () => target_collection.GetValues("key-0").ShouldContainOnly("value0", "value1");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_values_to_null_collection
    {
        static NameValueCollection target_collection;
        static IEnumerable<string> values;
        static Exception exception;

        Establish context = 
            () => values = new[] { "value11", "value12" };

        Because of =
            () => exception = Catch.Exception(() => target_collection.Add("key-1", values));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_collection_parameter =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("collection");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_values_with_null_key
    {
        static NameValueCollection target_collection;
        static IEnumerable<string> values;

        Establish context = () =>
        {
            target_collection = new NameValueCollection();
            values = new[] { "value11", "value12" };
        };

        Because of =
            () => target_collection.Add(null, values);

        It should_contain_all_added_values_with_the_specified_key =
            () => target_collection.GetValues(null).ShouldContainOnly("value11", "value12");
    }

    // ReSharper restore AssignNullToNotNullAttribute
}
