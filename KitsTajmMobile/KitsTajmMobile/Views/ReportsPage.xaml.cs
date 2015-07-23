using KitsTajmMobile.Service;
using KitsTajmMobile.ViewModels;
using Xamarin.Forms;

namespace KitsTajmMobile.Views
{
    public partial class ReportsPage : ContentPage
    {
        private readonly IKitsTajmService _service;
        public ReportsViewModel Model
        {
            get
            {
                return (ReportsViewModel)this.Resources["Model"];
            }
            private set
            {
                this.Resources["Model"] = value;
            }
        }

        public ReportsPage(IKitsTajmService service)
        {
            this._service = service;

            InitializeComponent();

            this.Model = new ReportsViewModel();

            this.Knappen.Clicked += (sender, e) =>
            {
                this.Model.Text2.Row = 1 - this.Model.Text2.Row;
            };

            this.Texten2.SetBinding<ReportsViewModel.Position>(Grid.ColumnProperty, p => p.Column);
            this.Texten2.SetBinding<ReportsViewModel.Position>(Grid.RowProperty, p => p.Row);
        }
    }
}
