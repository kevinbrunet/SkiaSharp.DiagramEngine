using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views.Forms;

namespace ModelerClient.DiagramEngine.Abstracts
{
    /// <summary>
    /// Permet de gerer les evenements de clic ou de toucher sur le canvas
    /// </summary>
    public interface ITouchListener
    {
        /// <summary>
        /// Le touchListener suivant car c'est une pile
        /// </summary>
        ITouchListener Next
        {
            get;
            set;
        }

        /// <summary>
        /// Methode traitant l'évenement OnTouch
        /// </summary>
        /// <param name="rootElement">element racine de la recherche</param>
        /// <param name="e">l'evenement</param>
        /// <returns>l'element qui traite l'évenement</returns>
        IElement OnTouch(IElement rootElement, SKTouchEventArgs e);

    }
}
