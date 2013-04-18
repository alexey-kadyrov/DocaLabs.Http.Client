using System;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping
{
    [Subject(typeof(QueryStringBuilder))]
    class when_adding_single_key_value_pair_once
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of =
            () => builder.Add("oneKey", "oneValue");

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=oneValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_same_key_with_different_values
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () =>
        {
            builder.Add("oneKey", "firstValue");
            builder.Add("oneKey", "secondValue");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=firstValue&oneKey=secondValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_several_key_and_value_pairs
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () =>
        {
            builder.Add("firstKey", "firstValue");
            builder.Add("secondKey", "secondValue");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=firstValue&secondKey=secondValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_values_that_contain_simbols_required_encodings
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () =>
        {
            builder.Add("firstKey", "first Value");
            builder.Add("secondKey", "second&Value");
            builder.Add("thirdKey", "third+Value");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=first+Value&secondKey=second%26Value&thirdKey=third%2bValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_null_key
    {
        static QueryStringBuilder builder;
        static Exception exception;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of =
            () => exception = Catch.Exception(() => builder.Add(null, "oneValue"));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_key_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("key");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_null_key_and_value
    {
        static QueryStringBuilder builder;
        static Exception exception;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of =
            () => exception = Catch.Exception(() => builder.Add(null, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_key_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("key");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_null_value
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of =
            () => builder.Add("oneKey", null);

        It should_not_change_anything =
            () => builder.ToString().ShouldBeEmpty();
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_single_key_value_pair_once_using_enumerable_overload
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () => builder.Add(new NameValueCollection
        {
            { "oneKey", "oneValue" }
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=oneValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_same_key_with_different_values_using_enumerable_overload
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () => builder.Add(new NameValueCollection
        {
            { "oneKey", "firstValue" },
            { "oneKey", "secondValue" }
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=firstValue&oneKey=secondValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_several_key_and_value_pairs_using_enumerable_overload
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () => builder.Add(new NameValueCollection
        {
            { "firstKey", "firstValue" },
            { "secondKey", "secondValue" },
            { "secondKey", "thirdValue" }
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=firstValue&secondKey=secondValue&secondKey=thirdValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_values_that_contain_simbols_required_encodings_using_enumerable_overload
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of = () => builder.Add(new NameValueCollection
        {
            { "firstKey", "first Value" },
            { "secondKey", "second&Value" },
            { "thirdKey", "third+Value" }
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=first+Value&secondKey=second%26Value&thirdKey=third%2bValue");
    }

    [Subject(typeof(QueryStringBuilder))]
    class when_adding_null_collection
    {
        static QueryStringBuilder builder;

        Establish context =
            () => builder = new QueryStringBuilder();

        Because of =
            () => builder.Add(null);

        It should_not_change_anything =
            () => builder.ToString().ShouldBeEmpty();
    }
}
