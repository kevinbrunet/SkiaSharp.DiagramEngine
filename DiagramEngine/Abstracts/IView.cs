using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts
{
    /// <summary>
    /// Réprésente un element visuel au sein d'une arborecence avec un fond et une bordure
    /// </summary>
    public interface IView : IElement
    {
        /// <summary>
        /// Couleur de fond du composant
        /// </summary>
        SKColor BackgroundColor { get; set; }

        /// <summary>
        /// Couleur de la bordure du composant
        /// </summary>
        SKColor BorderColor { get; set; }

        /// <summary>
        /// Epaisseur de la bordure
        /// </summary>
        float BorderWidth { get; set; }

        /// <summary>
        /// Arrondie aux coins
        /// </summary>
        SKPoint CornerRadius { get; set; }

    }
}
