
using Microsoft.Maui.Controls;
using MuresanDianaLab7.Models;
namespace MuresanDianaLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
  
            await App.Database.DeleteProductAsync(selectedProduct);

  
            var shopList = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);

           
            await DisplayAlert("Success", "Item deleted successfully.", "OK");
        }
        else
        {
           
            await DisplayAlert("No Item Selected", "Please select an item to delete.", "OK");
        }
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shopList = (ShopList)BindingContext;
        shopList.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        shopList.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(shopList);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shopList = (ShopList)BindingContext;

        await App.Database.DeleteShopListAsync(shopList);
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
}