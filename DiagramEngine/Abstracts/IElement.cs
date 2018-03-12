using ModelerClient.DiagramEngine.Core;
using SkiaSharp;
using System;

namespace ModelerClient.DiagramEngine.Abstracts
{
    /// <summary>
    /// Représente un élement visuel au sein d'une arborescence
    /// Ajoute la notion de parent/enfants et de padding
    /// </summary>
    public interface IElement : IVisual
    {
        /// <summary>
        /// Composant père
        /// </summary>
        IElement Parent { get; set; }

        /// <summary>
        /// Liste des composants enfants
        /// </summary>
        ElementsCollection Children { get; }

        /// <summary>
        /// Padding 
        /// </summary>
        SKRect Padding { get; set; }

        /// <summary>
        /// Méthode appelée par un enfant pour notifié son père que sa <see cref="IVisual.DesiredSize"/> a changé
        /// </summary>
        /// <param name="child"></param>
        void OnChildDesiredSizeChanged(Element child);
        
        /// <summary>
        /// Appelé lors du changement de parent
        /// </summary>
        /// <param name="parent">le nouveau parent</param>
        void AttachParent(IElement parent);

        /// <summary>
        /// Récupère la première feuille contenant le point passé en paramètre
        /// </summary>
        /// <param name="point">le point</param>
        /// <returns>le premier element feuille contenant le point</returns>
        IElement GetElementAtPoint(SKPoint point);
        /// <summary>
        /// Récupère la première feuille contenant le point passé en paramètre et validant le predicate
        /// </summary>
        /// <param name="point">le point</param>
        /// <param name="predicate">la condition de validation</param>
        /// <returns>le premier element feuille contenant le point et validant la condition</returns>
        IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate);

        /// <summary>
        /// Récupère la première feuille contenant le point passé en paramètre et validant le predicate
        /// </summary>
        /// <param name="point">le point</param>
        /// <param name="predicate">la condition de validation</param>
        /// <param name="transformationsStack">l'historique des transformation menant à ce composant</param>
        /// <returns>le premier element feuille contenant le point et validant la conditio</returns>
        IElement GetElementAtPoint(SKPoint point, Func<IElement, bool> predicate, SKMatrix transformationsStack);

    }
}
