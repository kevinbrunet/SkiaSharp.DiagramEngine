using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers.DeepPropertyChanged
{
    /// <summary>
    /// Source form https://gist.github.com/thojaw/705450#file-collectionchangelistener-cs
    /// </summary>
    public class CollectionChangeListener : ChangeListener
    {
        #region *** Members ***
        private readonly object _parent;
        private readonly INotifyCollectionChanged _value;
        private readonly Dictionary<INotifyPropertyChanged, ChangeListener> _collectionListeners = new Dictionary<INotifyPropertyChanged, ChangeListener>();
        #endregion


        #region *** Constructors ***
        public CollectionChangeListener(object parent,INotifyCollectionChanged collection, string propertyName,List<string> propsToIgnore)
        {
            _parent = parent;
            _value = collection;
            base.propertyName = propertyName;
            base.propsToIgnore = propsToIgnore;

        }
        #endregion


        #region *** Private Methods ***
        public override void Subscribe()
        {
            _value.CollectionChanged += new NotifyCollectionChangedEventHandler(value_CollectionChanged);

            foreach (INotifyPropertyChanged item in (IEnumerable)_value)
            {
                ResetChildListener(item);
            }
        }

        private void ResetChildListener(INotifyPropertyChanged item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            RemoveItem(item);

            ChangeListener listener = null;

            // Add new
            if (item is INotifyCollectionChanged)
                listener = new CollectionChangeListener(_value, item as INotifyCollectionChanged, propertyName,base.propsToIgnore);
            else
                listener = new ChildChangeListener(item as INotifyPropertyChanged,base.propsToIgnore);

            listener.PropertyChanged += listener_PropertyChanged;
            listener.DeepCollectionChanged += Listener_DeepCollectionChanged;
            _collectionListeners.Add(item, listener);
            RaiseDeepCollectionChanged(_parent, propertyName, item, null);
            listener.Subscribe();
        }

        private void Listener_DeepCollectionChanged(object sender, DeepCollectionChangedEventArg e)
        {
            RaiseDeepCollectionChanged(sender, e);
        }

        private void RemoveItem(INotifyPropertyChanged item)
        {
            // Remove old
            if (_collectionListeners.ContainsKey(item))
            {
                _collectionListeners[item].PropertyChanged -= listener_PropertyChanged;
                _collectionListeners[item].DeepCollectionChanged -= Listener_DeepCollectionChanged;

                _collectionListeners[item].Dispose();
                _collectionListeners.Remove(item);
                RaiseDeepCollectionChanged(_parent, propertyName, null, item);
            }
        }

        private void ClearCollection()
        {
            RaiseDeepCollectionChanged(_parent, propertyName, null, _collectionListeners.Keys.ToList<object>());
            foreach (var key in _collectionListeners.Keys)
            {
                _collectionListeners[key].Dispose();
            }
            _collectionListeners.Clear();
        }
        #endregion


        #region *** Event handlers ***
        void value_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                ClearCollection();
            }
            else
            {
                // Don't care about e.Action, if there are old items, Remove them...
                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged item in (IEnumerable)e.OldItems)
                        RemoveItem(item);
                }

                // ...add new items as well
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in (IEnumerable)e.NewItems)
                        ResetChildListener(item);
                }
            }
        }


        void listener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // ...then, notify about it
            RaisePropertyChanged(string.Format("{0}{1}{2}",
                propertyName, propertyName != null ? "[]." : null, e.PropertyName));
        }
        #endregion


        #region *** Overrides ***
        /// <summary>
        /// Releases all collection item handlers and self handler
        /// </summary>
        protected override void Unsubscribe()
        {
            ClearCollection();

            _value.CollectionChanged -= new NotifyCollectionChangedEventHandler(value_CollectionChanged);

            System.Diagnostics.Debug.WriteLine("CollectionChangeListener unsubscribed");
        }
        #endregion
    }
}
