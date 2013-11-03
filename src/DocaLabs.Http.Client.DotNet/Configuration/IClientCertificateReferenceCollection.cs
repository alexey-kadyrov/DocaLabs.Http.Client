using System.Collections;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientCertificateReference objects.
    /// </summary>
    public interface IClientCertificateReferenceCollection : IEnumerable
    {
        /// <summary>
        /// Gets all keys defined in the collection.
        /// </summary>
        string[] AllKeys { get; }

        /// <summary>
        /// Gets or sets the element object by its key.
        /// </summary>
        IClientCertificateReference this[string key] { get; set; }

        /// <summary>
        /// Gets or sets the element object by its position.
        /// </summary>
        IClientCertificateReference this[int index] { get; set; }

        /// <summary>
        /// Returns a position of the specified element.
        /// </summary>
        int IndexOf(IClientCertificateReference element);

        /// <summary>
        /// Adds a new element object to the collection.
        /// </summary>
        void Add(IClientCertificateReference element);

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes element by specified key.
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// Removes element.
        /// </summary>
        void Remove(IClientCertificateReference element);

        /// <summary>
        /// Removes element by specified position.
        /// </summary>
        void RemoveAt(int index);
    }
}