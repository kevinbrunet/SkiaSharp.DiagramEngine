using System;
using System.Collections.Generic;
using System.Text;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts
{
    public interface IItemsTemplate
    {
        IVisual CreateDefault(object item);

        void SetupContent(object item, int index);
    }
}
