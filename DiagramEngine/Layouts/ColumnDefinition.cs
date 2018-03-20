using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Layouts
{
    public sealed class ColumnDefinition : BindableObject, IDefinition
    {
        
        /// <summary>Backing store for the <see cref="P:Xamarin.Forms.ColumnDefinition.Width" /> property.</summary>
        /// <remarks>To be added.</remarks>
        public static readonly BindableProperty WidthProperty = BindableProperty.Create("Width", typeof(GridLength), typeof(ColumnDefinition), new GridLength(1.0, GridUnitType.Star), BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate((bindable,oldValue,newValue)=>((ColumnDefinition)bindable).OnSizeChanged()), null, null, null);

        /// <summary>Event that is raised when the size of the column is changed.</summary>
        /// <remarks>To be added.</remarks>
        public event EventHandler SizeChanged;

        /// <summary>Gets or sets the width of the column.</summary>
        /// <value>To be added.</value>
        /// <remarks>To be added.</remarks>
        public GridLength Width
        {
            get
            {
                return (GridLength)base.GetValue(WidthProperty);
            }
            set
            {
                base.SetValue(WidthProperty, value);
            }
        }

        internal double ActualWidth
        {
            get;
            set;
        }

        internal double MinimumWidth
        {
            get;
            set;
        }

        public ColumnDefinition()
        {
            this.MinimumWidth = -1.0f;
        }

        private void OnSizeChanged()
        {
            this.SizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
