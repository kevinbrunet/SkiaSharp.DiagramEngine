using System;
using System.Collections.Generic;
using System.Text;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.TouchListeners
{
    public class Draggable : Base
    {
        public static double MINIMUM_HORIZONTAL_DRAG_DISTANCE = 10;
        public static double MINIMUM_VERTICAL_DRAG_DISTANCE = 10;


        private IElement pressedElement = null;
        private IElement dragElement = null;
        private SKPoint startPoint = SKPoint.Empty;
        private float startX;
        private float startY;




        public override IElement OnTouch(IElement rootElement, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Moved)
            {
                if (pressedElement != null && dragElement == null)
                {
                    var dist = e.Location - startPoint;
                    if (Math.Abs(dist.X) > MINIMUM_HORIZONTAL_DRAG_DISTANCE || Math.Abs(dist.Y) > MINIMUM_VERTICAL_DRAG_DISTANCE)
                    {
                        //drag started
                        e.Handled = true;
                        dragElement = pressedElement;
                        return dragElement;
                    }
                }
                else if (dragElement != null)
                {
                    e.Handled = true;
                    var dist = e.Location - startPoint;
                    //drag move
                    dragElement.X = startX + dist.X;
                    dragElement.Y = startY + dist.Y;
                    return dragElement;
                }
            }
            else if (e.ActionType == SKTouchAction.Released && dragElement != null)
            {
                e.Handled = true;
                var ret = dragElement;
                //drop
                dragElement = null;
                startPoint = SKPoint.Empty;
                pressedElement = null;
                startX = 0;
                startY = 0;
                return ret;
            }
            else if (e.ActionType == SKTouchAction.Cancelled && dragElement != null)
            {
                //drag canceled
                e.Handled = true;
                var ret = dragElement;
                dragElement.X = startX;
                dragElement.Y = startY;
                dragElement = null;
                startPoint = SKPoint.Empty;
                pressedElement = null;
                startX = 0;
                startY = 0;
                return ret;
            }
            IElement elem = null;
            if(Next != null)
            {
                elem = Next.OnTouch(rootElement, e);
            }
            if (e.ActionType == SKTouchAction.Pressed && elem != null)
            {
                pressedElement = elem;
                startX = pressedElement.X;
                startY = pressedElement.Y;
                startPoint = e.Location;
            }
            return elem;
        }

    }
}
