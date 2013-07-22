using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_to_be_implicit_path_or_query
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

    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_to_be_explicit_query
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

    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_to_be_explicit_path
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

    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_to_be_header
    {
        // implicit
        It should_return_false_for_indexer_without_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(int)).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_array_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithoutHint).IsHeader().ShouldBeFalse();

        It should_return_true_for_webheadercollection_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithoutHint).IsHeader().ShouldBeTrue();

        It should_return_false_for_icredentials_property_without_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithoutHint).IsHeader().ShouldBeFalse();

        // query
        It should_return_false_for_indexer_with_query_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(string)).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithQueryHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_query_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithQueryHint).IsHeader().ShouldBeFalse();

        // path
        It should_return_false_for_indexer_with_path_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(long)).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithPathHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_path_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithPathHint).IsHeader().ShouldBeFalse();

        // header
        It should_return_false_for_indexer_with_header_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(decimal)).IsHeader().ShouldBeFalse();

        It should_return_true_for_simple_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_object_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_generic_dictionary_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_namevaluecollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_simple_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_object_array_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_webheadercollection_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithHeaderHint).IsHeader().ShouldBeTrue();

        It should_return_true_for_icredentials_property_with_header_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithHeaderHint).IsHeader().ShouldBeTrue();

        // ignore
        It should_return_false_for_indexer_with_ignore_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(double)).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithIgnoreHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_ignore_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithIgnoreHint).IsHeader().ShouldBeFalse();

        // request serialization
        It should_return_false_for_indexer_with_request_serialization_hint =
            () => Reflect<TestModel>.GetIndexerInfo(typeof(short)).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.DictionaryWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_generic_dictionary_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.GenericDictionaryWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_namevaluecollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.NameValueCollectionWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_simple_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.SimpleArrayWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_object_array_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.ObjectArrayWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_webheadercollection_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.WebHeaderCollectionWithRequestSerializationHint).IsHeader().ShouldBeFalse();

        It should_return_false_for_icredentials_property_with_request_serialization_hint =
            () => Reflect<TestModel>.GetPropertyInfo(x => x.CredentialsWithRequestSerializationHint).IsHeader().ShouldBeFalse();
    }

    [Subject(typeof(PropertyInfoExtensions))]
    class when_checking_whenever_property_is_considered_it_to_be_creadetials
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
