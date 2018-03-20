using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    public class HierarchicalNode : DiagramEngine.Core.Element
    {

        private object item;
        private int level;
        public List<HierarchicalNode> ChildrenNode = new List<HierarchicalNode>();

        public HierarchicalNode(object item, int level, Abstracts.IElement itemTemplate)
        {
            this.item = item;
            this.Level = level;
            this.ItemTemplate = itemTemplate;
            this.Bag = new ExpandoObject();
        }

        public object Item { get => item; set => item = value; }
        public int Level { get => level; set => level = value; }

        public dynamic Bag { get; private set; }

        private Abstracts.IElement itemTemplate;
        public Abstracts.IElement ItemTemplate
        {
            get
            {
                return itemTemplate;
            }
            set
            {
                itemTemplate = value;
                this.Children.Clear();

                if (itemTemplate == null)
                {
                    itemTemplate = CreateDefault(this.item);
                }
                this.Children.Add(itemTemplate);
                var bo = itemTemplate as BindableObject;
                if (bo != null)
                {
                    bo.BindingContext = this.item;
                }
            }
        }


        public IElement CreateDefault(object item)
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

    }

}
