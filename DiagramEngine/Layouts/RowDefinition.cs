using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Layouts
{
    public sealed class RowDefinition : BindableObject, IDefinition
    {
        
    public static readonly BindableProperty HeightProperty = BindableProperty.Create("Height", typeof(GridLength), typeof(RowDefinition), new GridLength(1.0, GridUnitType.Star), BindingMode.OneWay, null, new BindableProperty.BindingPropertyChangedDelegate((bindable, oldValue, newValue) => ((RowDefinition)bindable).OnSizeChanged()), null, null, null);

    public event EventHandler SizeChanged;

    public GridLength Height
    {
        get
        {
            return (GridLength)base.GetValue(RowDefinition.HeightProperty);
        }
        set
        {
            base.SetValue(RowDefinition.HeightProperty, value);
        }
    }

    internal double ActualHeight
    {
        get;
        set;
    }

    internal double MinimumHeight
    {
        get;
        set;
    }

    public RowDefinition()
    {
        this.MinimumHeight = -1.0f;
    }

    private void OnSizeChanged()
    {
            this.SizeChanged?.Invoke(this, EventArgs.Empty);
        }
}
}
