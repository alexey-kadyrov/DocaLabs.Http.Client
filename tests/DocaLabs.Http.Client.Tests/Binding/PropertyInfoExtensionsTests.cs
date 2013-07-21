using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_be_implicit_path_or_query
    {
        It should_return_true_for_simple_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_object_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_dictionary_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_simple_array_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_object_array_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_false_for_webheadercollection_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_indexer_without_hints =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsImplicitUrlPathOrQuery().ShouldBeFalse();
    }

    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedMember.Local
    // ReSharper disable UnusedParameter.Local

    class TestModel
    {
        public string this[int idx] { get { return ""; } set { } }
        public string SimpleWithoutHint { get; set; }
        public object ObjectWithoutHint { get; set; }
        public IDictionary DictionaryWithoutHint { get; set; }
        public IDictionary<string, string> GenericDictionaryWithoutHint { get; set; }
        public NameValueCollection NameValueCollectionWithoutHint { get; set; }
        public string[] SimpleArrayWithoutHint { get; set; }
        public object[] ObjectArrayWithoutHint { get; set; }
        public WebHeaderCollection WebHeaderCollectionWithoutHint { get; set; }
        public ICredentials CredentialsWithoutHint { get; set; }

        [RequestUse(RequestUseTargets.UrlQuery)]
        public string this[string idx] { get { return ""; } set { } }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public string SimpleWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public object ObjectWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public IDictionary DictionaryWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public IDictionary<string, string> GenericDictionaryWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public NameValueCollection NameValueCollectionWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public string[] SimpleArrayWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public object[] ObjectArrayWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public WebHeaderCollection WebHeaderCollectionWithQueryHint { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public ICredentials CredentialsWithQueryHint { get; set; }

        [RequestUse(RequestUseTargets.UrlPath)]
        public string this[long idx] { get { return ""; } set { } }
        [RequestUse(RequestUseTargets.UrlPath)]
        public string SimpleWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public object ObjectWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public IDictionary DictionaryWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public IDictionary<string, string> GenericDictionaryWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public NameValueCollection NameValueCollectionWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public string[] SimpleArrayWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public object[] ObjectArrayWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public WebHeaderCollection WebHeaderCollectionWithPathHint { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public ICredentials CredentialsWithPathHint { get; set; }

        [RequestUse(RequestUseTargets.RequestHeader)]
        public string this[decimal idx] { get { return ""; } set { } }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public string SimpleWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public object ObjectWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public IDictionary DictionaryWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public IDictionary<string, string> GenericDictionaryWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public NameValueCollection NameValueCollectionWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public string[] SimpleArrayWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public object[] ObjectArrayWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public WebHeaderCollection WebHeaderCollectionWithHeaderHint { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public ICredentials CredentialsWithHeaderHint { get; set; }

        [RequestUse(RequestUseTargets.Ignore)]
        public string this[double idx] { get { return ""; } set { } }
        [RequestUse(RequestUseTargets.Ignore)]
        public string SimpleWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public object ObjectWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public IDictionary DictionaryWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public IDictionary<string, string> GenericDictionaryWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public NameValueCollection NameValueCollectionWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public string[] SimpleArrayWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public object[] ObjectArrayWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public WebHeaderCollection WebHeaderCollectionWithIgnoreHint { get; set; }
        [RequestUse(RequestUseTargets.Ignore)]
        public ICredentials CredentialsWithIgnoreHint { get; set; }
    }

    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
    // ReSharper restore ValueParameterNotUsed
}
