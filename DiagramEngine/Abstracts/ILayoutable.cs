using System;
using System.Collections.Generic;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts
{
    /// <summary>
    /// Ajoute la notion de layout des enfants
    /// </summary>
    public interface ILayoutable : IView
    {
        /// <summary>
        /// Layout associé
        /// </summary>
        ILayout Layout
        {
            get;
            set;
        }
    }
}
