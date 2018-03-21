using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HierarchicalItemsTemplate : ContentPage
	{
        public class HierarchicalItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public HierarchicalItem(string name)
            {
                this.name = name;
            }

            private string name;
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }

            private ObservableCollection<HierarchicalItem> children = new ObservableCollection<HierarchicalItem>();

            public ObservableCollection<HierarchicalItem> Children

            {
                get
                {
                    return children;
                }
            }
        }

        private HierarchicalItem root;
        public HierarchicalItem Root
        {
            get
            {
                return root;
            }
           
        }


        public HierarchicalItemsTemplate()
		{
			InitializeComponent ();
            root = new HierarchicalItem("Root");
            root.Children.Add(new HierarchicalItem("1"));
            root.Children[0].Children.Add(new HierarchicalItem("1.1"));
            root.Children[0].Children.Add(new HierarchicalItem("1.2"));
            root.Children[0].Children.Add(new HierarchicalItem("1.3"));
            root.Children.Add(new HierarchicalItem("2"));
            root.Children[1].Children.Add(new HierarchicalItem("2.1"));
            root.Children[1].Children.Add(new HierarchicalItem("2.2"));
            root.Children[1].Children.Add(new HierarchicalItem("2.3"));
            root.Children.Add(new HierarchicalItem("3azertyuiopmlkjhgfdsqwxcvbnazertyuiopmlkjhgfdsqwxcvbnazertyuiopmlk\ndsfsgdfhfghhhfghgfhghfghf"));
            root.Children[2].Children.Add(new HierarchicalItem("3.1"));
            root.Children[2].Children.Add(new HierarchicalItem("3.2"));
            root.Children[2].Children.Add(new HierarchicalItem("3.3"));
            root.Children[2].Children[0].Children.Add(new HierarchicalItem("azertyuiopmlkjhgfdsqwxcvbn"));
            this.BindingContext = this;
		}
	}
}