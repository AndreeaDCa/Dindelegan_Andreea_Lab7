using Dindelegan_Andreea_Lab7.Models;

namespace Dindelegan_Andreea_Lab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    private async Task OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            
            if (button.BindingContext is Product product)
            {
                var confirmation = await DisplayAlert("Confirmare", $"Doriti sa stergeti produsul: {product.Nume}?", "Da", "Nu");

                if (confirmation)
                {
                    
                    await App.Database.DeleteProductAsync(product);

                    var shopList = (ShopList)BindingContext;
                    listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
                }
            }
        }
    }
}