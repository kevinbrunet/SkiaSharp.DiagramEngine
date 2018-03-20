using System;
using System.Linq;
using System.Collections.Generic;
using SkiaSharp;
using Xamarin.Forms;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SkiaSharp.Views.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Core
{
    [ContentProperty("Children")]
    public class Element : Visual, IElement
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(SKRect), typeof(Element), SKRect.Empty, propertyChanged: Visual.InvalidatePropertyChanged);

        public Element()
        {
            Children = new ElementsCollection(this);
        }

        #region Properties

        private IElement parent;
        public IElement Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if(parent != value)
                {
                    if(parent !=null)
                    {
                        parent.Children.Remove(this);
                    }
                    AttachParent(value);
                    if (parent != null)
                    {
                        parent.Children.Add(this);
                    }
                }
            }
        }

        public ElementsCollection Children { get; private set; }

        [TypeConverter(typeof(SKThicknessTypeConverter))]
        public SKRect Padding
        {
            get
            {
                return (SKRect)GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        #endregion


        #region Measure
        public override void Measure(SKSize availableSize)
        {
            SKSize desiredSize = this.DesiredSize;
            bool sameConstraint = FloatUtil.AreClose(availableSize, this.previousConstraint);
            base.Measure(availableSize);
            if( this.Parent != null && 
                this.Visibility && 
                (!((this.IsMeasureValid && !NeverMeasured) & sameConstraint)) && 
                !this.MeasureDuringArrange && 
                !FloatUtil.AreClose(DesiredSize, desiredSize))
            {
                this.Parent.OnChildDesiredSizeChanged(this);
            }
        }

        /// <summary>Prend en charge les comportements de disposition lorsqu'un élément enfant est redimensionné. </summary>
        /// <param name="child">Élément enfant faisant l'objet du redimensionnement.</param>
        public virtual void OnChildDesiredSizeChanged(Element child)
        {
            if (!this.MeasureInProgress)
                this.InvalidateMeasure();
        }

        protected override SKSize MeasureOverride(SKSize availableSize)
        {
            if (Children.Count == 0)
            {
                var contentSize = MeasureContentWithSizeConstraint(availableSize);
                return new SKSize(contentSize.Width + this.Margin.Right + this.Padding.Right, contentSize.Height + this.Margin.Bottom + this.Padding.Bottom);
            }
            else
            {
                var contentSize = MeasureContentWithSizeConstraint(availableSize);
                var childrenSize = MeasureChildren(availableSize);

                float width;
                float height;

                if (float.IsNaN(this.Width) == false && float.IsInfinity(this.Width) == false)
                    width = this.Width;
                else
                    width = Math.Max(contentSize.Width, childrenSize.Width) + this.Margin.Right + this.Padding.Right;

                if (float.IsNaN(this.Height) == false && float.IsInfinity(this.Height) == false)
                    height = this.Height;
                else
                    height = Math.Max(contentSize.Height, childrenSize.Height) + this.Margin.Bottom + this.Padding.Bottom;


                return new SKSize(width , height);
            }
        }

        protected virtual SKSize MeasureChildren(SKSize availableSize)
        {
            float width = 0.0f;
            float height = 0.0f;
            foreach (var element in Children)
            {
                element.Measure(availableSize);


                if (width < element.DesiredSize.Width + element.X)
                {
                    width = element.DesiredSize.Width + element.X;
                }

                if (height < element.DesiredSize.Height + element.Y)
                {
                    height = element.DesiredSize.Height + element.Y;
                }
            }

            return new SKSize(width, height);
        }

        protected virtual SKSize MeasureContentWithSizeConstraint(SKSize availableSize)
        {
            var ret = MeasureContent(availableSize);
            bool widthDefined = float.IsNaN(this.Width) == false && float.IsInfinity(this.Width) == false;
            bool heightDefined = float.IsNaN(this.Height) == false && float.IsInfinity(this.Height) == false;
            if (widthDefined)
            {
                ret.Width = this.Width;
            }
            else if (!widthDefined && heightDefined)
            {
                ret.Width = this.Height;
            }

            if (heightDefined)
            {
                ret.Height = this.Height;
            }
            else if (!heightDefined && widthDefined)
            {
                ret.Height = this.Width;
            }
            return ret;
        }

        protected virtual SKSize MeasureContent(SKSize availableSize)
        {
            return SKSize.Empty;
        }
        #endregion

        #region Arrange
        protected SKRect _childrenBounds;
        protected SKMatrix _childrenVisualTransform;

        protected override void ArrangeCore(SKRect finalRect)
        {
            base.ArrangeCore(finalRect);
            //on construit le rectangle des fils
            _childrenBounds = new SKRect(0, 0, boundsMinusMargin.Width - Padding.Right, boundsMinusMargin.Height - Padding.Bottom);
            //on construit la transfromation
            _childrenVisualTransform = VisualTransform.Translate(Padding.Left, Padding.Top);
            ArrangeChildren(_childrenBounds);

        }

        protected virtual void ArrangeChildren(SKRect childrenBounds)
        {
            foreach (var element in Children)
            {
                var X = childrenBounds.Left + element.X;
                var Y = childrenBounds.Top + element.Y;
                var right = Math.Min(X+ element.DesiredSize.Width,childrenBounds.Right);
                if(float.IsInfinity(element.Width) == false && float.IsNaN(element.Width) == false)
                {
                    right = X + element.Width;
                }

                var bottom = Math.Min(Y+ element.DesiredSize.Height,childrenBounds.Bottom);
                if (float.IsInfinity(element.Height) == false && float.IsNaN(element.Height) == false)
                {
                    bottom = Y+element.Height;
                }
                var rect = new SKRect(X,Y, right, bottom);
                element.Arrange(rect);
            }
        }
        #endregion

        #region Render
        public override void Render(SKCanvas canvas)
        {
            base.Render(canvas);
            SKMatrix visual = _childrenVisualTransform;
            SKMatrix invert = visual.Invert();
            canvas.Concat(ref visual);
            RenderChildren(canvas);
            canvas.Concat(ref invert);
        }

        protected virtual void RenderChildren(SKCanvas canvas)
        {
            foreach (var child in Children)
            {
                child.Render(canvas);
            }
        }

        #endregion

        public virtual void AttachParent(IElement parent)
        {
            this.parent = parent;
        }

        public override void Invalidate()
        {
            base.Invalidate();
            Parent?.Invalidate();
        }

        public virtual IElement GetElementAtPoint(SKPoint point)
        {
            return GetElementAtPoint(point, null, this._childrenVisualTransform);
        }

        public virtual IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate)
        {
            return GetElementAtPoint(point, predicate, this._childrenVisualTransform);
        }

        public virtual IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate, SKMatrix transformStack)
        {
            IList<IElement> items;
            if (predicate != null)
            {
                items = Children.Where(predicate).ToList();
            }
            else
            {
                items = Children;
            }
            var childrenStack = transformStack.Concat(this._childrenVisualTransform);
            for (int i=items.Count -1;i>=0;i--)
            {
                var element = items[i];
                if (element.IsPointInside(point, childrenStack))
                {
                    var subElement = element.GetElementAtPoint(point, predicate, childrenStack);
                    if (subElement != null)
                    {
                        return subElement;
                    }
                    else
                    {
                        return element;
                    }
                }
            }
            return null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach (var child in Children)
            {
                var bChild = child as BindableObject;
                if (bChild != null)
                {
                    SetInheritedBindingContext(bChild, this.BindingContext);
                }
            }
        }
    }



}
