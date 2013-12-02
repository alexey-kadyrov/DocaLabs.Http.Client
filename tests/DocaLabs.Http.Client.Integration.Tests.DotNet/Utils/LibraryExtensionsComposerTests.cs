using System;
using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Extension.Test.Example;
using DocaLabs.Http.Client.Integration.Tests.DotNet.Annotations;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet.Utils
{
    [TestClass]
    public class when_composing_using_default_search_pattern
    {
        static LibraryExtensionsComposer _composer;
        static ExtensionLoader _extensionLoader;

        [ClassCleanup]
        public static void Cleanup()
        {
            _composer.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _composer = new LibraryExtensionsComposer();
            _extensionLoader = new ExtensionLoader();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _composer.ComposePartsFor(_extensionLoader);
        }

        [TestMethod]
        public void it_should_load_existing_extension()
        {
            _extensionLoader.TestImportExtension1.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_not_load_existing_which_does_not_extension()
        {
            _extensionLoader.TestImportExtension2.ShouldBeNull();
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public ITestImport1 TestImportExtension1 { get; set; }

            [Import(AllowDefault = true)]
            public ITestImport2 TestImportExtension2 { get; set; }
        }
    }

    [TestClass]
    public class when_composing_using_specified_search_pattern
    {
        static LibraryExtensionsComposer _composer;
        static ExtensionLoader _extensionLoader;

        [ClassCleanup]
        public static void Cleanup()
        {
            _composer.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _composer = new LibraryExtensionsComposer("DocaLabs.Http.Client.Extension.Test.Example.dll");
            _extensionLoader = new ExtensionLoader();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _composer.ComposePartsFor(_extensionLoader);
        }

        [TestMethod]
        public void it_should_load_existing_extension()
        {
            _extensionLoader.TestImportExtension1.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_not_load_existing_which_does_not_extension()
        {
            _extensionLoader.TestImportExtension2.ShouldBeNull();
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public ITestImport1 TestImportExtension1 { get; set; }

            [Import(AllowDefault = true)]
            public ITestImport2 TestImportExtension2 { get; set; }
        }
    }

    [TestClass]
    public class when_composing_with_non_existing_extensions_for_required_import_from_existing_assembly
    {
        static LibraryExtensionsComposer _composer;
        static ExtensionLoader _extensionLoader;
        static Exception _exception;

        [ClassCleanup]
        public static void Cleanup()
        {
            _composer.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _composer = new LibraryExtensionsComposer();
            _extensionLoader = new ExtensionLoader();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _composer.ComposePartsFor(_extensionLoader));
        }

        [TestMethod]
        public void it_should_throw_composition_exception()
        {
            _exception.ShouldBeOfType<CompositionException>();
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public ITestImport1 TestImportExtension1 { [UsedImplicitly] get; set; }

            [Import(AllowDefault = false)]
            public ITestImport2 TestImportExtension2 { [UsedImplicitly] get; set; }
        }
    }

    [TestClass]
    public class when_composing_with_non_existing_assembly_for_required_import
    {
        static LibraryExtensionsComposer _composer;
        static ExtensionLoader _extensionLoader;
        static Exception _exception;

        [ClassCleanup]
        public static void Cleanup()
        {
            _composer.Dispose();
        }

        [ClassInitialize]
        public static void EstablisContext(TestContext context)
        {
            _composer = new LibraryExtensionsComposer("some-non-existent-assembly");
            _extensionLoader = new ExtensionLoader();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _composer.ComposePartsFor(_extensionLoader));
        }

        [TestMethod]
        public void it_should_throw_composition_exception()
        {
            _exception.ShouldBeOfType<CompositionException>();
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = false)]
            public ITestImport1 TestImportExtension1 { [UsedImplicitly] get; set; }

            [Import(AllowDefault = false)]
            public ITestImport2 TestImportExtension2 { [UsedImplicitly] get; set; }
        }
    }

    [TestClass]
    public class when_composing_with_non_existing_assembly_for_optional_import
    {
        static LibraryExtensionsComposer _composer;
        static ExtensionLoader _extensionLoader;

        [ClassCleanup]
        public static void Cleanup()
        {
            _composer.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _composer = new LibraryExtensionsComposer("some-non-existent-assembly");
            _extensionLoader = new ExtensionLoader();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _composer.ComposePartsFor(_extensionLoader);
        }

        [TestMethod]
        public void it_should_not_load_extension()
        {
            _extensionLoader.TestImportExtension1.ShouldBeNull();
        }

        class ExtensionLoader
        {
            [Import(AllowDefault = true)]
            public ITestImport1 TestImportExtension1 { get; set; }
        }
    }

    [TestClass]
    public class when_instantiating_library_extension_composer_with_null_search_pattern
    {
        static Exception _exception;

        [ClassInitialize]
        public static void BecauseOf(TestContext context)
        {
            _exception = Catch.Exception(() => new LibraryExtensionsComposer(null));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_search_pattern_parameter()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("searchPattern");
        }
    }
}
