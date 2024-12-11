using MuresanDianaLab7.Models;
using Plugin.LocalNotification;

namespace MuresanDianaLab7;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;

        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };

        var shopLocation = locations?.FirstOrDefault();

        if (shopLocation != null)
        {
            var myLocation = await Geolocation.GetLocationAsync();
            /* For testing on a Windows machine, use the following hardcoded location:
               var myLocation = new Location(46.7731796289, 23.6213886738); */

            var distance = myLocation.CalculateDistance(shopLocation, DistanceUnits.Kilometers);

            if (distance < 5)
            {
                var request = new NotificationRequest
                {
                    Title = "Ai de facut cumparaturi in apropiere!",
                    Description = address,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            }

            // Open the map with the shop location
            await Map.OpenAsync(shopLocation, options);
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Unable to locate the address. Please check the address and try again.",
                "OK"
            );
        }
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (shop != null)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete the shop '{shop.ShopName}'?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                await App.Database.DeleteShopAsync(shop);
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Unable to delete the shop. Please try again.",
                "OK"
            );
        }
    }
}
