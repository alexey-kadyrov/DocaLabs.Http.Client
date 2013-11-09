using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    // ReSharper disable AssignNullToNotNullAttribute

    /// <summary>
    /// Defines a base collection type to implement configuration collections.
    /// </summary>
    public abstract class ConfigurationElementCollectionBase<TKey, TElement> 
        : ConfigurationElementCollection, IReadOnlyList<TElement> 
            where TKey : class
            where TElement : class
    {
        readonly ConfigurationElementCollectionType _collectionType;
        readonly string _elementName;

        /// <summary>
        /// Gets the default type of configuration element collection that contains elements that can be merged across a hierarchy of configuration files.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return _collectionType; }
        }

        /// <summary>
        /// Gets the element's tag name.
        /// </summary>
        protected override string ElementName
        {
            get { return string.IsNullOrWhiteSpace(_elementName) ? base.ElementName : _elementName; }
        }

        /// <summary>
        /// Gets all keys defined in the collection.
        /// </summary>
        public virtual TKey[] AllKeys
        {
            get
            {
                var keys = BaseGetAllKeys();
                var array = new TKey[keys.Length];
                keys.CopyTo(array, 0);
                return array;
            }
        }

        /// <summary>
        /// Always returns true causing the exception to be thrown in an attempt to add a duplicate element to the collection.
        /// </summary>
        protected override bool ThrowOnDuplicate { get { return true; } }

        /// <summary>
        /// Gets or sets the element object by its key.
        /// </summary>
        public virtual TElement this[TKey key]
        {
            get { return BaseGet(key) as TElement; }

            set
            {
                if (BaseGet(key) != null)
                    BaseRemove(key);

                Add(value);
            }
        }

        /// <summary>
        /// Gets or sets the element object by its position.
        /// </summary>
        public TElement this[int index]
        {
            get { return BaseGet(index) as TElement; }

            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value as ConfigurationElement);
            }
        }

        /// <summary>
        /// Initializes an instance of the ConfigurationElementCollectionBase class.
        /// </summary>
        protected ConfigurationElementCollectionBase()
            : this(ConfigurationElementCollectionType.AddRemoveClearMap, null)
        {
        }

        /// <summary>
        /// Initializes an instance of the ConfigurationElementCollectionBase class with the specified element's name.
        /// </summary>
        protected ConfigurationElementCollectionBase(string elementName)
            : this(ConfigurationElementCollectionType.AddRemoveClearMap, elementName)
        {
        }

        /// <summary>
        /// Initializes an instance of the ConfigurationElementCollectionBase class with the specified collection type and element's name.
        /// </summary>
        protected ConfigurationElementCollectionBase(ConfigurationElementCollectionType collectionType, string elementName)
        {
            _collectionType = collectionType;
            _elementName = elementName;

            if (!string.IsNullOrWhiteSpace(elementName))
                AddElementName = elementName;
        }

        /// <summary>
        /// Initializes an instance of the ConfigurationElementCollectionBase class with the specified collection type, element's name, and comparer.
        /// </summary>
        protected ConfigurationElementCollectionBase(ConfigurationElementCollectionType collectionType, string elementName, IComparer comparer)
            : base(comparer)
        {
            _collectionType = collectionType;
            _elementName = elementName;
        }

        /// <summary>
        /// Returns a position of the specified element.
        /// </summary>
        public int IndexOf(TElement element)
        {
            return BaseIndexOf(element as ConfigurationElement);
        }

        /// <summary>
        /// Adds a new element object to the collection.
        /// </summary>
        public virtual void Add(TElement element)
        {
            BaseAdd(element as ConfigurationElement);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public virtual void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Removes element by specified key.
        /// </summary>
        public virtual void Remove(TKey key)
        {
            BaseRemove(key);
        }

        /// <summary>
        /// Removes element.
        /// </summary>
        public void Remove(TElement element)
        {
            BaseRemove(GetElementKey(element as ConfigurationElement));
        }

        /// <summary>
        /// Removes element by specified position.
        /// </summary>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            var enumerator = base.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (TElement)enumerator.Current;
            }
        }
    }

    // ReSharper restore AssignNullToNotNullAttribute
}