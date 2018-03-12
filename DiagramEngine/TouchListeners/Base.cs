using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.TouchListeners
{
    [ContentProperty("Next")]
    public class Base : BindableObject, ITouchListener
    {
        public static readonly BindableProperty NextProperty = BindableProperty.Create(nameof(Next), typeof(ITouchListener), typeof(Base), null);


        public ITouchListener Next { get => (ITouchListener)this.GetValue(NextProperty); set => SetValue(NextProperty, value); }

        public virtual IElement OnTouch(IElement rootElement, SKTouchEventArgs e)
        {
            if (Next != null)
                return Next.OnTouch(rootElement, e);
            else
                return null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(Next != null)
            BindableObject.SetInheritedBindingContext((BindableObject)Next, this.BindingContext);
        }
    }
}
