using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Provides means to discover alternative implementations (extensions) using MEF.
    /// </summary>
    public class LibraryExtensionsComposer : IDisposable
    {
        /// <summary>
        /// Defines the default search pattern for searching assemblies.
        /// </summary>
        public const string DeafultSearchPattern = "DocaLabs.Http.Client.Extension.*";

        readonly CompositionContainer _compositionContainer;

        /// <summary>
        /// Initializes an instance of the LibraryExtensionsComposer class with the default search pattern.
        /// </summary>
        public LibraryExtensionsComposer()
            : this(DeafultSearchPattern)
        {
        }

        /// <summary>
        /// Initializes an instance of the LibraryExtensionsComposer class with the specified search pattern.
        /// </summary>
        public LibraryExtensionsComposer(string searchPattern)
        {
            if(searchPattern == null)
                throw new ArgumentNullException("searchPattern");

            var catalog = new AggregateCatalog();

            // Cannot use BaseDirectory as the first choice due that in the web application it will be the parent of the "bin" folder.
            catalog.Catalogs.Add(new DirectoryCatalog(
                    string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.RelativeSearchPath)
                        ? AppDomain.CurrentDomain.BaseDirectory
                        : AppDomain.CurrentDomain.RelativeSearchPath, searchPattern));

            _compositionContainer = new CompositionContainer(catalog);
        }

        /// <summary>
        /// Fill the imports of the object.
        /// </summary>
        /// <param name="o">The object to compose for.</param>
        public void ComposePartsFor(object o)
        {
            _compositionContainer.ComposeParts(o);
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// Releases all resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if(!disposing)
                return;
            
            _compositionContainer.Dispose();
        }
    }
}
