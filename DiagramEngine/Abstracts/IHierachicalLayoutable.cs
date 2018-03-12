using System;
using System.Collections.Generic;
using System.Text;

namespace ModelerClient.DiagramEngine.Abstracts
{
    /// <summary>
    /// Ajoute une notion de layout hierarchique aux enfants
    /// </summary>
    public interface IHierarchicalLayoutable : IView
    {
        /// <summary>
        /// Layout associé
        /// </summary>
        IHierarchicalLayout Layout
        {
            get;
            set;
        }
    }
}
