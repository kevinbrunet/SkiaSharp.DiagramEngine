using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using ModelerClient.DiagramEngine.Core;
using ModelerClient.DiagramEngine.Helpers;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Controls
{
    [ContentProperty("ItemTemplate")]
    public class ItemsTemplate : Core.Layoutable, IItemsTemplate
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ItemsTemplate), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(ItemsTemplate.OnItemsSourceChanged), null, null, null);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ItemsTemplate), null, BindingMode.OneWay, null,new BindableProperty.BindingPropertyChangedDelegate(ItemsTemplate.ItemTemplateChanged), null, null, null);



        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)base.GetValue(ItemsSourceProperty);
            }
            set
            {
                base.SetValue(ItemsSourceProperty, value);
            }
        }

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(ItemTemplateProperty);
            }
            set
            {
                base.SetValue(ItemTemplateProperty, value);
            }
        }

        public TemplatedItemsList<ItemsTemplate, Core.Element> TemplatedItems
        {
            get;
            private set;
        }

        public ItemsTemplate()
        {
            this.TemplatedItems = new TemplatedItemsList<ItemsTemplate, Core.Element>(this);
            this.TemplatedItems.CollectionChanged += TemplatedItems_CollectionChanged;
        }

        protected virtual void TemplatedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                this.Children.Clear();
                foreach (var item in this.TemplatedItems)
                {
                    this.Children.Add(item);
                }
            }
            else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                this.Children.Move(e.OldStartingIndex, e.NewStartingIndex);
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        this.Children.Remove((Core.Element)oldItem);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var newItem in e.NewItems)
                    {
                        this.Children.Add((Core.Element)newItem);
                    }
                }
            }
        }

        private static void OnItemsSourceChanged(BindableObject bo, object oldValue, object newValue)
        {
            ((ItemsTemplate)bo).TemplatedItems.ItemsSource = (IEnumerable)newValue;
        }


        private static void ItemTemplateChanged(BindableObject bo, object oldValue, object newValue)
        {
            ((ItemsTemplate)bo).TemplatedItems.ItemTemplate = (DataTemplate)newValue;
        }

        public IVisual CreateDefault(object item)
        {
            string text = null;
            if (item != null)
            {
                text = item.ToString();
            }
            return new TextBlock
            {
                Text = text
            };
        }

        public void SetupContent(object item, int index)
        {
            //throw new NotImplementedException();
        }
    }
}
