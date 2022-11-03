using SmartMobileWMS.Views;
using System.Collections.Generic;
using Xamarin.Forms;
using MenuItem = SmartMobileWMS.Models.MenuItem;

namespace SmartMobileWMS.Data
{
    public  class MenuItemsData
    {
        public static List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>{
                new MenuItem
                {
                    Title = "RFΠωλήσεις",
                    ImageSource = "sell.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new OrdersPage()))
                },
                new MenuItem
                {
                    Title = "RFΑγορές",
                    ImageSource = "notes.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new RFPurchasePage()))
                },
                new MenuItem
                {
                    Title = "RFΑπογραφή",
                    ImageSource = "notes.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new RFCesusPage()))
                },
                new MenuItem
                {
                    Title = "Παραλαβή σε θέση",
                    ImageSource = "boximport.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new PositionImportPage()))
                },
                new MenuItem
                {
                    Title = "Εξαγωγή απο θέση",
                    ImageSource = "boxexport.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new PositionExportPage()))
                },
                new MenuItem
                {
                    Title = "Μετακίνηση σε θέση",
                    ImageSource = "move.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new MoveToPositionPage()))
                },
                new MenuItem
                {
                    Title = "Ελευθερη συλλογή",
                    ImageSource = "go.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new FreePickingPage()))
                },
                new MenuItem
                {
                    Title = "Εντολή συλλογής",
                    ImageSource = "go.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new ColCommandPage()))
                },
                new MenuItem
                {
                    Title = "Υπόλοιπα Θέσης",
                    ImageSource = "census.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new RestOfPositionPage()))
                },
                new MenuItem
                {
                    Title = "Θέσεις Είδους",
                    ImageSource = "multiposition.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new RestOfProducts()))
                },
                new MenuItem
                {
                    Title = "Παλετοποίηση",
                    ImageSource = "palette.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new PaletteStartPage()))
                },
                new MenuItem
                {
                    Title = "SelectTrainingPage",
                    ImageSource = "palette.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new SelectTrainingPage()))
                },
                new MenuItem
                {
                    Title = "Ανταλλακτικό",
                    ImageSource = "screw.png",
                    Action = new Command(async () => await Shell.Current.Navigation.PushAsync(new SparePartPage()))
                }
            };
        }
    }
}
