using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelerClient.DiagramEngine.Abstracts;
using SkiaSharp;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class Grid : Base
    {

        public static readonly BindableProperty RowProperty = BindableProperty.CreateAttached("Row", typeof(int), typeof(Grid), default(int), validateValue: (bindable, value) => (int)value >= 0);

        public static readonly BindableProperty RowSpanProperty = BindableProperty.CreateAttached("RowSpan", typeof(int), typeof(Grid), 1, validateValue: (bindable, value) => (int)value >= 1);

        public static readonly BindableProperty ColumnProperty = BindableProperty.CreateAttached("Column", typeof(int), typeof(Grid), default(int), validateValue: (bindable, value) => (int)value >= 0);

        public static readonly BindableProperty ColumnSpanProperty = BindableProperty.CreateAttached("ColumnSpan", typeof(int), typeof(Grid), 1, validateValue: (bindable, value) => (int)value >= 1);

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(float), typeof(Grid), 6.0f);

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(float), typeof(Grid), 6.0f);

        public static readonly BindableProperty ColumnDefinitionsProperty = BindableProperty.Create("ColumnDefinitions", typeof(ColumnDefinitionCollection), typeof(Grid),
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (oldvalue != null)
                    ((ColumnDefinitionCollection)oldvalue).ItemSizeChanged -= ((Grid)bindable).OnDefinitionChanged;
                if (newvalue != null)
                    ((ColumnDefinitionCollection)newvalue).ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
            }, defaultValueCreator: bindable =>
            {
                var colDef = new ColumnDefinitionCollection();
                colDef.ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
                return colDef;
            });

        public static readonly BindableProperty RowDefinitionsProperty = BindableProperty.Create("RowDefinitions", typeof(RowDefinitionCollection), typeof(Grid),
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (oldvalue != null)
                    ((RowDefinitionCollection)oldvalue).ItemSizeChanged -= ((Grid)bindable).OnDefinitionChanged;
                if (newvalue != null)
                    ((RowDefinitionCollection)newvalue).ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
            }, defaultValueCreator: bindable =>
            {
                var rowDef = new RowDefinitionCollection();
                rowDef.ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
                return rowDef;
            });

        private void OnDefinitionChanged(object sender, EventArgs args)
        {
            //base.ComputeConstrainsForChildren();
            this.UpdateInheritedBindingContexts();
        }

        private void UpdateInheritedBindingContexts()
        {
            object bindingContext = base.BindingContext;
            RowDefinitionCollection rowDefs = this.RowDefinitions;
            if (rowDefs != null)
            {
                for (int i = 0; i < rowDefs.Count; i++)
                {
                    BindableObject.SetInheritedBindingContext(rowDefs[i], bindingContext);
                }
            }
            ColumnDefinitionCollection colDefs = this.ColumnDefinitions;
            if (colDefs != null)
            {
                for (int j = 0; j < colDefs.Count; j++)
                {
                    BindableObject.SetInheritedBindingContext(colDefs[j], bindingContext);
                }
            }
        }

        private List<ColumnDefinition> _columns;

        private List<RowDefinition> _rows;

        public ColumnDefinitionCollection ColumnDefinitions
        {
            get { return (ColumnDefinitionCollection)GetValue(ColumnDefinitionsProperty); }
            set { SetValue(ColumnDefinitionsProperty, value); }
        }

        public float ColumnSpacing
        {
            get { return (float)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public RowDefinitionCollection RowDefinitions
        {
            get { return (RowDefinitionCollection)GetValue(RowDefinitionsProperty); }
            set { SetValue(RowDefinitionsProperty, value); }
        }

        public float RowSpacing
        {
            get { return (float)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public static int GetColumn(BindableObject bindable)
        {
            return (int)bindable.GetValue(ColumnProperty);
        }

        public static int GetColumnSpan(BindableObject bindable)
        {
            return (int)bindable.GetValue(ColumnSpanProperty);
        }

        public static int GetRow(BindableObject bindable)
        {
            return (int)bindable.GetValue(RowProperty);
        }

        public static int GetRowSpan(BindableObject bindable)
        {
            return (int)bindable.GetValue(RowSpanProperty);
        }

        public static void SetColumn(BindableObject bindable, int value)
        {
            bindable.SetValue(ColumnProperty, value);
        }

        public static void SetColumnSpan(BindableObject bindable, int value)
        {
            bindable.SetValue(ColumnSpanProperty, value);
        }

        public static void SetRow(BindableObject bindable, int value)
        {
            bindable.SetValue(RowProperty, value);
        }

        public static void SetRowSpan(BindableObject bindable, int value)
        {
            bindable.SetValue(RowSpanProperty, value);
        }


        public override SKSize Measure(IList<IElement> elements, SKSize availableSize)
        {
            MeasureGrid(elements, availableSize.Width, availableSize.Height, true);
            SKSize ret = new SKSize();
            List<ColumnDefinition> columnsCopy = this._columns;
            List<RowDefinition> rowsCopy = this._rows;
            for (int index = 0; index < elements.Count; index++)
            {
                IElement child = (IElement)elements[index];
                if (child.Visibility)
                {
                    int r = Grid.GetRow((BindableObject)child);
                    int c = Grid.GetColumn((BindableObject)child);
                    int rs = Grid.GetRowSpan((BindableObject)child);
                    int cs = Grid.GetColumnSpan((BindableObject)child);
                    float posx = (float)c * this.ColumnSpacing;
                    for (int i = 0; i < c; i++)
                    {
                        posx += (float)columnsCopy[i].ActualWidth;
                    }
                    float posy = (float)r * this.RowSpacing;
                    for (int j = 0; j < r; j++)
                    {
                        posy += (float)rowsCopy[j].ActualHeight;
                    }
                    float w = (float)columnsCopy[c].ActualWidth;
                    for (int k = 1; k < cs; k++)
                    {
                        w += this.ColumnSpacing + (float)columnsCopy[c + k].ActualWidth;
                    }
                    float h = (float)rowsCopy[r].ActualHeight;
                    for (int l = 1; l < rs; l++)
                    {
                        h += this.RowSpacing + (float)rowsCopy[r + l].ActualHeight;
                    }
                    ret.Width = Math.Max(ret.Width, posx + w);
                    ret.Height = Math.Max(ret.Height, posy + h);
                }
            }
            if (_columns.Any(c => c.Width.IsStar) || _rows.Any(r => r.Height.IsStar))
                return availableSize;
            else
                return ret;
        }

        public override void Arrange(IList<IElement> elements, SKRect bounds)
        {
            if (elements.Count == 0)
            {
                return;
            }
            this.MeasureGrid(elements,bounds.Width, bounds.Height, false);
            List<ColumnDefinition> columnsCopy = this._columns;
            List<RowDefinition> rowsCopy = this._rows;
            for (int index = 0; index < elements.Count; index++)
            {
                IElement child = (IElement)elements[index];
                if (child.Visibility)
                {
                    int r = Grid.GetRow((BindableObject)child);
                    int c = Grid.GetColumn((BindableObject)child);
                    int rs = Grid.GetRowSpan((BindableObject)child);
                    int cs = Grid.GetColumnSpan((BindableObject)child);
                    float posx = bounds.Left + (float)c * this.ColumnSpacing;
                    for (int i = 0; i < c; i++)
                    {
                        posx += (float)columnsCopy[i].ActualWidth;
                    }
                    float posy = bounds.Top + (float)r * this.RowSpacing;
                    for (int j = 0; j < r; j++)
                    {
                        posy += (float)rowsCopy[j].ActualHeight;
                    }
                    float w = (float)columnsCopy[c].ActualWidth;
                    for (int k = 1; k < cs; k++)
                    {
                        w += this.ColumnSpacing + (float)columnsCopy[c + k].ActualWidth;
                    }
                    float h = (float)rowsCopy[r].ActualHeight;
                    for (int l = 1; l < rs; l++)
                    {
                        h += this.RowSpacing + (float)rowsCopy[r + l].ActualHeight;
                    }
                    child.Arrange(new SKRect(posx, posy, posx+ w, posy+h));
                }
            }
        }

      


        void MeasureGrid(IList<IElement> elements, float width, float height, bool requestSize = false)
        {
            EnsureRowsColumnsInitialized(elements);

            AssignAbsoluteCells();

            CalculateAutoCells(elements,width, height);

            if (!requestSize)
            {
                ContractColumnsIfNeeded(width, c => c.Width.IsAuto);
                ContractRowsIfNeeded(height, r => r.Height.IsAuto);
            }

            double totalStarsHeight = 0;
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (row.Height.IsStar)
                    totalStarsHeight += row.Height.Value;
            }

            double totalStarsWidth = 0;
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (col.Width.IsStar)
                    totalStarsWidth += col.Width.Value;
            }

            if (requestSize)
            {
                MeasureAndContractStarredColumns(elements,width, height, totalStarsWidth);
                MeasureAndContractStarredRows(elements, width, height, totalStarsHeight);
            }
            else
            {
                CalculateStarCells(width, height, totalStarsWidth, totalStarsHeight);
            }

            ZeroUnassignedCells();

            ExpandLastAutoRowIfNeeded(elements,height, requestSize);
            ExpandLastAutoColumnIfNeeded(elements, width, requestSize);
        }

        void ZeroUnassignedCells()
        {
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (col.ActualWidth < 0)
                    col.ActualWidth = 0;
            }
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (row.ActualHeight < 0)
                    row.ActualHeight = 0;
            }
        }


        void AssignAbsoluteCells()
        {
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (row.Height.IsAbsolute)
                    row.ActualHeight = (float)row.Height.Value;
            }

            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (col.Width.IsAbsolute)
                    col.ActualWidth = (float)col.Width.Value;
            }
        }


        void CalculateAutoCells(IList<IElement> elements, float width, float height)
        {
            // this require multiple passes. First process the 1-span, then 2, 3, ...
            // And this needs to be run twice, just in case a lower-span column can be determined by a larger span
            for (var iteration = 0; iteration < 2; iteration++)
            {
                for (var rowspan = 1; rowspan <= _rows.Count; rowspan++)
                {
                    for (var i = 0; i < _rows.Count; i++)
                    {
                        RowDefinition row = _rows[i];
                        if (!row.Height.IsAuto)
                            continue;
                        if (row.ActualHeight >= 0) // if Actual is already set (by a smaller span), skip till pass 3
                            continue;

                        double actualHeight = row.ActualHeight;
                        double minimumHeight = row.MinimumHeight;
                        for (var index = 0; index < elements.Count; index++)
                        {
                            var child = elements[index];
                            if (!child.Visibility|| GetRowSpan((BindableObject)child) != rowspan || !IsInRow((BindableObject)child, i) || NumberOfUnsetRowHeight((BindableObject)child) > 1)
                                continue;
                            double assignedWidth = GetAssignedColumnWidth((BindableObject)child);
                            double assignedHeight = GetAssignedRowHeight((BindableObject)child);
                            double widthRequest = assignedWidth + GetUnassignedWidth(width);
                            double heightRequest = double.IsPositiveInfinity(height) ? double.PositiveInfinity : assignedHeight + GetUnassignedHeight(height);

                            child.Measure(new SKSize((float)widthRequest, (float)heightRequest));
                            actualHeight = Math.Max(actualHeight, child.DesiredSize.Height - assignedHeight - RowSpacing * (GetRowSpan((BindableObject)child) - 1));
                            //minimumHeight = Math.Max(minimumHeight, child.DesiredSize.Height - assignedHeight - RowSpacing * (GetRowSpan((BindableObject)child) - 1));
                        }
                        if (actualHeight >= 0)
                            row.ActualHeight = actualHeight;
                        if (minimumHeight >= 0)
                            row.MinimumHeight = minimumHeight;
                    }
                }

                for (var colspan = 1; colspan <= _columns.Count; colspan++)
                {
                    for (var i = 0; i < _columns.Count; i++)
                    {
                        ColumnDefinition col = _columns[i];
                        if (!col.Width.IsAuto)
                            continue;
                        if (col.ActualWidth >= 0) // if Actual is already set (by a smaller span), skip
                            continue;

                        double actualWidth = col.ActualWidth;
                        double minimumWidth = col.MinimumWidth;
                        for (var index = 0; index < elements.Count; index++)
                        {
                            var child = elements[index];
                            if (!child.Visibility || GetColumnSpan((BindableObject)child) != colspan || !IsInColumn((BindableObject)child, i) || NumberOfUnsetColumnWidth((BindableObject)child) > 1)
                                continue;
                            double assignedWidth = GetAssignedColumnWidth((BindableObject)child);
                            double assignedHeight = GetAssignedRowHeight((BindableObject)child);
                            double widthRequest = double.IsPositiveInfinity(width) ? double.PositiveInfinity : assignedWidth + GetUnassignedWidth(width);
                            double heightRequest = assignedHeight + GetUnassignedHeight(height);

                            child.Measure(new SKSize((float)widthRequest, (float)heightRequest));
                            actualWidth = Math.Max(actualWidth, child.DesiredSize.Width - assignedWidth - (GetColumnSpan((BindableObject)child) - 1) * ColumnSpacing);
                            //minimumWidth = Math.Max(minimumWidth, child.DesiredSize.Width - assignedWidth - (GetColumnSpan((BindableObject)child) - 1) * ColumnSpacing);
                        }
                        if (actualWidth >= 0)
                            col.ActualWidth = actualWidth;
                        if (minimumWidth >= 0)
                            col.MinimumWidth = actualWidth;
                    }
                }
            }
        }

        void CalculateStarCells(double width, double height, double totalStarsWidth, double totalStarsHeight)
        {
            double starColWidth = GetUnassignedWidth(width) / totalStarsWidth;
            double starRowHeight = GetUnassignedHeight(height) / totalStarsHeight;

            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (col.Width.IsStar)
                    col.ActualWidth = col.Width.Value * starColWidth;
            }

            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (row.Height.IsStar)
                    row.ActualHeight = row.Height.Value * starRowHeight;
            }
        }

        void ContractColumnsIfNeeded(double width, Func<ColumnDefinition, bool> predicate)
        {
            double columnWidthSum = 0;
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition c = _columns[index];
                columnWidthSum += c.ActualWidth;
            }

            double rowHeightSum = 0;
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition r = _rows[index];
                rowHeightSum += r.ActualHeight;
            }

            var request = new Size(columnWidthSum + (_columns.Count - 1) * ColumnSpacing, rowHeightSum + (_rows.Count - 1) * RowSpacing);
            if (request.Width > width)
            {
                double contractionSpace = 0;
                for (var index = 0; index < _columns.Count; index++)
                {
                    ColumnDefinition c = _columns[index];
                    if (predicate(c))
                        contractionSpace += c.ActualWidth - c.MinimumWidth;
                }
                if (contractionSpace > 0)
                {
                    // contract as much as we can but no more
                    double contractionNeeded = Math.Min(contractionSpace, Math.Max(request.Width - width, 0));
                    double contractFactor = contractionNeeded / contractionSpace;
                    for (var index = 0; index < _columns.Count; index++)
                    {
                        ColumnDefinition col = _columns[index];
                        if (!predicate(col))
                            continue;
                        double availableSpace = col.ActualWidth - col.MinimumWidth;
                        double contraction = availableSpace * contractFactor;
                        col.ActualWidth -= contraction;
                        contractionNeeded -= contraction;
                    }
                }
            }
        }

        void ContractRowsIfNeeded(double height, Func<RowDefinition, bool> predicate)
        {
            double columnSum = 0;
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition c = _columns[index];
                columnSum += Math.Max(0, c.ActualWidth);
            }
            double rowSum = 0;
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition r = _rows[index];
                rowSum += Math.Max(0, r.ActualHeight);
            }

            var request = new Size(columnSum + (_columns.Count - 1) * ColumnSpacing, rowSum + (_rows.Count - 1) * RowSpacing);
            if (request.Height <= height)
                return;
            double contractionSpace = 0;
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition r = _rows[index];
                if (predicate(r))
                    contractionSpace += r.ActualHeight - r.MinimumHeight;
            }
            if (!(contractionSpace > 0))
                return;
            // contract as much as we can but no more
            double contractionNeeded = Math.Min(contractionSpace, Math.Max(request.Height - height, 0));
            double contractFactor = contractionNeeded / contractionSpace;
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (!predicate(row))
                    continue;
                double availableSpace = row.ActualHeight - row.MinimumHeight;
                double contraction = availableSpace * contractFactor;
                row.ActualHeight -= contraction;
                contractionNeeded -= contraction;
            }
        }

        void EnsureRowsColumnsInitialized(IList<IElement> elements)
        {
            _columns = ColumnDefinitions == null ? new List<ColumnDefinition>() : ColumnDefinitions.ToList();
            _rows = RowDefinitions == null ? new List<RowDefinition>() : RowDefinitions.ToList();

            int lastRow = -1;
            for (var index = 0; index < elements.Count; index++)
            {
                var w = (BindableObject)elements[index];
                lastRow = Math.Max(lastRow, GetRow(w) + GetRowSpan(w) - 1);
            }
            lastRow = Math.Max(lastRow, RowDefinitions.Count - 1);

            int lastCol = -1;
            for (var index = 0; index < elements.Count; index++)
            {
                var w = (BindableObject)elements[index];
                lastCol = Math.Max(lastCol, GetColumn(w) + GetColumnSpan(w) - 1);
            }
            lastCol = Math.Max(lastCol, ColumnDefinitions.Count - 1);

            while (_columns.Count <= lastCol)
                _columns.Add(new ColumnDefinition());
            while (_rows.Count <= lastRow)
                _rows.Add(new RowDefinition());

            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                col.ActualWidth = -1;
            }
            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                row.ActualHeight = -1;
            }
        }

        void ExpandLastAutoColumnIfNeeded(IList<IElement> elements,double width, bool expandToRequest)
        {
            for (var index = 0; index < elements.Count; index++)
            {
                var child = elements[index];
                if (!child.Visibility)
                    continue;

                ColumnDefinition col = GetLastAutoColumn((BindableObject)child);
                if (col == null)
                    continue;

                double assignedWidth = GetAssignedColumnWidth((BindableObject)child);
                double w = double.IsPositiveInfinity(width) ? double.PositiveInfinity : assignedWidth + GetUnassignedWidth(width);
                child.Measure(new SKSize((float)w, (float)GetAssignedRowHeight((BindableObject)child)));
                double requiredWidth = expandToRequest ? child.DesiredSize.Width :1;
                double deltaWidth = requiredWidth - assignedWidth - (GetColumnSpan((BindableObject)child) - 1) * ColumnSpacing;
                if (deltaWidth > 0)
                {
                    col.ActualWidth += deltaWidth;
                }
            }
        }

        void ExpandLastAutoRowIfNeeded(IList<IElement> elements, double height, bool expandToRequest)
        {
            for (var index = 0; index < elements.Count; index++)
            {
                var child = elements[index];
                if (!child.Visibility)
                    continue;

                RowDefinition row = GetLastAutoRow((BindableObject)child);
                if (row == null)
                    continue;

                double assignedHeight = GetAssignedRowHeight((BindableObject)child);
                double h = double.IsPositiveInfinity(height) ? double.PositiveInfinity : assignedHeight + GetUnassignedHeight(height);
                child.Measure(new SKSize((float)GetAssignedColumnWidth((BindableObject)child), (float)h));
                double requiredHeight = expandToRequest ? child.DesiredSize.Height : 1;
                double deltaHeight = requiredHeight - assignedHeight - (GetRowSpan((BindableObject)child) - 1) * RowSpacing;
                if (deltaHeight > 0)
                {
                    row.ActualHeight += deltaHeight;
                }
            }
        }

        void MeasureAndContractStarredColumns(IList<IElement> elements, double width, double height, double totalStarsWidth)
        {
            double starColWidth;
            starColWidth = MeasuredStarredColumns(elements);

            if (!double.IsPositiveInfinity(width) && double.IsPositiveInfinity(height))
            {
                // re-zero columns so GetUnassignedWidth returns correctly
                for (var index = 0; index < _columns.Count; index++)
                {
                    ColumnDefinition col = _columns[index];
                    if (col.Width.IsStar)
                        col.ActualWidth = 0;
                }

                starColWidth = Math.Max(starColWidth, GetUnassignedWidth(width) / totalStarsWidth);
            }

            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (col.Width.IsStar)
                    col.ActualWidth = col.Width.Value * starColWidth;
            }

            ContractColumnsIfNeeded(width, c => c.Width.IsStar);
        }

        void MeasureAndContractStarredRows(IList<IElement> elements, double width, double height, double totalStarsHeight)
        {
            double starRowHeight;
            starRowHeight = MeasureStarredRows(elements);

            if (!double.IsPositiveInfinity(height) && double.IsPositiveInfinity(width))
            {
                for (var index = 0; index < _rows.Count; index++)
                {
                    RowDefinition row = _rows[index];
                    if (row.Height.IsStar)
                        row.ActualHeight = 0;
                }

                starRowHeight = Math.Max(starRowHeight, GetUnassignedHeight(height) / totalStarsHeight);
            }

            for (var index = 0; index < _rows.Count; index++)
            {
                RowDefinition row = _rows[index];
                if (row.Height.IsStar)
                    row.ActualHeight = row.Height.Value * starRowHeight;
            }

            ContractRowsIfNeeded(height, r => r.Height.IsStar);
        }

        double MeasuredStarredColumns(IList<IElement> elements)
        {
            double starColWidth;
            for (var iteration = 0; iteration < 2; iteration++)
            {
                for (var colspan = 1; colspan <= _columns.Count; colspan++)
                {
                    for (var i = 0; i < _columns.Count; i++)
                    {
                        ColumnDefinition col = _columns[i];
                        if (!col.Width.IsStar)
                            continue;
                        if (col.ActualWidth >= 0) // if Actual is already set (by a smaller span), skip
                            continue;

                        double actualWidth = col.ActualWidth;
                        double minimumWidth = col.MinimumWidth;
                        for (var index = 0; index < elements.Count; index++)
                        {
                            var child = elements[index];
                            if (!child.Visibility || GetColumnSpan((BindableObject)child) != colspan || !IsInColumn((BindableObject)child, i) || NumberOfUnsetColumnWidth((BindableObject)child) > 1)
                                continue;
                            double assignedWidth = GetAssignedColumnWidth((BindableObject)child);

                            child.Measure(new SKSize(float.PositiveInfinity, float.PositiveInfinity));
                            actualWidth = Math.Max(actualWidth, child.DesiredSize.Width - assignedWidth - (GetColumnSpan((BindableObject)child) - 1) * ColumnSpacing);
                            //minimumWidth = Math.Max(minimumWidth, child.DesiredSize.Width - assignedWidth - (GetColumnSpan((BindableObject)child) - 1) * ColumnSpacing);
                        }
                        if (actualWidth >= 0)
                            col.ActualWidth = actualWidth;

                        if (minimumWidth >= 0)
                            col.MinimumWidth = minimumWidth;
                    }
                }
            }

            //Measure the stars
            starColWidth = 1;
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnDefinition col = _columns[index];
                if (!col.Width.IsStar)
                    continue;
                starColWidth = Math.Max(starColWidth, col.ActualWidth / col.Width.Value);
            }

            return starColWidth;
        }

        private double MeasureStarredRows(IList<IElement> elements)
        {
            for (int iteration = 0; iteration < 2; iteration++)
            {
                for (int rowspan = 1; rowspan <= this._rows.Count; rowspan++)
                {
                    for (int i = 0; i < this._rows.Count; i++)
                    {
                        RowDefinition row = this._rows[i];
                        if (row.Height.IsStar && row.ActualHeight < 0.0)
                        {
                            double actualHeight = row.ActualHeight;
                            double minimumHeight = row.MinimumHeight;
                            for (int index = 0; index < elements.Count; index++)
                            {
                                var child = elements[index];
                                if (child.Visibility && Grid.GetRowSpan((BindableObject)child) == rowspan && Grid.IsInRow((BindableObject)child, i) && this.NumberOfUnsetRowHeight((BindableObject)child) <= 1)
                                {
                                    double assignedHeight = this.GetAssignedRowHeight((BindableObject)child);
                                    double assignedWidth = this.GetAssignedColumnWidth((BindableObject)child);
                                    child.Measure(new SKSize((float)assignedWidth, float.PositiveInfinity));
                                    actualHeight = Math.Max(actualHeight, child.DesiredSize.Height - assignedHeight - this.RowSpacing * (double)(Grid.GetRowSpan((BindableObject)child) - 1));
                                    //minimumHeight = Math.Max(minimumHeight, child.DesiredSize.Height - assignedHeight - this.RowSpacing * (double)(Grid.GetRowSpan((BindableObject)child) - 1));
                                }
                            }
                            if (actualHeight >= 0.0)
                            {
                                row.ActualHeight = actualHeight;
                            }
                            if (minimumHeight >= 0.0)
                            {
                                row.MinimumHeight = minimumHeight;
                            }
                        }
                    }
                }
            }
            double starRowHeight = 1.0;
            for (int index2 = 0; index2 < this._rows.Count; index2++)
            {
                RowDefinition row2 = this._rows[index2];
                if (row2.Height.IsStar)
                {
                    starRowHeight = Math.Max(starRowHeight, row2.ActualHeight / row2.Height.Value);
                }
            }
            return starRowHeight;
        }

        private ColumnDefinition GetLastAutoColumn(BindableObject child)
        {
            int index = Grid.GetColumn(child);
            int span = Grid.GetColumnSpan(child);
            for (int i = index + span - 1; i >= index; i--)
            {
                if (this._columns[i].Width.IsAuto)
                {
                    return this._columns[i];
                }
            }
            return null;
        }

        private RowDefinition GetLastAutoRow(BindableObject child)
        {
            int index = Grid.GetRow(child);
            int span = Grid.GetRowSpan(child);
            for (int i = index + span - 1; i >= index; i--)
            {
                if (this._rows[i].Height.IsAuto)
                {
                    return this._rows[i];
                }
            }
            return null;
        }

        private double GetAssignedColumnWidth(BindableObject child)
        {
            double actual = 0.0;
            int index = Grid.GetColumn(child);
            int span = Grid.GetColumnSpan(child);
            for (int i = index; i < index + span; i++)
            {
                if (this._columns[i].ActualWidth >= 0.0)
                {
                    actual += this._columns[i].ActualWidth;
                }
            }
            return actual;
        }


        private double GetUnassignedHeight(double heightRequest)
        {
            double assigned = (double)(this._rows.Count - 1) * this.RowSpacing;
            for (int i = 0; i < this._rows.Count; i++)
            {
                double actual = this._rows[i].ActualHeight;
                if (actual >= 0.0)
                {
                    assigned += actual;
                }
            }
            return heightRequest - assigned;
        }

        private double GetUnassignedWidth(double widthRequest)
        {
            double assigned = (double)(this._columns.Count - 1) * this.ColumnSpacing;
            for (int i = 0; i < this._columns.Count; i++)
            {
                double actual = this._columns[i].ActualWidth;
                if (actual >= 0.0)
                {
                    assigned += actual;
                }
            }
            return widthRequest - assigned;
        }

        private static bool IsInColumn(BindableObject child, int column)
        {
            int childColumn = Grid.GetColumn(child);
            int span = Grid.GetColumnSpan(child);
            return childColumn <= column && column < childColumn + span;
        }

        private static bool IsInRow(BindableObject child, int row)
        {
            int childRow = Grid.GetRow(child);
            int span = Grid.GetRowSpan(child);
            return childRow <= row && row < childRow + span;
        }


        private double GetAssignedRowHeight(BindableObject child)
        {
            double actual = 0.0;
            int index = Grid.GetRow(child);
            int span = Grid.GetRowSpan(child);
            for (int i = index; i < index + span; i++)
            {
                if (this._rows[i].ActualHeight >= 0.0)
                {
                    actual += this._rows[i].ActualHeight;
                }
            }
            return actual;
        }

        private int NumberOfUnsetColumnWidth(BindableObject child)
        {
            int i = 0;
            int index = Grid.GetColumn(child);
            int span = Grid.GetColumnSpan(child);
            for (int j = index; j < index + span; j++)
            {
                if (this._columns[j].ActualWidth <= 0.0)
                {
                    i++;
                }
            }
            return i;
        }

        private int NumberOfUnsetRowHeight(BindableObject child)
        {
            int i = 0;
            int index = Grid.GetRow(child);
            int span = Grid.GetRowSpan(child);
            for (int j = index; j < index + span; j++)
            {
                if (this._rows[j].ActualHeight <= 0.0)
                {
                    i++;
                }
            }
            return i;
        }

    }
}
