using System;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [Subject(typeof(StringExtensions))]
    class when_replacing_a_single_occurrence_ignoring_the_case_in_the_begging_of_the_string
    {
        static string result;

        Because of =
            () => result = "$replaCE-Me$ World!".Replace("$replace-me$", "Hello", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Hello World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_a_single_occurrence_ignoring_the_case_in_the_middle_of_the_string
    {
        static string result;

        Because of =
            () => result = "Hello$replaCE-Me$!".Replace("$replace-me$", " World", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Hello World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_a_single_occurrence_ignoring_the_case_in_the_end_of_the_string
    {
        static string result;

        Because of =
            () => result = "Hello $replaCE-Me$".Replace("$replace-me$", "World!", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Hello World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_a_single_occurrence_with_mismatched_case
    {
        static string result;

        Because of =
            () => result = "Hello$replaCE-Me$!".Replace("$replace-me$", " World", StringComparison.Ordinal);

        It should_not_alter_the_string =
            () => result.ShouldEqual("Hello$replaCE-Me$!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_a_single_occurrence_with_mismatched_old_value
    {
        static string result;

        Because of =
            () => result = "Hello$replaCE-Me$!".Replace("something-different", " World", StringComparison.Ordinal);

        It should_not_alter_the_string =
            () => result.ShouldEqual("Hello$replaCE-Me$!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_multiple_occurrences_starting_from_the_begining
    {
        static string result;

        Because of =
            () => result = "$replaCE-Me$, $replaCE-Me$ World!".Replace("$replace-me$", "Crazy", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Crazy, Crazy World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_multiple_occurrences_in_the_middle
    {
        static string result;

        Because of =
            () => result = "Hello $replaCE-Me$, $replaCE-Me$ World!".Replace("$replace-me$", "Crazy", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Hello Crazy, Crazy World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_multiple_occurrences_where_the_last_is_at_the_end
    {
        static string result;

        Because of =
            () => result = "World is $replaCE-Me$, $replaCE-Me$".Replace("$replace-me$", "Crazy", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("World is Crazy, Crazy");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_in_empty_string
    {
        static string result;

        Because of =
            () => result = "".Replace("something-different", " World", StringComparison.Ordinal);

        It should_not_alter_the_string =
            () => result.ShouldEqual("");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_by_empty_string
    {
        static string result;

        Because of =
            () => result = "Hello $replaCE-Me$ $replaCE-Me$ World!".Replace("$replace-me$", "", StringComparison.OrdinalIgnoreCase);

        It should_replace_it =
            () => result.ShouldEqual("Hello   World!");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_in_null_string
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((string)null).Replace("something", "World", StringComparison.Ordinal));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_str_parameter =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("str");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_the_null_string
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => "Hello World!".Replace(null, "World", StringComparison.Ordinal));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_old_value_parameter =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("oldValue");
    }

    [Subject(typeof(StringExtensions))]
    class when_replacing_by_null_value
    {
        static string result;

        Because of =
            () => result = "Hello $replaCE-Me$, $replaCE-Me$ World!".Replace("$replace-me$", null, StringComparison.OrdinalIgnoreCase);

        It should_replace_it_by_empty_strings =
            () => result.ShouldEqual("Hello ,  World!");
    }
}