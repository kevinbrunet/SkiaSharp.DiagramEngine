using SkiaSharp;

namespace ModelerClient.DiagramEngine.Abstracts
{
    /// <summary>
    /// Réprésente un élement visuel
    /// Il dispose de marges, d'une position, d'une taille et d'une matrice de transformation
    /// </summary>
    public interface IVisual
    {
        /// <summary>
        /// Représente la marge autour du composant
        /// Celle ci diminue le rectangle de taille et position
        /// </summary>
        SKRect Margin { get; set; }
        /// <summary>
        /// Matrice de transformation associé au composant
        /// Pratique pour les animations
        /// </summary>
        SKMatrix? Transformation { get; set; }

        /// <summary>
        /// Point de pivot de la matrice de transormation
        /// </summary>
        SKPoint TransformationPivot { get; set; }

        /// <summary>
        /// Permet de savoir si la transformation a été prise en compte et est valide
        /// </summary>
        bool IsTransformationValid { get; }


        /// <summary>
        /// Statut de visibilité du composant
        /// </summary>
        bool Visibility { get; set; }


        /// <summary>
        /// Position du composant sur l'axe X
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Position du composant sur l'axe Y
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Largeur du composant
        /// Si NaN ou Infinity la taille est automatiquement calculé
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// Hauteur du composant
        /// Si NaN ou Infinity la taille est automatiquement calculé
        /// </summary>
        float Height { get; set; }

        /// <summary>
        /// Taille demandée par le composant dans l'absolue
        /// Celle-ci tient compte des contraintes Width/Height
        /// Le calcul de cette taille est effectué pendant la phase de Measure
        /// </summary>
        SKSize DesiredSize { get; }

        /// <summary>
        /// Permet de savoir si la phase de Measure a été effectué et est valide
        /// </summary>
        bool IsMeasureValid { get; }
        /// <summary>
        /// Permet de savoir si la phase de Measure est en cours de calcul
        /// </summary>
        bool MeasureInProgress { get; }

        /// <summary>
        /// Matrice de transformation contenant l'offeset des marges ainsi que la matrice d'animation <see cref="Transformation"/> avec son pivot <see cref="TransformationPivot"/>
        /// </summary>
        SKMatrix VisualTransform { get; }

        /// <summary>
        /// Représente la taille du composant affecté lors de la phase Arrange
        /// </summary>
        SKRect Bounds { get; }

        /// <summary>
        /// Permet d'invalider la phase de Measure
        /// </summary>
        void InvalidateMeasure();

        /// <summary>
        /// Declenche la phase de Measure
        /// </summary>
        /// <param name="availableSize">taille maximum dont dispose le composant (la taille peut-être infini)</param>
        void Measure(SKSize availableSize);


        /// <summary>
        /// Permet de savoir si la phase de Arrange a été effectuée et est valide
        /// </summary>
        bool IsArrangeValid { get; }

        /// <summary>
        /// Permet d'invalider la phase de Arrange
        /// </summary>
        void InvalidateArrange();

        /// <summary>
        /// Declenche la phase Arrange
        /// </summary>
        /// <param name="finalRect"></param>
        void Arrange(SKRect finalRect);


        /// <summary>
        /// Permet de savoir si la phase de rendu a été effectuée et est valide.
        /// </summary>
        bool IsRenderValid { get; }
        
        /// <summary>
        /// Declenche la phase de rendu
        /// </summary>
        /// <param name="canvas">le canvas dans lequel rendre</param>
        void Render(SKCanvas canvas);
       

        /// <summary>
        /// Invalide l'ensembles des phases (Measure,Arrange et Render)
        /// </summary>
        void Invalidate();


        /// <summary>
        /// Permet de savoir si le point est contenu dans l'espace du composant ou non
        /// </summary>
        /// <param name="point">le point a tester</param>
        /// <param name="transfromStack">l'historique des transformation menant à ce composant</param>
        /// <returns>true si le point est contenu dans l'espace du composant</returns>
        bool IsPointInside(SKPoint point, SKMatrix transfromStack);



    }
}
