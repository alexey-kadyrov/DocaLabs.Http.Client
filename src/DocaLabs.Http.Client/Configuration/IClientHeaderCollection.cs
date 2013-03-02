using System.Collections;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a collection of IClientHeader objects.
    /// </summary>
    public interface IClientHeaderCollection : IEnumerable
    {
        /// <summary>
        /// Gets all keys defined in the collection.
        /// </summary>
        string[] AllKeys { get; }

        /// <summary>
        /// Gets or sets the element object by its key.
        /// </summary>
        IClientHeader this[string key] { get; set; }

        /// <summary>
        /// Gets or sets the element object by its position.
        /// </summary>
        IClientHeader this[int index] { get; set; }

        /// <summary>
        /// Returns a position of the specified element.
        /// </summary>
        int IndexOf(IClientHeader element);

        /// <summary>
        /// Adds a new element object to the collection.
        /// </summary>
        void Add(IClientHeader element);

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
        void Remove(IClientHeader element);

        /// <summary>
        /// Removes element by specified position.
        /// </summary>
        void RemoveAt(int index);
    }
}