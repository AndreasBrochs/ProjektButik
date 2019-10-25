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

namespace Projekt_Butik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        Grid grid;
        WrapPanel wrapPanel;
        Button button;
        TextBox header;
        ListBox productsListBox;
        TextBox nrProducts;

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
            Layout();
        }
        public void Layout()
        {
            grid = (Grid)Content;
            grid.Children.Add(wrapPanel);


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
            productsListBox.Items.Add("Test produkt1");
            productsListBox.Items.Add("Test produkt2");
            productsListBox.Items.Add("Test produkt3");
            productsListBox.Items.Add("Test produkt4");
            Grid.SetColumn(productsListBox, 0);
            Grid.SetColumnSpan(productsListBox, 3);
            Grid.SetRow(productsListBox, 2);
            Grid.SetRowSpan(productsListBox, 6);

            Grid.SetColumn(wrapPanel, 0);
            Grid.SetColumnSpan(wrapPanel, 6);
            Grid.SetRow(wrapPanel, 7);
            nrProducts = new TextBox
            {
                Text = "1"
            };
            wrapPanel.Children.Add(nrProducts);



        }
    }
}
