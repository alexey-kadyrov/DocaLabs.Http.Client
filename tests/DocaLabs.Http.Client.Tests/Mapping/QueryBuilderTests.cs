using System;
using System.Collections.Generic;
using DocaLabs.Http.Client.Mapping;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping
{
    [Subject(typeof(QueryBuilder))]
    class when_adding_single_key_value_pair_once
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of =
            () => builder.Add("oneKey", "oneValue");

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=oneValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_same_key_with_different_values
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () =>
        {
            builder.Add("oneKey", "firstValue");
            builder.Add("oneKey", "secondValue");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=firstValue&oneKey=secondValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_several_key_and_value_pairs
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () =>
        {
            builder.Add("firstKey", "firstValue");
            builder.Add("secondKey", "secondValue");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=firstValue&secondKey=secondValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_values_that_contain_simbols_required_encodings
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () =>
        {
            builder.Add("firstKey", "first Value");
            builder.Add("secondKey", "second&Value");
            builder.Add("thirdKey", "third+Value");
        };

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=first+Value&secondKey=second%26Value&thirdKey=third%2bValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_null_key
    {
        static QueryBuilder builder;
        static Exception exception;

        Establish context =
            () => builder = new QueryBuilder();

        Because of =
            () => exception = Catch.Exception(() => builder.Add(null, "oneValue"));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_key_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("key");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_null_key_and_value
    {
        static QueryBuilder builder;
        static Exception exception;

        Establish context =
            () => builder = new QueryBuilder();

        Because of =
            () => exception = Catch.Exception(() => builder.Add(null, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_key_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("key");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_null_value
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of =
            () => builder.Add("oneKey", null);

        It should_not_change_anything =
            () => builder.ToString().ShouldBeEmpty();
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_single_key_value_pair_once_using_enumerable_overload
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () => builder.Add(new List<KeyValuePair<string, IList<string>>>
        {
            new KeyValuePair<string, IList<string>>("oneKey", new [] {"oneValue"})
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=oneValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_same_key_with_different_values_using_enumerable_overload
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () => builder.Add(new List<KeyValuePair<string, IList<string>>>
        {
            new KeyValuePair<string, IList<string>>("oneKey", new [] {"firstValue", "secondValue"})
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("oneKey=firstValue&oneKey=secondValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_several_key_and_value_pairs_using_enumerable_overload
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () => builder.Add(new List<KeyValuePair<string, IList<string>>>
        {
            new KeyValuePair<string, IList<string>>("firstKey", new [] {"firstValue"}),
            new KeyValuePair<string, IList<string>>("secondKey", new [] {"secondValue", "thirdValue"})
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=firstValue&secondKey=secondValue&secondKey=thirdValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_values_that_contain_simbols_required_encodings_using_enumerable_overload
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of = () => builder.Add(new List<KeyValuePair<string, IList<string>>>
        {
            new KeyValuePair<string, IList<string>>("firstKey", new [] {"first Value"}),
            new KeyValuePair<string, IList<string>>("secondKey", new [] {"second&Value"}),
            new KeyValuePair<string, IList<string>>("thirdKey", new [] {"third+Value"})
        });

        It should_form_valid_query_string =
            () => builder.ToString().ShouldEqual("firstKey=first+Value&secondKey=second%26Value&thirdKey=third%2bValue");
    }

    [Subject(typeof(QueryBuilder))]
    class when_adding_null_collection
    {
        static QueryBuilder builder;

        Establish context =
            () => builder = new QueryBuilder();

        Because of =
            () => builder.Add(null);

        It should_not_change_anything =
            () => builder.ToString().ShouldBeEmpty();
    }
}
