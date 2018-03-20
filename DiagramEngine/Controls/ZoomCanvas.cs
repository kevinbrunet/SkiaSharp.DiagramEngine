using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Core;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Controls
{
    public class ZoomCanvas : Canvas
    {

        private bool gestureActivated = false;

        private SKMatrix lastTranslateTransformation = SKMatrix.MakeIdentity();
        private SKMatrix lastScaleTransformation = SKMatrix.MakeIdentity();
        private SKMatrix translateTransformation = SKMatrix.MakeIdentity();
        private SKMatrix scaleTransformation = SKMatrix.MakeIdentity();
        private readonly PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        private readonly PanGestureRecognizer panGesture = new PanGestureRecognizer();


        protected readonly Core.Layoutable overlaySurface = new Core.Layoutable();
        public ElementsCollection Overlay
        {
            get
            {
                return overlaySurface.Children;
            }
        }

        public ICommand ZoomCommand { get; private set; }
        public ICommand IncrementalZoomCommand { get; private set; }

        public ZoomCanvas()
        {
            overlaySurface.TransformationPivot = SKPoint.Empty;
            overlaySurface.AttachParent(this);
            this.GestureRecognizers.Add(panGesture);
            panGesture.PanUpdated += PanGesture_PanUpdated;

            this.GestureRecognizers.Add(pinchGesture);
            pinchGesture.PinchUpdated += OnPinchUpdated;
            ZoomCommand = new Command(o =>
           {
               float scaleFactor = 0;
               if (o is string)
               {
                   scaleFactor = float.Parse((string)o, CultureInfo.InvariantCulture);
               }
               else
               {
                   scaleFactor = Convert.ToSingle(o);
               }
               scaleTransformation = SKMatrix.MakeScale((float)scaleFactor, (float)scaleFactor);
               surface.Transformation = translateTransformation.Concat(scaleTransformation);
           });
            IncrementalZoomCommand = new Command(o =>
           {
               float scaleFactor = 0;
               if (o is string)
               {
                   scaleFactor = float.Parse((string)o,CultureInfo.InvariantCulture);
               }
               else
               {
                   scaleFactor = Convert.ToSingle(o);
               }
               scaleTransformation = scaleTransformation
                   .Scale(scaleFactor, scaleFactor);
               surface.Transformation = translateTransformation.Concat(scaleTransformation);
           });
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Started)
            {
                lastTranslateTransformation = translateTransformation;
                gestureActivated = true;
            }
            else if (e.StatusType == GestureStatus.Canceled)
            {
                translateTransformation = lastTranslateTransformation;
                gestureActivated = false;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                lastTranslateTransformation = translateTransformation;
                gestureActivated = false;
            }
            else
            {
                translateTransformation = lastTranslateTransformation.Translate((float)e.TotalX, (float)e.TotalY);
                gestureActivated = true;
            }
            surface.Transformation = translateTransformation.Concat(scaleTransformation);
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                lastScaleTransformation = scaleTransformation;
                gestureActivated = true;
            }
            else if (e.Status == GestureStatus.Canceled)
            {
                scaleTransformation = lastScaleTransformation;
                gestureActivated = false;
            }
            else if (e.Status == GestureStatus.Completed)
            {
                lastScaleTransformation = scaleTransformation;
                gestureActivated = false;
            }
            else
            {
                scaleTransformation = scaleTransformation//.Translate(-(float)e.ScaleOrigin.X,-(float)e.ScaleOrigin.Y)
                    .Scale((float)e.Scale, (float)e.Scale);
                gestureActivated = false;
            }
            surface.Transformation = translateTransformation.Concat(scaleTransformation);
        }


        protected override void OnTouch(SKTouchEventArgs e)
        {
            if (TouchListener != null && gestureActivated == false)
            {
                var elem = TouchListener.OnTouch(overlaySurface, e);
                if (elem == null)
                {
                    TouchListener.OnTouch(surface, e);
                }
            }
        }

        public override void Measure(SKSize availableSize)
        {
            base.Measure(availableSize);
            overlaySurface.Measure(availableSize);
        }

        public override void Arrange(SKRect finalRect)
        {
            base.Arrange(finalRect);
            overlaySurface.Arrange(finalRect);
        }

        public override void Render(SKCanvas canvas)
        {
            base.Render(canvas);
            overlaySurface.Render(canvas);
        }

        public override IElement GetElementAtPoint(SKPoint point)
        {
            return overlaySurface.GetElementAtPoint(point) ?? base.GetElementAtPoint(point);
        }

        public override IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate)
        {
            return overlaySurface.GetElementAtPoint(point,predicate) ?? base.GetElementAtPoint(point,predicate);
        }

        public override IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate, SKMatrix transformationsStack)
        {
            return overlaySurface.GetElementAtPoint(point,predicate,transformationsStack) ?? base.GetElementAtPoint(point, predicate, transformationsStack);
        }

    }
}
