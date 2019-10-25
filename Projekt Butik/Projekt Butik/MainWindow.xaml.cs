using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Projekt_Butik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid;
        WrapPanel wrapPanel;
        TextBox header;
        ListBox productsListBox;
        TextBox nrProducts;
        Button addRemove;

        private Thickness defaultMargin = new Thickness(5);


        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        public void Start()
        {
            Title = "Guitar store";
            Width = 900;
            Height = 600;
            BasicLayout();
            Controlls();
        }
        public void BasicLayout()
        {
            grid = (Grid)Content;

            for (int i = 0; i < 6; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 10; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            header = new TextBox
            {
                Text = "Welcome to the store!",
                TextAlignment = TextAlignment.Center,
                FontSize = 20,
                FontFamily = new FontFamily("Verdana"),
                Margin = defaultMargin,
                Padding = defaultMargin,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = Brushes.LightBlue
            };
            grid.Children.Add(header);
            Grid.SetRowSpan(header, 2);
            Grid.SetColumnSpan(header, 6);

            productsListBox = new ListBox
            {
                Margin = defaultMargin
            };
            grid.Children.Add(productsListBox);
            productsListBox.Items.Add("Test product1");
            productsListBox.Items.Add("Test product2");
            productsListBox.Items.Add("Test product3");
            productsListBox.Items.Add("Test product4");
            Grid.SetColumn(productsListBox, 0);
            Grid.SetColumnSpan(productsListBox, 2);
            Grid.SetRow(productsListBox, 2);
            Grid.SetRowSpan(productsListBox, 6);

            Image productImage = CreateImage("gibsonsg.jpg");
            grid.Children.Add(productImage);
            Grid.SetColumn(productImage, 4);
            Grid.SetRow(productImage, 3);

        }
        public void Controlls()
        {
            wrapPanel = new WrapPanel //wrappanel for the controlls adding/removing produkts to the cart
            {
                Orientation = Orientation.Horizontal
            };
            grid.Children.Add(wrapPanel);
            Grid.SetColumn(wrapPanel, 0);
            Grid.SetColumnSpan(wrapPanel, 6);
            Grid.SetRow(wrapPanel, 8);
            Grid.SetRowSpan(wrapPanel, 2);

            addRemove = new Button
            {
                Content = "+",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);

            nrProducts = new TextBox
            {
                Text = "1",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(nrProducts);

            addRemove = new Button
            {
                Content = "-",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove = new Button
            {
                Content = "Add to cart",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
        }
        private Image CreateImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            Image image = new Image
            {
                Source = source,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = defaultMargin
            };
            return image;
        }
    }

    public class Product
    {
        string brand;

    }
}
