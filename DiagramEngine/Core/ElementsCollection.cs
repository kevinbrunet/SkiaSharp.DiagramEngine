using ModelerClient.DiagramEngine.Abstracts;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ModelerClient.DiagramEngine.Core
{
    public class ElementsCollection : ObservableCollection<IElement>
    {
        private readonly IElement container;
        public ElementsCollection()
        {
        }

        internal ElementsCollection(IElement container)
        {
            this.container = container;
        }

        protected override void ClearItems()
        {
            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if(e.OldItems != null)
            {
                foreach(var oldItem in e.OldItems)
                {
                    ((IElement)oldItem).AttachParent(null);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    ((IElement)newItem).AttachParent(container);
                }
            }
            this.Invalidate();
        }

        private void Invalidate()
        {
            container?.Invalidate();
        }

       
    }
}
