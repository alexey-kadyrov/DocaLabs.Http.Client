using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [Subject(typeof(CollectionExtensions))]
    class when_adding_range_of_values_to_collection
    {
        static ICollection<string> collection;

        Establish context =
            () => collection = new Collection<string>();

        Because of =
            () => collection.AddRange(new[] { "1", "2" });

        It should_add_those_values =
            () => collection.ShouldContainOnly("1", "2");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_range_of_values_to_null_collection
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((ICollection<string>) null).AddRange(new[] {"1", "2"}));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_collection_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("collection");
    }

    [Subject(typeof(CollectionExtensions))]
    class when_adding_null_range_of_values_to_collection
    {
        static ICollection<string> collection; 
        static Exception exception;

        Establish context =
            () => collection = new Collection<string>();

        Because of =
            () => exception = Catch.Exception(() => collection.AddRange(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_items_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("items");
    }
}
