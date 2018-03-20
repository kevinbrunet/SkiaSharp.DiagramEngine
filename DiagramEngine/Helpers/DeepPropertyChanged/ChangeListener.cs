using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers.DeepPropertyChanged
{
    /// <summary>
    /// Source form https://gist.github.com/thojaw/705450
    /// With modification for support add/remove
    /// </summary>
    public abstract class ChangeListener : INotifyPropertyChanged, IDisposable
    {
        protected List<string> propsToIgnore;

        protected string propertyName;

        public abstract void Subscribe();
        protected abstract void Unsubscribe();


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DeepCollectionChangedEventArg> DeepCollectionChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaiseDeepCollectionChanged(object sender, string propertyName, object added, object removed)
        {
            DeepCollectionChanged?.Invoke(sender, new DeepCollectionChangedEventArg(propertyName, added, removed));
        }
        protected virtual void RaiseDeepCollectionChanged(object sender,string propertyName,ICollection<object> added,ICollection<object> removed)
        {
            DeepCollectionChanged?.Invoke(sender, new DeepCollectionChangedEventArg(propertyName,added,removed));
        }

        protected virtual void RaiseDeepCollectionChanged(object sender, DeepCollectionChangedEventArg e)
        {
            DeepCollectionChanged?.Invoke(sender, e);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
            }
        }

        ~ChangeListener()
        {
            Dispose(false);
        }



        public static ChangeListener Create(object value,List<string> propsToIgnore = null)
        {
            return Create(value, null, propsToIgnore);
        }

        public static ChangeListener Create(object value, string propertyName, List<string> propsToIgnore = null)
        {
            if (value is INotifyCollectionChanged)
            {
                return new CollectionChangeListener(null,value as INotifyCollectionChanged, propertyName, propsToIgnore);
            }
            else if (value is INotifyPropertyChanged)
            {
                return new ChildChangeListener(value as INotifyPropertyChanged, propertyName, propsToIgnore);
            }
            else
                return null;
        }
    }
}
