using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace ModelerClient.DiagramEngine.TouchListeners
{
    public class Default : Base
    {
        public event EventHandler<SKTouchEventArgs> OnTunnelingTouchEvent;
        public event EventHandler<SKTouchEventArgs> OnBubblingTouchEvent;
        public event EventHandler<SKTouchEventArgs> OnTouchEvent;

        public override IElement OnTouch(IElement rootElement, SKTouchEventArgs e)
        {
            SKMatrix transformStack = rootElement.VisualTransform;
            Stack<IElement> touchedElements = new Stack<IElement>();
            touchedElements.Push(rootElement);
            bool toDeep = true;
            while (touchedElements.Any() || e.Handled)
            {
                bool savedToDeep = toDeep;
                if (toDeep)
                {
                    toDeep = false;
                    var lastElement = touchedElements.Peek();
                    for (int i = lastElement.Children.Count - 1; i >= 0; i--)
                    {
                        var element = lastElement.Children[i];
                        if (element.IsPointInside(e.Location, transformStack))
                        {
                            OnTunnelingTouchElement(lastElement,e);
                            if (e.Handled)
                                return lastElement;
                            touchedElements.Push(element);
                            toDeep = true;
                            break;
                        }
                    }
                    if (e.Handled == false && !toDeep)
                    {
                        OnTouchElement(lastElement,e);
                        if (e.Handled)
                            return lastElement;
                    }
                }
                else
                {
                    var lastElement = touchedElements.Pop();
                    if (touchedElements.Count > 0)
                    {
                        var parentElement = touchedElements.Peek();
                        OnBubblingTouchElement(lastElement,e);
                        if (e.Handled)
                            return lastElement;
                        for (int i = parentElement.Children.IndexOf(lastElement) - 1; i >= 0; i--)
                        {
                            var element = parentElement.Children[i];
                            if (element.IsPointInside(e.Location, transformStack))
                            {
                                OnTunnelingTouchElement(lastElement,e);
                                if (e.Handled)
                                    return lastElement;
                                touchedElements.Push(element);
                                toDeep = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (Next != null)
            {
                return Next.OnTouch(rootElement, e);
            }
            else
            {
                return base.OnTouch(rootElement, e);
            }
        }

        protected virtual void OnBubblingTouchElement(IElement element,SKTouchEventArgs e)
        {
            OnBubblingTouchEvent?.Invoke(element, e);
        }

        protected virtual void OnTouchElement(IElement element,SKTouchEventArgs e)
        {
            OnTouchEvent?.Invoke(element, e);
        }

        protected virtual void OnTunnelingTouchElement(IElement element,SKTouchEventArgs e)
        {
            OnTunnelingTouchEvent?.Invoke(element, e);
        }
    }
}
