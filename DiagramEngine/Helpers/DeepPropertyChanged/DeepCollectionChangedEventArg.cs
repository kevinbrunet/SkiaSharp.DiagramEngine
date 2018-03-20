using System;
using System.Collections.Generic;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers.DeepPropertyChanged
{
    public class DeepCollectionChangedEventArg
    {
        public string PropertyName { get; set; }

        public List<object> AddedItems { get; set; }

        public List<object> RemoveItems { get; set; }


        public DeepCollectionChangedEventArg(string propertyName,ICollection<object> added,ICollection<object> removed)
        {
            this.PropertyName = propertyName;
            if(added != null && added.Count > 0)
            {
                AddedItems = new List<object>();
                AddedItems.AddRange(added);
            }

            if (removed != null && removed.Count > 0)
            {
                RemoveItems = new List<object>();
                RemoveItems.AddRange(removed);
            }
        }

        public DeepCollectionChangedEventArg(string propertyName, object added, object removed)
        {
            this.PropertyName = propertyName;
            if (added != null)
            {
                AddedItems = new List<object>();
                AddedItems.Add(added);
            }

            if (removed != null)
            {
                RemoveItems = new List<object>();
                RemoveItems.Add(removed);
            }
        }
    }



}
