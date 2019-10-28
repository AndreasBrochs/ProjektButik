using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        DataGrid dataGrid;
        WrapPanel wrapPanel;
        TextBox header;
        ListBox productsListBox;
        ListBox tempCart;
        TextBox nrProducts;
        TextBox discount;
        Button addRemove;
        TextBlock totalPrice;
        DataTable dataTable;


        public List<Product> productlist;
        public List<string> listBoxProducts;

        private Thickness defaultMargin = new Thickness(5);
        public MainWindow()
        {
            InitializeComponent();
            CreateProducts();
            Start();
        }
        public void Start()
        {
            Title = "Guitar store";
            Width = 900;
            Height = 600;
            BasicLayout();
            ControllsProductsInStore();
            ControllsCart();
            ControllsBuy();
        }
        public void BasicLayout()
        {
            grid = (Grid)Content;
            dataGrid = new DataGrid();
            
            
            
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
            //productsListBox = new ListBox
            //{
            //    Margin = defaultMargin
            //};
            //grid.Children.Add(productsListBox);
            ////productsListBox.Items.Add("Test product1");
            ////productsListBox.Items.Add("Test product2");
            ////productsListBox.Items.Add("Test product3");
            ////productsListBox.Items.Add("Test product4");
            //foreach(string s in listBoxProducts)
            //{
            //    productsListBox.Items.Add(s);
            //}
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Info", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Price", typeof(int)));
            for(int i = 0; i < productlist.Count; i++)
            {
                string brand = productlist[i].brand;
                string info = productlist[i].info;
                int price = productlist[i].price;
                dataTable.Rows.Add(new object[] { brand, info, price });
            }
            grid.Children.Add(dataGrid);
            Grid.SetColumn(dataGrid, 0);
            Grid.SetColumnSpan(dataGrid, 2);
            Grid.SetRow(dataGrid, 2);
            Grid.SetRowSpan(dataGrid, 6);
            dataGrid.ItemsSource = dataTable.DefaultView;





            Image productImage = CreateImage(productlist[2].soruce.ToString());
            grid.Children.Add(productImage);
            Grid.SetColumn(productImage, 4);
            Grid.SetRow(productImage, 3);
            Grid.SetColumn(productImage, 2);
            Grid.SetColumnSpan(productImage, 2);
            Grid.SetRow(productImage, 2);
            Grid.SetRowSpan(productImage, 6);

            tempCart = new ListBox
            {
                Margin = defaultMargin
            };
            grid.Children.Add(tempCart);
            Grid.SetColumn(tempCart, 4);
            Grid.SetColumnSpan(tempCart, 2);
            Grid.SetRow(tempCart, 2);
            Grid.SetRowSpan(tempCart, 4);
            tempCart.Items.Add("Test cart");
        }
        public void ControllsCart()
        {
            wrapPanel = new WrapPanel
            {
                Orientation = Orientation.Horizontal
            };
            grid.Children.Add(wrapPanel);
            Grid.SetColumn(wrapPanel, 4);
            Grid.SetColumnSpan(wrapPanel, 2);
            Grid.SetRow(wrapPanel, 6);
            Grid.SetRowSpan(wrapPanel, 2);

            addRemove = new Button
            {
                Content = "Remove product",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += RemoveProduct_Click;

            addRemove = new Button
            {
                Content = "Empty cart",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += EmptyCart_Click;

            addRemove = new Button
            {
                Content = "Save Cart",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += SaveCart_Click;
        }
        private void SaveCart_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public void ControllsBuy()
        {
            wrapPanel = new WrapPanel //wrappanel for the discount
            {
                Orientation = Orientation.Horizontal
            };
            grid.Children.Add(wrapPanel);
            Grid.SetColumn(wrapPanel, 4);
            Grid.SetColumnSpan(wrapPanel, 6);
            Grid.SetRow(wrapPanel, 7);
            Grid.SetRowSpan(wrapPanel, 2);

            Label discountLabel = new Label
            {
                Content = "Discount code:",
                VerticalAlignment = VerticalAlignment.Center,
            };
            wrapPanel.Children.Add(discountLabel);

            discount = new TextBox
            {
                Text = "enter code",
                Margin = defaultMargin,
                Padding = defaultMargin,
                Width = 75 //to keep the box the same size when writing in it
            };
            wrapPanel.Children.Add(discount);
            Grid.SetColumn(discount, 5);
            Grid.SetRow(discount, 6);

            addRemove = new Button
            {
                Content = "Add discount",
                Margin = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += Discount_Click;
            wrapPanel = new WrapPanel //wrappanel for the controlls under the cart, minus the discount
            {
                Orientation = Orientation.Horizontal
            };
            grid.Children.Add(wrapPanel);
            Grid.SetColumn(wrapPanel, 4);
            Grid.SetColumnSpan(wrapPanel, 2);
            Grid.SetRow(wrapPanel, 8);
            Grid.SetRowSpan(wrapPanel, 3);

            totalPrice = new TextBlock
            {
                Text = "Total Price: 0kr",
                VerticalAlignment = VerticalAlignment.Center,
                Width = 120
            };
            wrapPanel.Children.Add(totalPrice);

            addRemove = new Button
            {
                Content = "BUY",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += Buy_Click;
        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Discount_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public void ControllsProductsInStore()
        {
            wrapPanel = new WrapPanel //wrappanel for the controlls under the avaiable proucts
            {
                Orientation = Orientation.Horizontal
            };
            grid.Children.Add(wrapPanel);
            Grid.SetColumn(wrapPanel, 0);
            Grid.SetColumnSpan(wrapPanel, 2);
            Grid.SetRow(wrapPanel, 8);
            Grid.SetRowSpan(wrapPanel, 3);

            addRemove = new Button
            {
                Content = "+",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += PlusProduct_Click;

            nrProducts = new TextBox
            {
                Text = "1",
                Margin = defaultMargin,
                Padding = new Thickness(10),
                Width = 40
            };
            wrapPanel.Children.Add(nrProducts);

            addRemove = new Button
            {
                Content = "-",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += MinusProduct_Click;

            addRemove = new Button
            {
                Content = "Add to cart",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += AddToCart_Click;
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MinusProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            int i = int.Parse(nrProducts.Text);
            if (i != 0)
            {
            i--;
            string count = i.ToString();
            nrProducts.Text = count;
            }
            }
            catch
            {
                MessageBox.Show("You must enter a number");
            }
        }
        private void PlusProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            int i = int.Parse(nrProducts.Text);
            i++;
            string count = i.ToString();
            nrProducts.Text = count;
            }
            catch
            {
                MessageBox.Show("You must enter a number");
            }
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
        public void CreateProducts()
        {
            string[] path = File.ReadAllLines("productlist.txt");
            productlist = new List<Product>();
            for (int i = 0; i < path.Length; i++)
            {

                string[] temp = path[i].Split(';');
                try
                {
                    int errorTest = int.Parse(temp[2]);
                }
                catch
                {
                    i++;
                    MessageBox.Show("Felaktig prissättning, kontrollera rad " + i + " i productlist.txt");
                    Environment.Exit(0);
                }

                if (temp.Length < 4)
                {
                    Product p = new Product
                    {
                        brand = temp[0],
                        info = temp[1],
                        price = int.Parse(temp[2]),
                        soruce = new BitmapImage(new Uri("/pics/error.jpg", UriKind.Relative))
                    };
                    productlist.Add(p);
                }
                else
                {
                        Product p = new Product
                        {
                            brand = temp[0],
                            info = temp[1],
                            price = int.Parse(temp[2]),
                            soruce = new BitmapImage(new Uri("/pics/" + temp[3], UriKind.Relative))
                        };
                        productlist.Add(p);
                    } 
                }

            listBoxProducts = new List<string>();
            listBoxProducts = productlist.Select(p => p.brand + " " + p.info + " " + p.price).ToList();
            }
        }
    }
    public class Product
    {
        public string brand;
        public string info;
        public int price;
        public ImageSource soruce;
    }
