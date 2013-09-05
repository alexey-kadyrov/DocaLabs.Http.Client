using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_implicit_path_or_query
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_true_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_true_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeTrue();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // all hints
        It should_return_false_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsImplicitUrlPathOrQuery().ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_explicit_query
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsExplicitUrlQuery().ShouldBeFalse();

        // all hints
        It should_return_true_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsExplicitUrlQuery().ShouldBeTrue();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_true_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsExplicitUrlQuery().ShouldBeTrue();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsExplicitUrlQuery().ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsExplicitUrlQuery().ShouldBeFalse();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsExplicitUrlQuery().ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsExplicitUrlQuery().ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsExplicitUrlQuery().ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_explicit_path
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsExplicitUrlPath().ShouldBeFalse();

        // all hints
        It should_return_true_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsExplicitUrlPath().ShouldBeTrue();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsExplicitUrlPath().ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_true_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsExplicitUrlPath().ShouldBeTrue();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsExplicitUrlPath().ShouldBeFalse();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsExplicitUrlPath().ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsExplicitUrlPath().ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsExplicitUrlPath().ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_header_using_implicit_conditions
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsHeader(true).ShouldBeFalse();

        It should_return_true_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsHeader(true).ShouldBeTrue();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsHeader(true).ShouldBeFalse();

        // all hints
        It should_return_true_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsHeader(true).ShouldBeTrue();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsHeader(true).ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsHeader(true).ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsHeader(true).ShouldBeFalse();

        It should_return_true_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsHeader(true).ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsHeader(true).ShouldBeTrue();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsHeader(true).ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsHeader(true).ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsHeader(true).ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_header_not_using_implicit_conditions
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsHeader(false).ShouldBeFalse();

        // all hints
        It should_return_true_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsHeader(false).ShouldBeTrue();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsHeader(false).ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsHeader(false).ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsHeader(false).ShouldBeFalse();

        It should_return_true_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsHeader(false).ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsHeader(false).ShouldBeTrue();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsHeader(false).ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsHeader(false).ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsHeader(false).ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_creadetials
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsCredentials().ShouldBeFalse();

        It should_return_true_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsCredentials().ShouldBeTrue();

        // all hints
        It should_return_false_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsCredentials().ShouldBeFalse();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsCredentials().ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsCredentials().ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsCredentials().ShouldBeFalse();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsCredentials().ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_false_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsCredentials().ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsCredentials().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsCredentials().ShouldBeFalse();
    }

    [Subject(typeof(RequestUsageExtensions))]
    class when_checking_whenever_property_is_considered_to_be_serializable_to_request_stream
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsRequestStream().ShouldBeFalse();

        // all hints
        It should_return_true_for_all_hints =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WithAllHints).IsRequestStream().ShouldBeTrue();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsRequestStream().ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsRequestStream().ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsRequestStream().ShouldBeFalse();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsRequestStream().ShouldBeFalse();

        // RequestBodyAsForm
        It should_return_true_for_indexer_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(char)).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_simple_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_object_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_object_array_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_request_body_as_form_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestBodyAsFormHint).IsRequestStream().ShouldBeTrue();

        // request serialization
        It should_return_true_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsRequestStream().ShouldBeTrue();
    }

    [Subject(typeof(RequestUsageExtensions), "TryGetRequestSerializer")]
    class when_trying_to_get_request_serializer_for_property_which_does_not_have_request_serialization_attribute
    {
        It should_return_null =
            () => Reflect<Model>.GetPropertyInfo(x => x.Value).TryGetRequestSerializer().ShouldBeNull();

        class Model
        {
            [UsedImplicitly]
            public string Value { get; set; }
        }
    }

    [Subject(typeof(RequestUsageExtensions), "TryGetRequestSerializer")]
    class when_trying_to_get_request_serializer_for_property_which_has_request_serialization_attribute
    {
        It should_return_specified_attribute =
            () => Reflect<Model>.GetPropertyInfo(x => x.Value).TryGetRequestSerializer().ShouldBeOfType<TestSerializerAttribute>();

        class Model
        {
            [TestSerializer, UsedImplicitly]
            public string Value { get; set; }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(RequestUsageExtensions), "TryGetRequestSerializer")]
    class when_trying_to_get_request_serializer_for_property_which_has_request_serialization_attribute_on_base_class_property
    {
        It should_return_specified_attribute =
            () => Reflect<Model>.GetPropertyInfo(x => x.Value).TryGetRequestSerializer().ShouldBeOfType<TestSerializerAttribute>();

        class BaseModel
        {
            [TestSerializer, UsedImplicitly]
            public virtual string Value { get; set; }
        }

        class Model :BaseModel
        {
            public override string Value { get; set; }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(RequestUsageExtensions), "IsSerializableToRequestBody")]
    class when_trying_to_get_whenever_the_type_is_serializable_into_request_body_for_type_which_does_not_have_request_serialization_attribute
    {
        It should_return_false =
            () => typeof (Model).IsSerializableToRequestBody().ShouldBeFalse();

        class Model
        {
        }
    }

    [Subject(typeof(RequestUsageExtensions), "IsSerializableToRequestBody")]
    class when_trying_to_get_whenever_the_type_is_serializable_into_request_body_for_type_which_has_request_serialization_attribute
    {
        It should_return_true =
            () => typeof(Model).IsSerializableToRequestBody().ShouldBeTrue();

        [TestSerializer]
        class Model
        {
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(RequestUsageExtensions), "IsSerializableToRequestBody")]
    class when_trying_to_get_whenever_the_type_is_serializable_into_request_body_for_type_which_has_request_serialization_attribute_on_the_base_class
    {
        It should_return_true =
            () => typeof(Model).IsSerializableToRequestBody().ShouldBeTrue();

        [TestSerializer]
        class BaseModel
        {
        }

        class Model : BaseModel
        {
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
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

        [SerializeAsForm, RequestUse(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath | RequestUseTargets.RequestHeader)]
        public string WithAllHints { get; set; }

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

        [SerializeAsForm]
        public string this[char idx] { get { return ""; } set { } }
        [SerializeAsForm]
        public string SimpleWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public object ObjectWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public IDictionary DictionaryWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public IDictionary<string, string> GenericDictionaryWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public NameValueCollection NameValueCollectionWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public string[] SimpleArrayWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public object[] ObjectArrayWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public WebHeaderCollection WebHeaderCollectionWithRequestBodyAsFormHint { get; set; }
        [SerializeAsForm]
        public ICredentials CredentialsWithRequestBodyAsFormHint { get; set; }

        [SerializeAsJson]
        public string this[short idx] { get { return ""; } set { } }
        [SerializeAsJson]
        public string SimpleWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public object ObjectWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public IDictionary DictionaryWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public IDictionary<string, string> GenericDictionaryWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public NameValueCollection NameValueCollectionWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public string[] SimpleArrayWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public object[] ObjectArrayWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public WebHeaderCollection WebHeaderCollectionWithRequestSerializationHint { get; set; }
        [SerializeAsJson]
        public ICredentials CredentialsWithRequestSerializationHint { get; set; }
    }

    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
    // ReSharper restore ValueParameterNotUsed
}
