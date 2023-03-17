using DevExpress.XamarinForms.CollectionView;
using DevExpress.XamarinForms.Editors;
using DevExpress.XamarinForms.Popup;
using SmartMobileWMS.Models;
using SmartMobileWMS.Views.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SmartMobileWMS.Views.CustomViews
{
    public class ProductPopup : DXPopup
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(Label),
            defaultValue: default(string));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly BindableProperty ProductsProperty = BindableProperty.Create(
            propertyName: "Products",
            returnType: typeof(ObservableCollection<Product>),
            declaringType: typeof(ProductPopup),
            defaultValue: default(string));

        public ObservableCollection<Product> Products
        {
            get { return (ObservableCollection<Product>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }
        public static readonly BindableProperty SelectedProductProperty = BindableProperty.Create(
           propertyName: "SelectedProduct",
           returnType: typeof(Product),
           declaringType: typeof(ProductPopup),
           defaultValue: default(Product));

        public Product SelectedProduct
        {
            get { return (Product)GetValue(SelectedProductProperty); }
            set { SetValue(SelectedProductProperty, value); }
        }
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
           propertyName: "IsLoading",
           returnType: typeof(bool),
           declaringType: typeof(ProductPopup),
           defaultValue: default(bool));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        public event EventHandler<EventArgs> TextChanged;
        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = TextChanged;
            handler?.Invoke(this, e);
        }
        public event EventHandler<CollectionViewSelectionChangedEventArgs> SelectionChanged;
        protected virtual void OnSelectionChanged(CollectionViewSelectionChangedEventArgs e)
        {
            EventHandler<CollectionViewSelectionChangedEventArgs> handler = SelectionChanged;
            handler?.Invoke(this, e);
        }
        public ProductPopup()
        {
            Frame frame = new Frame()
            {
                Padding = 20,
                WidthRequest = 340,
            };
            Grid grid = new Grid
            {
                RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition{ Height = new GridLength(50)},
                        new RowDefinition { Height = new GridLength(360) },
                        new RowDefinition { Height = new GridLength(60) },
                    },
            };
            grid.Children.Add(new Label
            {
                Text = "Search Product"
            });
            var searchText = new TextEdit
            {
                PlaceholderText = "Αναζήτηση"
            };
            searchText.BindingContext = this;
            searchText.SetBinding(TextEdit.TextProperty, "Text");
            searchText.TextChanged += (sender, args) =>
            {
                OnTextChanged(args);
            };
            grid.Children.Add(searchText, 0,1);
            var itemCollection = new DXCollectionView();
            itemCollection.ItemTemplate = new DataTemplate(() => { return new ProductTemplate(); });
            itemCollection.SelectionMode = SelectionMode.Single;
            itemCollection.BindingContext = this;
            itemCollection.SetBinding(DXCollectionView.ItemsSourceProperty, "Products");
            itemCollection.SetBinding(DXCollectionView.SelectedItemProperty, "SelectedProduct");
            itemCollection.SelectionChanged += (sender, args) =>
            {
                OnSelectionChanged(args);
                SelectedProduct = (Product)args.SelectedItems[0];
                this.IsOpen = false;
            };
            grid.Children.Add(itemCollection, 0, 2);
            Button button = new Button
            {
                Text = "Κλείσιμο",
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
            };
            button.Clicked += Button_Clicked;
            grid.Children.Add(button, 0, 3);
            StackLayout popupContent = new StackLayout()
            {
                WidthRequest = 200,
                BackgroundColor = Color.AliceBlue
            };
            Label popupLabel = new Label()
            {
                Margin = new Thickness(5, 5)
            };
            var indicator = new ActivityIndicator();
            indicator.BindingContext = this;
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            grid.Children.Add(indicator);
            frame.Content = grid;
            popupLabel.BindingContext = this;
            popupLabel.SetBinding(Label.TextProperty, "Text");
            popupContent.Children.Add(popupLabel);
            this.Content = frame;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.IsOpen = false;
        }
    }
}