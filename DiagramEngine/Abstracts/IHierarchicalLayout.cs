

using SynodeTechnologies.SkiaSharp.DiagramEngine.Controls;
using SkiaSharp;
using System.Collections.Generic;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts
{
    /// <summary>
    /// Permet de disposer les <see cref="HierarchicalNode"/> au cours des 3 phases (Measure,Arrange et Render)
    /// </summary>
    public interface IHierarchicalLayout
    {
        /// <summary>
        ///  Measure l'ensemble des elements et retourne la taille désirée en fonction du layout
        /// </summary>
        /// <param name="elements">les elements à layout</param>
        /// <param name="availableSize">la taille disponible</param>
        /// <returns>la taille désirée</returns>
        SKSize Measure(IList<HierarchicalNode> elements, SKSize availableSize);

        /// <summary>
        ///  Positionne les elements
        /// </summary>
        /// <param name="elements">les elements à layout</param>
        /// <param name="bounds">les dimensions</param>
        void Arrange(IList<HierarchicalNode> elements, SKRect bounds);

        /// <summary>
        /// Affiche les éléments
        /// </summary>
        /// <param name="elements">les elements à layout</param>
        /// <param name="bounds">les dimensions</param>
        /// <param name="canvas">le canvas dans lequel rendre les composants</param>
        void Render(IList<HierarchicalNode> elements, SKRect bounds,SKCanvas canvas);

    }
}