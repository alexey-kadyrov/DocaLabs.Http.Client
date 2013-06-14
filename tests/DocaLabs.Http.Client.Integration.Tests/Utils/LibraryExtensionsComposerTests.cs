using System;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Integration.Tests.Utils
{
    [Subject(typeof(LibraryExtensionsComposer))]
    class when_composing_using_default_search_pattern
    {
        static LibraryExtensionsComposer composer;
        static ExtensionLoader extension_loader;

        Cleanup after_each =
            () => composer.Dispose();

        Establish context = () =>
        {
            composer = new LibraryExtensionsComposer();
            extension_loader = new ExtensionLoader();
        };

        Because of =
            () => composer.ComposePartsFor(extension_loader);

        It should_load_existing_extension =
            () => extension_loader.SerializerExtension.ShouldNotBeNull();

        It should_not_load_existing_which_does_not_extension =
            () => extension_loader.DeserializerExtension.ShouldBeNull();

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public IJsonSerializer SerializerExtension { get; set; }

            [Import(AllowDefault = true)]
            public IJsonDeserializer DeserializerExtension { get; set; }
        }
    }

    [Subject(typeof(LibraryExtensionsComposer))]
    class when_composing_using_specified_search_pattern
    {
        static LibraryExtensionsComposer composer;
        static ExtensionLoader extension_loader;

        Cleanup after_each =
            () => composer.Dispose();

        Establish context = () =>
        {
            composer = new LibraryExtensionsComposer("DocaLabs.Http.Client.Extension.Test.Example.dll");
            extension_loader = new ExtensionLoader();
        };

        Because of =
            () => composer.ComposePartsFor(extension_loader);

        It should_load_existing_extension =
            () => extension_loader.SerializerExtension.ShouldNotBeNull();

        It should_not_load_existing_which_does_not_extension =
            () => extension_loader.DeserializerExtension.ShouldBeNull();

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public IJsonSerializer SerializerExtension { get; set; }

            [Import(AllowDefault = true)]
            public IJsonDeserializer DeserializerExtension { get; set; }
        }
    }

    [Subject(typeof(LibraryExtensionsComposer))]
    class when_composing_with_non_existing_extensions_for_required_import_from_existing_assembly
    {
        static LibraryExtensionsComposer composer;
        static ExtensionLoader extension_loader;
        static Exception exception;

        Cleanup after_each =
            () => composer.Dispose();

        Establish context = () =>
        {
            composer = new LibraryExtensionsComposer();
            extension_loader = new ExtensionLoader();
        };

        Because of =
            () => exception = Catch.Exception(() => composer.ComposePartsFor(extension_loader));

        It should_throw_composition_exception =
            () => exception.ShouldBeOfType<CompositionException>();

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public IJsonSerializer SerializerExtension { [UsedImplicitly] get; set; }

            [Import(AllowDefault = false)]
            public IJsonDeserializer DeserializerExtension { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(LibraryExtensionsComposer))]
    class when_composing_with_non_existing_assembly_for_required_import
    {
        static LibraryExtensionsComposer composer;
        static ExtensionLoader extension_loader;
        static Exception exception;

        Cleanup after_each =
            () => composer.Dispose();

        Establish context = () =>
        {
            composer = new LibraryExtensionsComposer("some-non-existent-assembly");
            extension_loader = new ExtensionLoader();
        };

        Because of =
            () => exception = Catch.Exception(() => composer.ComposePartsFor(extension_loader));

        It should_throw_composition_exception =
            () => exception.ShouldBeOfType<CompositionException>();

        class ExtensionLoader
        {
            [Import(AllowDefault = false)]
            public IJsonSerializer SerializerExtension { [UsedImplicitly] get; set; }

            [Import(AllowDefault = false)]
            public IJsonDeserializer DeserializerExtension { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(LibraryExtensionsComposer))]
    class when_composing_with_non_existing_assembly_for_optional_import
    {
        static LibraryExtensionsComposer composer;
        static ExtensionLoader extension_loader;

        Cleanup after_each =
            () => composer.Dispose();

        Establish context = () =>
        {
            composer = new LibraryExtensionsComposer("some-non-existent-assembly");
            extension_loader = new ExtensionLoader();
        };

        Because of =
            () => composer.ComposePartsFor(extension_loader);

        It should_not_load_extension =
            () => extension_loader.SerializerExtension.ShouldBeNull();

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public IJsonSerializer SerializerExtension { get; set; }
        }
    }

    [Subject(typeof(LibraryExtensionsComposer))]
    class when_instantiating_library_extension_composer_with_null_search_pattern
    {
        static LibraryExtensionsComposer composer;
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => composer = new LibraryExtensionsComposer(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_search_pattern_parameter =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("searchPattern");
    }
}
