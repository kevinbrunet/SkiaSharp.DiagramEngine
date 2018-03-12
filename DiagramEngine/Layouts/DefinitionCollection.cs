using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ModelerClient.DiagramEngine.Layouts
{
    public class DefinitionCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IDefinition
    {
        private readonly List<T> _internalList = new List<T>();

        public event EventHandler ItemSizeChanged;

        public int Count
        {
            get
            {
                return this._internalList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                return this._internalList[index];
            }
            set
            {
                if (index < this._internalList.Count && index >= 0 && this._internalList[index] != null)
                {
                    T t = this._internalList[index];
                    t.SizeChanged -= new EventHandler(this.OnItemSizeChanged);
                }
                this._internalList[index] = value;
                value.SizeChanged += new EventHandler(this.OnItemSizeChanged);
                this.OnItemSizeChanged(this, EventArgs.Empty);
            }
        }

        internal DefinitionCollection()
        {
        }

        public void Add(T item)
        {
            this._internalList.Add(item);
            item.SizeChanged += new EventHandler(this.OnItemSizeChanged);
            this.OnItemSizeChanged(this, EventArgs.Empty);
        }

        public void Clear()
        {
            foreach (T item in this._internalList)
            {
                item.SizeChanged -= new EventHandler(this.OnItemSizeChanged);
            }
            this._internalList.Clear();
            this.OnItemSizeChanged(this, EventArgs.Empty);
        }

        public bool Contains(T item)
        {
            return this._internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            item.SizeChanged -= new EventHandler(this.OnItemSizeChanged);
            bool expr_25 = this._internalList.Remove(item);
            if (expr_25)
            {
                this.OnItemSizeChanged(this, EventArgs.Empty);
            }
            return expr_25;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._internalList.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._internalList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this._internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this._internalList.Insert(index, item);
            item.SizeChanged += new EventHandler(this.OnItemSizeChanged);
            this.OnItemSizeChanged(this, EventArgs.Empty);
        }

        public void RemoveAt(int index)
        {
            T item = this._internalList[index];
            this._internalList.RemoveAt(index);
            item.SizeChanged -= new EventHandler(this.OnItemSizeChanged);
            this.OnItemSizeChanged(this, EventArgs.Empty);
        }

        private void OnItemSizeChanged(object sender, EventArgs e)
        {
            this.ItemSizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
