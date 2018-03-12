using ModelerClient.DiagramEngine.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ModelerClient.DiagramEngine.Helpers
{
    public class TemplatedItemsList<TView, TItem> : IList where TView : BindableObject,IItemsTemplate where TItem : BindableObject
    {
        private static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(TItem), -1, BindingMode.OneWay, null, null, null, null, null);

        readonly TView itemsTemplate;

        readonly List<TItem> templatedObjects = new List<TItem>();

        Internals.ListProxy ListProxy;

        bool _disposed;

        private IEnumerable itemsSource;
        public IEnumerable ItemsSource
        {
            get
            {
                return itemsSource;
            }
            set
            {
                itemsSource = value;
                OnItemsSourceChanged();
            }
        }

        private DataTemplate itemTemplate;
        public DataTemplate ItemTemplate
        {
            get { return itemTemplate; }
            set
            {
                itemTemplate = value;
                OnItemTemplateChanged();
            }
        }

        internal TemplatedItemsList(TView itemsView)
        {
            itemsTemplate = itemsView ?? throw new ArgumentNullException("itemsView");
        }

        internal ListViewCachingStrategy CachingStrategy
        {
            get
            {
                return ListViewCachingStrategy.RetainElement;
            }
        }

       
        public void Dispose()
        {
            if (_disposed)
                return;

            for (var i = 0; i < templatedObjects.Count; i++)
            {
                TItem item = templatedObjects[i];
                if (item != null)
                    UnhookItem(item);
            }

            _disposed = true;
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            var i = 0;
            foreach (object item in ListProxy)
                yield return GetOrCreateContent(i++, item);
        }

        int IList.Add(object item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object item)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object item)
        {
            return IndexOf((TItem)item);
        }

        void IList.Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set { throw new NotSupportedException(); }
        }

        void IList.Remove(object item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count
        {
            get { return ListProxy.Count; }
        }

        public TItem this[int index]
        {
            get { return GetOrCreateContent(index, ListProxy[index]); }
        }

        public int IndexOf(TItem item)
        {
            return GetIndex(item);
        }

        public DataTemplate SelectDataTemplate(object item)
        {
            return ItemTemplate.SelectDataTemplate(item, itemsTemplate);
        }

        public TItem ActivateContent(int index, object item)
        {
            TItem content = ItemTemplate != null ? (TItem)ItemTemplate.CreateContent(item, itemsTemplate) : (TItem)itemsTemplate.CreateDefault(item);

            content = UpdateContent(content, index, item);

            return content;
        }

        public TItem CreateContent(int index, object item, bool insert = false)
        {
            var content = ActivateContent(index, item);

            if ((CachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
                return content;

            for (int i = templatedObjects.Count; i <= index; i++)
                templatedObjects.Add(null);

            if (!insert)
                templatedObjects[index] = content;
            else
                templatedObjects.Insert(index, content);

            return content;
        }
       
        internal static int GetIndex(TItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return (int)item.GetValue(IndexProperty);
        }

       
        internal TItem GetOrCreateContent(int index, object item)
        {
            TItem content;
            if (templatedObjects.Count <= index || (content = templatedObjects[index]) == null)
                content = CreateContent(index, item);

            return content;
        }

        internal TItem UpdateContent(TItem content, int index, object item)
        {
            content.BindingContext = item;

            SetIndex(content, index);

            itemsTemplate.SetupContent(content, index);

            return content;
        }

        internal TItem UpdateContent(TItem content, int index)
        {
            object item = ListProxy[index];
            return UpdateContent(content, index, item);
        }



        IList ConvertContent(int startingIndex, IList items, bool forceCreate = false, bool setIndex = false)
        {
            var contentItems = new List<TItem>(items.Count);
            for (var i = 0; i < items.Count; i++)
            {
                int index = i + startingIndex;
                TItem content = !forceCreate ? GetOrCreateContent(index, items[i]) : CreateContent(index, items[i]);
                if (setIndex)
                    SetIndex(content, index);

                contentItems.Add(content);
            }

            return contentItems;
        }

     


        void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

      
      

        void OnItemsSourceChanged()
        {
            if(ListProxy != null)
                ListProxy.CollectionChanged -= OnProxyCollectionChanged;

            if (itemsSource == null)
                ListProxy = new Internals.ListProxy(new object[0]);
            else
                ListProxy = new Internals.ListProxy(itemsSource);

            ListProxy.CollectionChanged += OnProxyCollectionChanged;
            OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void OnItemTemplateChanged()
        {
            if (ListProxy == null || ListProxy.Count == 0)
                return;

            OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void OnProxyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnProxyCollectionChanged(sender, e, true);
        }

        void OnProxyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, bool fixWindows = true)
        {
            if ((CachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
            {
                OnCollectionChanged(e);
                return;
            }

            /* HACKAHACKHACK: LongListSelector on WP SL has a bug in that it completely fails to deal with
			 * INCC notifications that include more than 1 item. */
            if (fixWindows && Device.RuntimePlatform == Device.WinPhone)
            {
                SplitCollectionChangedItems(e);
                return;
            }

            var maxindex = 0;
            if (e.NewStartingIndex >= 0 && e.NewItems != null)
                maxindex = Math.Max(maxindex, e.NewStartingIndex + e.NewItems.Count);
            if (e.OldStartingIndex >= 0 && e.OldItems != null)
                maxindex = Math.Max(maxindex, e.OldStartingIndex + e.OldItems.Count);
            if (maxindex > templatedObjects.Count)
                templatedObjects.InsertRange(templatedObjects.Count, Enumerable.Repeat<TItem>(null, maxindex - templatedObjects.Count));

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex >= 0)
                    {
                        for (int i = e.NewStartingIndex; i < templatedObjects.Count; i++)
                            SetIndex(templatedObjects[i], i + e.NewItems.Count);

                        templatedObjects.InsertRange(e.NewStartingIndex, Enumerable.Repeat<TItem>(null, e.NewItems.Count));

                        IList items = ConvertContent(e.NewStartingIndex, e.NewItems, true, true);
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, e.NewStartingIndex);
                    }
                    else
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.NewStartingIndex < 0 || e.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    bool movingForward = e.OldStartingIndex < e.NewStartingIndex;

                    if (movingForward)
                    {
                        int moveIndex = e.OldStartingIndex;
                        for (int i = moveIndex + e.OldItems.Count; i <= e.NewStartingIndex; i++)
                            SetIndex(templatedObjects[i], moveIndex++);
                    }
                    else
                    {
                        for (var i = 0; i < e.OldStartingIndex - e.NewStartingIndex; i++)
                        {
                            TItem item = templatedObjects[i + e.NewStartingIndex];
                            if (item != null)
                                SetIndex(item, GetIndex(item) + e.OldItems.Count);
                        }
                    }

                    TItem[] itemsToMove = templatedObjects.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToArray();

                    templatedObjects.RemoveRange(e.OldStartingIndex, e.OldItems.Count);
                    templatedObjects.InsertRange(e.NewStartingIndex, itemsToMove);
                    for (var i = 0; i < itemsToMove.Length; i++)
                        SetIndex(itemsToMove[i], e.NewStartingIndex + i);

                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, itemsToMove, e.NewStartingIndex, e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex >= 0)
                    {
                        int removeIndex = e.OldStartingIndex;
                        for (int i = removeIndex + e.OldItems.Count; i < templatedObjects.Count; i++)
                            SetIndex(templatedObjects[i], removeIndex++);

                        var items = new TItem[e.OldItems.Count];
                        for (var i = 0; i < items.Length; i++)
                        {
                            TItem item = templatedObjects[e.OldStartingIndex + i];
                            if (item == null)
                                continue;

                            UnhookItem(item);
                            items[i] = item;
                        }

                        templatedObjects.RemoveRange(e.OldStartingIndex, e.OldItems.Count);
                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items, e.OldStartingIndex);
                    }
                    else
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.NewStartingIndex >= 0)
                    {
                        IList oldItems = ConvertContent(e.NewStartingIndex, e.OldItems);
                        IList newItems = ConvertContent(e.NewStartingIndex, e.NewItems, true, true);

                        for (var i = 0; i < oldItems.Count; i++)
                        {
                            UnhookItem((TItem)oldItems[i]);
                        }

                        e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems, e.NewStartingIndex);
                    }
                    else
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    UnhookAndClear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnCollectionChanged(e);
        }

        static void SetIndex(TItem item, int index)
        {
            if (item == null)
                return;

            item.SetValue(IndexProperty, index);
        }

        void SplitCollectionChangedItems(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 0)
                        goto default;

                    for (var i = 0; i < e.NewItems.Count; i++)
                        OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems[i], e.NewStartingIndex + i), false);

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 0)
                        goto default;

                    for (var i = 0; i < e.OldItems.Count; i++)
                        OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems[i], e.OldStartingIndex + i), false);

                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 0)
                        goto default;

                    for (var i = 0; i < e.OldItems.Count; i++)
                        OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewItems[i], e.OldItems[i], e.OldStartingIndex + i), false);

                    break;

                default:
                    OnProxyCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset), false);
                    break;
            }
        }

        void UnhookAndClear()
        {
            for (var i = 0; i < templatedObjects.Count; i++)
            {
                TItem item = templatedObjects[i];
                if (item == null)
                    continue;

                UnhookItem(item);
            }

            templatedObjects.Clear();
        }

        void UnhookItem(TItem item)
        {
            SetIndex(item, -1);
            item.BindingContext = null;
        }
    }
}
