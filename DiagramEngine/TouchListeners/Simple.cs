using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace ModelerClient.DiagramEngine.TouchListeners
{
    public class Simple : Base
    {
        public event EventHandler<SKTouchEventArgs> OnTunnelingTouchEvent;
        public event EventHandler<SKTouchEventArgs> OnBubblingTouchEvent;
        public event EventHandler<SKTouchEventArgs> OnTouchEvent;

        public override IElement OnTouch(IElement rootElement, SKTouchEventArgs e)
        {
            var elem = rootElement.GetElementAtPoint(e.Location);
            if(elem != null)
            {
                return elem;
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
    }
}
