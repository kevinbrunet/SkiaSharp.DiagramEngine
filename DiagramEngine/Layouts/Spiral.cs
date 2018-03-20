using System;
using System.Collections.Generic;
using System.Text;
using SynodeTechnologies.SkiaSharp.DiagramEngine.Abstracts;
using SkiaSharp;
using Xamarin.Forms;

namespace SynodeTechnologies.SkiaSharp.DiagramEngine.Layouts
{
    public class Spiral : Base
    {
        private enum Move
        {
            Bottom =0,
            Left = 1,
            Top = 2,
            Right = 3
        }
        private int[,] BuildGraph(IList<IElement> elements)
        {
            int nb = (int)Math.Ceiling(Math.Sqrt(elements.Count));
            int[,] mat;
            if (elements.Count == 1)
            {
                mat = new int[1, 1];
                mat[0, 0] = 0;
            }
            else if (elements.Count == 2)
            {
                mat = new int[2, 1];
                mat[0, 0] = 0;
                mat[1, 0] = 1;

            }
            else
            {
                mat = new int[Math.Max(nb,3), Math.Max(nb, 3)];
                int maxX = mat.GetLength(0);
                int maxY = mat.GetLength(1);
                for(int i = 0; i < maxX; i++)
                {
                    for(int j=0;j<maxY;j++)
                    {
                        mat[i, j] = -1;
                    }
                }

                int x = (int)Math.Floor(maxX / 2.0);
                int y = (int)Math.Floor(maxY / 2.0);
                Move m = Move.Left;
                for (int i = 0;i<elements.Count;i++)
                {
                    mat[x, y] = i;
                    bool done = false;
                    if (i < elements.Count - 1)
                    {
                        do
                        {
                            switch (m)
                            {
                                case Move.Bottom:
                                    if (y + 1 < maxY && mat[x, y + 1] == -1)
                                    {
                                        y++;
                                        m = Move.Left;
                                        done = true;
                                    }
                                    else
                                    {
                                        m = Move.Right;
                                    }
                                    break;
                                case Move.Left:
                                    if (x - 1 >= 0 && mat[x - 1, y] == -1)
                                    {
                                        x--;
                                        m = Move.Top;
                                        done = true;
                                    }
                                    else
                                    {
                                        m = Move.Bottom;
                                    }
                                    break;
                                case Move.Right:
                                    if (x + 1 < maxX && mat[x + 1, y] == -1)
                                    {
                                        x++;
                                        m = Move.Bottom;
                                        done = true;
                                    }
                                    else
                                    {
                                        m = Move.Top;
                                    }
                                    break;
                                case Move.Top:
                                    if (y - 1 >= 0 && mat[x, y - 1] == -1)
                                    {
                                        y--;
                                        m = Move.Right;
                                        done = true;
                                    }
                                    else
                                    {
                                        m = Move.Left;
                                    }
                                    break;
                            }
                        } while (!done);
                    }
                }
            }
            return mat;

        }

        public override SKSize Measure(IList<IElement> elements, SKSize availableSize)
        {
            float width = 0.0f;
            float height = 0.0f;
            foreach (var element in elements)
            {
                element.Measure(availableSize);


                if (width < element.DesiredSize.Width)
                {
                    width = element.DesiredSize.Width;
                }

                if (height < element.DesiredSize.Height)
                {
                    height = element.DesiredSize.Height;
                }
            }

            var graph = BuildGraph(elements);

            return new SKSize(graph.GetLength(0)*width, graph.GetLength(1) * height);
        }

        public override void Arrange(IList<IElement> elements, SKRect bounds)
        {
            var graph = BuildGraph(elements);
            SKSize cellSize = new SKSize((float)Math.Ceiling(bounds.Width / (float)graph.GetLength(0)), (float)Math.Ceiling(bounds.Height / (float)graph.GetLength(1)));
            for(int x = 0;x<graph.GetLength(0);x++)
            {
                for(int y=0;y<graph.GetLength(1);y++)
                {
                    var idx = graph[x, y];
                    if (idx > -1)
                    {
                        var elem = elements[idx];
                        float offsetX = cellSize.Width * x;
                        float offsetY = cellSize.Height * y;
                        offsetX += (cellSize.Width - elem.DesiredSize.Width) / 2.0f;
                        offsetY += (cellSize.Height - elem.DesiredSize.Height) / 2.0f;
                        elem.Arrange(new SKRect(offsetX, offsetY, offsetX+ elem.DesiredSize.Width, offsetY+elem.DesiredSize.Height));
                    }
                }
            }
        }

        
    }
}
