using System;
using System.Collections;
using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Contains a base collection type to implement configuration collections.
    /// </summary>
    public abstract class ConfigurationElementCollectionBase<TKey, TElement> : ConfigurationElementCollection
        where TKey : class
        where TElement : ConfigurationElement, new()
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
        /// Always returns false letting the element to be modified at runtime.
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// Gets or sets the EntityTableNameElement object by its key.
        /// </summary>
        public virtual TElement this[TKey key]
        {
            get { return (TElement)BaseGet(key); }

            set
            {
                if (BaseGet(key) != null)
                    BaseRemove(key);

                Add(value);
            }
        }

        /// <summary>
        /// Gets or sets the EntityTableNameElement object by its position.
        /// </summary>
        public TElement this[int index]
        {
            get { return (TElement)BaseGet(index); }

            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
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
            return BaseIndexOf(element);
        }

        /// <summary>
        /// Adds a new EntityTableNameElement object to the collection.
        /// </summary>
        public virtual void Add(TElement element)
        {
            BaseAdd(element);
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
            BaseRemove(GetElementKey(element));
        }

        /// <summary>
        /// Removes element by specified position.
        /// </summary>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Creates a new instance of the element.
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<TElement>();
        }
    }
}