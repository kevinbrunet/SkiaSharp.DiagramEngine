using SkiaSharp;
using Xamarin.Forms;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Helpers;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Core
{
    public abstract class Visual : BindableObject, IVisual
    {
        public static readonly BindableProperty MarginProperty = BindableProperty.Create(nameof(Margin),typeof(SKRect),typeof(Visual), SKRect.Empty, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty VisibilityProperty = BindableProperty.Create(nameof(Visibility), typeof(bool), typeof(Visual), true, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty XProperty = BindableProperty.Create(nameof(X), typeof(float), typeof(Visual), 0.0f, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty YProperty = BindableProperty.Create(nameof(Y), typeof(float), typeof(Visual), 0.0f, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(float), typeof(Visual), float.NaN, propertyChanged: InvalidatePropertyChanged);
        public static readonly BindableProperty HeightProperty = BindableProperty.Create(nameof(Height), typeof(float), typeof(Visual), float.NaN, propertyChanged: InvalidatePropertyChanged);

        public static readonly BindableProperty TransformationProperty = BindableProperty.Create(nameof(Visual.Transformation), typeof(SKMatrix?), typeof(Visual), null, propertyChanged: TransformationPropertyChanged);
        public static readonly BindableProperty TransformationPivotProperty = BindableProperty.Create(nameof(TransformationPivot), typeof(SKPoint), typeof(Visual), new SKPoint(0.5f,0.5f), propertyChanged: TransformationPropertyChanged);


        public static void InvalidatePropertyChanged(BindableObject bo,object oldValue,object newValue)
        {
            ((Visual)bo).Invalidate();
        }

        public static void TransformationPropertyChanged(BindableObject bo, object oldValue, object newValue)
        {
            ((Visual)bo).TransformationDirty = true;
            ((Visual)bo).Invalidate();
        }

        public Visual()
        {
        }

        #region Properties


        [TypeConverter(typeof(SKThicknessTypeConverter))]
        public SKRect Margin
        {
            get
            {
                return (SKRect)GetValue(MarginProperty);
            }
            set
            {
                SetValue(MarginProperty, value);
            }
        }

        public SKMatrix? Transformation
        {
            get
            {
                return (SKMatrix?)GetValue(TransformationProperty);
            }
            set
            {
                SetValue(TransformationProperty, value);
            }
        }

        public SKPoint TransformationPivot
        {
            get
            {
                return (SKPoint)GetValue(TransformationPivotProperty);
            }
            set
            {
                SetValue(TransformationPivotProperty, value);
            }
        }

        public bool Visibility
        {
            get
            {
                return (bool)GetValue(VisibilityProperty);
            }
            set
            {
                SetValue(VisibilityProperty, value);
            }
        }

        public float X
        {
            get
            {
                return (float)GetValue(XProperty);
            }
            set
            {
                SetValue(XProperty, value);
            }
        }

        public float Y
        {
            get
            {
                return (float)GetValue(YProperty);
            }
            set
            {
                SetValue(YProperty, value);
            }
        }

        public float Width
        {
            get
            {
                return (float)GetValue(WidthProperty);
            }
            set
            {
                SetValue(WidthProperty, value);
            }
        }

        public float Height
        {
            get
            {
                return (float)GetValue(HeightProperty);
            }
            set
            {
                SetValue(HeightProperty, value);
            }
        }

        #endregion

        #region Measure

        private SKSize desiredSize;
        public SKSize DesiredSize
        {
            get
            {
                if (this.Visibility == false)
                {
                    return new SKSize(0.0f, 0.0f);
                }
                return this.desiredSize;
            }
        }

        private SKSize previousAvailableSize;
        protected SKSize previousConstraint
        {
            get
            {
                return this.previousAvailableSize;
            }
        }

        internal bool MeasureDirty;

        /// <summary>Obtient une valeur indiquant si les dimensions actuelles retournées par la mesure de la disposition sont valides. </summary>
        /// <returns>true si la passe de mesure de disposition a retourné une valeur valide actuelle ; sinon, false.</returns>
        public bool IsMeasureValid
        {
            get
            {
                return !this.MeasureDirty;
            }
        }

        internal bool MeasureDuringArrange;

        public bool MeasureInProgress { get; protected set; }

        internal bool NeverMeasured;


        /// <summary>Invalide l'état de mesure (disposition) de l'élément. </summary>
        public void InvalidateMeasure()
        {
            if (!this.MeasureDirty && !this.MeasureInProgress)
            {
                this.MeasureDirty = true;
            }
        }

        /// <summary>Met à jour la propriété <see cref="P:System.Windows.UIElement.DesiredSize" /> de la classe <see cref="T:System.Windows.UIElement" />.Les éléments parents appellent cette méthode depuis leur propre implémentation <see cref="M:System.Windows.UIElement.MeasureCore(System.Windows.Size)" /> pour former une actualisation récursive de la disposition.L'appel à cette méthode constitue la première passe (la passe de "Measure") d'une actualisation de disposition.</summary>
        /// <param name="availableSize">Espace disponible qu'un élément parent peut allouer à un élément enfant.Un élément enfant peut demander un espace plus grand que ce qui est disponible ; les dimensions fournies peuvent être adaptées si le défilement est possible dans le modèle de contenu pour l'élément actif.</param>
        public virtual void Measure(SKSize availableSize)
        {
            bool sameConstraint = FloatUtil.AreClose(availableSize, this.previousAvailableSize);
            if (this.Visibility == false)
            {
                if (!sameConstraint)
                {
                    this.MeasureDirty = true;
                    this.previousAvailableSize = availableSize;
                }
            }
            else if (!((this.IsMeasureValid && !NeverMeasured) & sameConstraint))
            {
                this.NeverMeasured = false;
                this.InvalidateArrange();
                this.MeasureInProgress = true;
                SKSize size = new SKSize(0.0f, 0.0f);
                try
                {
                    size = this.MeasureOverride(availableSize);
                }
                finally
                {
                    this.MeasureInProgress = false;
                    this.previousAvailableSize = availableSize;
                }
                this.MeasureDirty = false;
                this.desiredSize = size;
            }

        }

        /// <summary>En cas de substitution dans une classe dérivée, fournit le code de mesure afin de dimensionner cet élément correctement, en considérant les dimensions de tout contenu d'élément enfant. </summary>
        /// <returns>Dimensions désirées de cet élément dans la disposition.</returns>
        /// <param name="availableSize">Dimensions disponibles que l'élément parent peut allouer à l'enfant.</param>
        protected virtual SKSize MeasureOverride(SKSize availableSize)
        {
            SKSize ret = new SKSize();
            if (float.IsNaN(this.Width) == false && float.IsInfinity(this.Width) == false)
                ret.Width = this.Width;
            else
                ret.Width = 0;

            if (float.IsNaN(this.Height) == false && float.IsInfinity(this.Height) == false)
                ret.Height = this.Height;
            else
                ret.Height = 0;
            //on inflate avec la margin
            ret.Width += this.Margin.Right;
            ret.Height += this.Margin.Bottom;
            return ret;
        }


        #endregion

        #region Arrange

        protected SKRect boundsMinusMargin;


        private SKRect finalRect;
        protected SKRect previousArrangeRect
        {
            get
            {
                return this.finalRect;
            }
        }

        public SKRect Bounds
        {
            get
            {
                return this.finalRect;
            }
        }

        public SKMatrix VisualTransform { get; private set; }



        internal bool ArrangeDirty;

        /// <summary>Obtient une valeur indiquant si les dimensions et la position calculées d'éléments enfants dans la disposition de cet élément sont valides. </summary>
        /// <returns>true si les dimensions et la position de disposition sont valides ; sinon, false.</returns>
        public bool IsArrangeValid
        {
            get
            {
                return !this.ArrangeDirty;
            }
        }

        internal bool NeverArranged = true;

        internal bool ArrangeInProgress;

        private SKMatrix computedTransformation;

        internal bool NeverTransform = true;


        internal bool TransformationDirty;

        /// <summary>Obtient une valeur indiquant si la trasnformation calculée pour cet element lors de la disposition de cet élément est valide. </summary>
        /// <returns>true si la transformation calculée est valide ; sinon, false.</returns>
        public bool IsTransformationValid
        {
            get
            {
                return !this.TransformationDirty;
            }
        }

        /// <summary>Invalide l'état de réorganisation (disposition) de l'élément.Après invalidation, l'élément voit sa disposition actualisée, ce qui se produit de façon asynchrone à moins qu'elle ne soit forcée a posteriori par <see cref="M:System.Windows.UIElement.UpdateLayout" />.</summary>
        public void InvalidateArrange()
        {
            if (!this.ArrangeDirty && !this.ArrangeInProgress)
            {
                this.ArrangeDirty = true;
            }
        }

        /// <summary>Positionne des éléments enfants et détermine une taille pour <see cref="T:System.Windows.UIElement" />.Les éléments parents appellent cette méthode depuis leur implémentation <see cref="M:System.Windows.UIElement.ArrangeCore(System.Windows.Rect)" /> (ou un équivalent au niveau de l'infrastructure WPF) pour former une actualisation de disposition récursive.Cette méthode constitue la seconde passe d'une actualisation de disposition.</summary>
        /// <param name="finalRect">La taille finale que le parent calcule pour l'élément enfant, fournie sous forme d'instance <see cref="T:System.Windows.Rect" />.</param>
        public void Arrange(SKRect finalRect)
        {
            if (this.Visibility == false)
            {
                this.finalRect = finalRect;
            }
            else
            {
                if (this.MeasureDirty || this.NeverMeasured)
                {
                    try
                    {
                        this.MeasureDuringArrange = true;
                        if (this.NeverMeasured)
                        {
                            this.Measure(finalRect.Size);
                        }
                        else
                        {
                            this.Measure(this.previousAvailableSize);
                        }
                    }
                    finally
                    {
                        this.MeasureDuringArrange = false;
                    }
                }

                if (!this.IsArrangeValid || this.NeverArranged || NeverTransform || TransformationDirty || !FloatUtil.AreClose(finalRect, this.finalRect))
                {
                    this.NeverArranged = false;
                    this.ArrangeInProgress = true;
                    try
                    {

                        this.ArrangeCore(finalRect);
                    }
                    finally
                    {
                        this.ArrangeInProgress = false;
                    }
                    this.finalRect = finalRect;
                    this.ArrangeDirty = false;
                }
            }
        }

        /// <summary>Définit le modèle pour la définition de disposition de réorganisation au niveau du noyau WPF. </summary>
        /// <param name="finalRect">Zone finale dans le parent que cet élément doit utiliser pour se réorganiser et réorganiser ses éléments enfants.</param>
        protected virtual void ArrangeCore(SKRect finalRect)
        {

            CreateBounds(finalRect);

            if (NeverTransform || TransformationDirty)
            {
                computedTransformation = SKMatrix.MakeIdentity();
                if (Transformation.HasValue)
                {
                    var transformationPivot = this.TransformationPivot;
                    var tx = (boundsMinusMargin.Width * transformationPivot.X);
                    var ty = (boundsMinusMargin.Height * transformationPivot.Y);

                    var anchor = SKMatrix.MakeTranslation(tx, ty);
                    var anchorN = SKMatrix.MakeTranslation(-tx, -ty);

                    computedTransformation = computedTransformation.Concat(anchor).Concat(Transformation.Value)
                                           .Concat(anchorN);
                }
            }

            this.VisualTransform = SKMatrix.MakeIdentity().Translate(boundsMinusMargin.Left, boundsMinusMargin.Top).Concat(computedTransformation);
        }

        private void CreateBounds(SKRect finalRect)
        {
            var margin = this.Margin;
            float newLeft = finalRect.Left + margin.Left;
            float newTop = finalRect.Top + margin.Top;

            float newWidth = finalRect.Width;
            if (float.IsNaN(finalRect.Width) ||float.IsInfinity(finalRect.Width) /* || this.desiredSize.Width < finalRect.Width*/)
            {
                newWidth = this.desiredSize.Width;
            }
            newWidth -= margin.Right;

            float newHeight = finalRect.Height;
            if (float.IsNaN(finalRect.Height) || float.IsInfinity(finalRect.Height)/* || this.desiredSize.Height < finalRect.Height*/)
            {
                newHeight = this.desiredSize.Height;
            }
            newHeight -= margin.Bottom;

            boundsMinusMargin = new SKRect(newLeft, newTop, newLeft + newWidth, newTop + newHeight);
        }

        #endregion

        #region Render

        internal bool RenderDirty;

        /// <summary>Obtient une valeur indiquant si les dimensions et la position calculées d'éléments enfants dans la disposition de cet élément sont valides. </summary>
        /// <returns>true si les dimensions et la position de disposition sont valides ; sinon, false.</returns>
        public bool IsRenderValid
        {
            get
            {
                return !this.ArrangeDirty;
            }
        }


        public virtual void Render(SKCanvas canvas)
        {
            SKMatrix visual = VisualTransform;
            SKMatrix invert = visual.Invert();
            canvas.Concat(ref visual);
            Draw(canvas);
            canvas.Concat(ref invert);
        }


        public virtual void Draw(SKCanvas canvas)
        {

        }

        #endregion

        public virtual void Invalidate()
        {
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }

        public virtual bool IsPointInside(SKPoint point, SKMatrix transfromStack)
        {
            var mat = transfromStack;//.Concat(this.VisualTransform);
            if (mat.TryInvert(out var invert))
            {
                point = invert.MapPoint(point);
            }

            return boundsMinusMargin.Contains(point);
        }

        
    }
}
