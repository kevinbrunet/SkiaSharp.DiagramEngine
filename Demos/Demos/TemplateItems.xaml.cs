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
	public partial class TemplateItems : ContentPage
	{
        public class Item : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;


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
        }

        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items
        {
            get
            {
                return items;
            }
           
        }

        public ICommand AddItemCommand { get; }

        public TemplateItems()
		{
			InitializeComponent ();
            this.AddItemCommand = new Command(() =>
            {
                Items.Add(new Item() { Name = "Item" + Items.Count });
            });
            this.BindingContext = this;
		}
	}
}