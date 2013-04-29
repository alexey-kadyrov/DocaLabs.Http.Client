using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace DocaLabs.Http.Client.Binding.Utils
{
    /// <summary>
    /// Provides means to discover alternative implementations (extensions) using MEF.
    /// </summary>
    public class LibraryExtensionsComposer : IDisposable
    {
        readonly CompositionContainer _compositionContainer;

        /// <summary>
        /// Initializes an instance of the LibraryExtensionsComposer class.
        /// </summary>
        public LibraryExtensionsComposer(string searchPattern = "DocaLabs.Extensions.Http.Client.*")
        {
            // An aggregate catalogue that combines multiple catalogues
            var catalog = new AggregateCatalog();

            // Adds all the parts found in the base folder where current assembly resolver looks for.
            // Cannot use BaseDirectory as the first choice due that in the web application it will be the parent of the "bin" folder.
            catalog.Catalogs.Add(new DirectoryCatalog(
                    string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.RelativeSearchPath)
                        ? AppDomain.CurrentDomain.BaseDirectory
                        : AppDomain.CurrentDomain.RelativeSearchPath, searchPattern));

            // Create the CompositionContainer with the parts in the catalogue.
            _compositionContainer = new CompositionContainer(catalog);
        }

        /// <summary>
        /// Fill the imports of the object.
        /// </summary>
        /// <param name="o">The object to compose for.</param>
        /// <param name="isOptional">If the parameter is false and there is no import found the CompositionException will be thrown.</param>
        public void ComposePartsFor(object o, bool isOptional = true)
        {
            try
            {
                _compositionContainer.ComposeParts(o);
            }
            catch (CompositionException)
            {
                if (!isOptional)
                    throw;
            }
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
