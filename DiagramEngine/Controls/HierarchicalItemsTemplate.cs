using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers.DeepPropertyChanged;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    [ContentProperty("ItemTemplate")]
    public class HierarchicalItemsTemplate : Core.HierarchicalLayoutable
    {

        public static readonly BindableProperty PropsToIgnoreProperty = BindableProperty.Create(nameof(ItemsSourceSelector), typeof(string), typeof(HierarchicalItemsTemplate), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnPropsToIngoreChanged), null, null, null);
        public static readonly BindableProperty ItemsSourceSelectorProperty = BindableProperty.Create(nameof(ItemsSourceSelector), typeof(string), typeof(HierarchicalItemsTemplate), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourceSelectorChanged), null, null, null);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(HierarchicalItemsTemplate), null, BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate(OnItemTemplateChanged), null, null, null);

        private List<string> propsToIgnoreList;
        public string PropsToIgnore
        {
            get
            {
                return (string)base.GetValue(PropsToIgnoreProperty);
            }
            set
            {
                base.SetValue(PropsToIgnoreProperty, value);
            }
        }

        public string ItemsSourceSelector
        {
            get
            {
                return (string)base.GetValue(ItemsSourceSelectorProperty);
            }
            set
            {
                base.SetValue(ItemsSourceSelectorProperty, value);
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

        public HierarchicalItemsTemplate()
        {
        }

        private ChangeListener listener;
        private void Monitor()
        {
            if (listener != null)
            {
                listener.DeepCollectionChanged -= CollectionListener_DeepCollectionChanged;
                listener.Dispose();
                this.Children.Clear();
                listener = null;
            }
            if (this.BindingContext != null && string.IsNullOrEmpty(this.ItemsSourceSelector) == false)
            {
                listener = ChangeListener.Create(this.BindingContext, this.ItemsSourceSelector,this.propsToIgnoreList);
                listener.DeepCollectionChanged += CollectionListener_DeepCollectionChanged;
                listener.Subscribe();
            }
         }

        private void CollectionListener_DeepCollectionChanged(object sender, DeepCollectionChangedEventArg e)
        {
            if (e.PropertyName == this.ItemsSourceSelector)
            {
                if (e.RemoveItems != null)
                {
                    foreach (var remove in e.RemoveItems)
                    {
                        this.Children.Remove(this.Children.First(ie => ((HierarchicalNode)ie).Item == remove));
                    }
                }
                if (e.AddedItems != null)
                {
                    HierarchicalNode parentCtrl = (HierarchicalNode)this.Children.FirstOrDefault(ie => ((HierarchicalNode)ie).Item == sender);
                    int level = 0;
                    if (parentCtrl != null)
                    {
                        level = parentCtrl.Level + 1;
                    }
                    foreach (var add in e.AddedItems)
                    {
                        Core.Element template = null;
                        if(this.ItemTemplate != null)
                        {
                            template = (Core.Element)this.ItemTemplate.CreateContent();
                        }
                        var ctrl = new HierarchicalNode(add, level, template);
                        if (parentCtrl != null)
                            parentCtrl.ChildrenNode.Add(ctrl);
                        Children.Add(ctrl);
                    }
                }
            }
        }

        private static void OnItemsSourceSelectorChanged(BindableObject bo, object oldValue, object newValue)
        {
            var hit = (HierarchicalItemsTemplate)bo;
            hit.Monitor();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.Monitor();
        }

        private static void OnItemTemplateChanged(BindableObject bo, object oldValue, object newValue)
        {
            foreach(var child in ((HierarchicalItemsTemplate)bo).Children)
            {
                if(child is HierarchicalNode)
                {
                    ((HierarchicalNode)child).ItemTemplate = (Core.Element)(newValue == null ? null : ((DataTemplate)newValue).CreateContent());
                }
            }
        }

        private static void OnPropsToIngoreChanged(BindableObject bo, object oldValue, object newValue)
        {
            var hit = (HierarchicalItemsTemplate)bo;
            if (string.IsNullOrEmpty((string)newValue))
                hit.propsToIgnoreList = null;
            else
                hit.propsToIgnoreList = ((string)newValue).Split(',').ToList();
        }


    }
}
