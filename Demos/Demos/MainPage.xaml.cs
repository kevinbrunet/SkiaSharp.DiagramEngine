using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Demos
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            this.GetStarted.Pressed += GetStarted_Pressed;
            this.WithLayout.Pressed += WithLayout_Pressed;
            this.TemplateItems.Pressed += TemplateItems_Pressed;
            this.ZoomCanvas.Pressed += ZoomCanvas_Pressed;
		}

        private void ZoomCanvas_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ZoomCanvas());
        }

        private void TemplateItems_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TemplateItems());
        }

        private void WithLayout_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WithLayout());
        }

        private void GetStarted_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetStarted());
        }
    }
}
