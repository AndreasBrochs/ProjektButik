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
        ListBox showCart;
        TextBox nrProducts;
        TextBox discount;
        Button addRemove;
        Button buy;
        TextBlock totalPriceBlock;
        DataTable dataTable;

        public int shopIndex;
        public int shopAmount = 1;
        private int TotalPrice = 0;
        public int totalPrice
        {
            get
            {
                return TotalPrice;
            }
            set
            {
                if (value >= 0)
                {
                    TotalPrice = value;
                }
                else if (value < 0)
                {
                    showCart.Items.Clear();
                    totalPrice = 0;
                    totalPriceBlock.Text = $"Totalt Pris: {totalPrice}";
                    for (int i = 0; i < usedCodes.Count; i++)
                    {
                        usedCodes.Remove(usedCodes[i]);
                    }
                    buy.IsEnabled = false;
                }
            }
        }
        private ImageSource imageSource;
        private Image Image;

        public string test = new string(' ', 1000);
        public List<Product> productlist;
        public List<string> listBoxProducts;
        List<string> usedCodes = new List<string> { };
        public Dictionary<string, int> discountCodes;
        public Cart cart = new Cart();
        const string CartFilePath = @"C:\Windows\Temp\Cart.csv";

        private Thickness defaultMargin = new Thickness(5);
        public MainWindow()
        {
            InitializeComponent();
            CreateProducts();
            CreateDiscount();
            Start();
        }
        //Grid and layout
        public void Start()
        {
            Title = "Gitarr butik";
            Width = 1100;
            Height = 700;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            BasicLayout();

            ControllsProductsInStore();
            ControllsCart();
            ControllsBuy();
            LoadProducts();

        }
        public void BasicLayout()
        {
            Cart cart = new Cart();
            grid = (Grid)Content;
            dataGrid = new DataGrid
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

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
                Text = "Välkommen till Gitarrbutiken!",
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

            dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[3]
            { new DataColumn ("Brand", typeof(string)), new DataColumn ("Info", typeof(string)), new DataColumn("Price", typeof(string))});

            for (int i = 0; i < productlist.Count; i++)
            {
                dataTable.Rows.Add(new object[] { productlist[i].brand, productlist[i].info, String.Format("{0:### ###}", productlist[i].price) });
            };

            grid.Children.Add(dataGrid);
            Grid.SetColumn(dataGrid, 0);
            Grid.SetColumnSpan(dataGrid, 2);
            Grid.SetRow(dataGrid, 2);
            Grid.SetRowSpan(dataGrid, 6);
            dataGrid.IsReadOnly = true;
            dataGrid.CanUserSortColumns = false;
            dataGrid.CanUserResizeColumns = false;
            dataGrid.CanUserResizeRows = false;
            dataGrid.ItemsSource = dataTable.DefaultView;

            dataGrid.GotMouseCapture += DataGridProductClick;

            showCart = new ListBox
            {
                Margin = defaultMargin
            };
            grid.Children.Add(showCart);
            Grid.SetColumn(showCart, 4);
            Grid.SetColumnSpan(showCart, 2);
            Grid.SetRow(showCart, 2);
            Grid.SetRowSpan(showCart, 4);
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
                Content = "Ta bort produkt",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += RemoveProduct_Click;

            addRemove = new Button
            {
                Content = "Töm kundvagn",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += EmptyCart_Click;

            addRemove = new Button
            {
                Content = "Spara kundvagn",
                Margin = defaultMargin,
                Padding = defaultMargin
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += SaveCart_Click;
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
                Content = "Rabattkod:",
                VerticalAlignment = VerticalAlignment.Center,
            };
            wrapPanel.Children.Add(discountLabel);

            discount = new TextBox
            {
                Text = "Skriv kod",
                Margin = defaultMargin,
                Padding = defaultMargin,
                Width = 75 //to keep the box the same size when writing in it
            };
            wrapPanel.Children.Add(discount);
            discount.SelectAll();
            Grid.SetColumn(discount, 5);
            Grid.SetRow(discount, 6);

            addRemove = new Button
            {
                Content = "Aktivera rabatt",
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

            totalPriceBlock = new TextBlock
            {
                Text = $"Totaltpris: {totalPrice} kr",
                VerticalAlignment = VerticalAlignment.Center,
                Width = 120,
               
            };
            wrapPanel.Children.Add(totalPriceBlock);
            
            

            buy = new Button
            {
                Content = "KÖP",
                Margin = defaultMargin,
                Padding = new Thickness(10),
                IsEnabled = false,
                
            };
            wrapPanel.Children.Add(buy);
            buy.Click += Buy_Click;
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
                Text = shopAmount.ToString(),
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
                Content = "Lägg till i kundvagn",
                Margin = defaultMargin,
                Padding = new Thickness(10)
            };
            wrapPanel.Children.Add(addRemove);
            addRemove.Click += AddToCart_Click;
        }

        //Buttons
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                shopAmount = int.Parse(nrProducts.Text);
                if (shopAmount > 0)
                {
                    buy.IsEnabled = true;
                    showCart.Items.Clear();
                    string temp = productlist[shopIndex].info;
                    if (cart.shoppingCart.ContainsKey(temp))
                    {
                        cart.shoppingCart[temp] += shopAmount;
                        totalPrice += shopAmount * productlist[shopIndex].price;
                        //totalPriceBlock.Text = $"Totalt Pris: {totalPrice}kr";
                        totalPriceBlock.Text = String.Format("Totalpris {0: ### ### ###}", totalPrice + " kr");
                    }
                    else
                    {
                        cart.shoppingCart.Add(temp, shopAmount);
                        totalPrice += shopAmount * productlist[shopIndex].price;
                        //totalPriceBlock.Text = $"Totalt Pris: {totalPrice}kr";
                        totalPriceBlock.Text = String.Format("Totalpris {0: ### ### ###}", totalPrice + " kr");
                    }
                    foreach (KeyValuePair<string, int> key in cart.shoppingCart)
                    {
                        showCart.Items.Add(key).ToString();
                    }
                    foreach (string discount in usedCodes)
                    {
                        showCart.Items.Add(discount);
                    }
                }
                else
                {
                    MessageBox.Show("Du måste lägga till antal högre än 0");
                }
            }
            catch
            {
                nrProducts.Text = shopAmount.ToString();
            }
        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            string buy = "***************KVITTO***************\n\n";
            string thanks = "Tack för ditt köp, välkommen åter!\n";
            string mark = "-------------------------------------------\n";
            string receipt = "";
            string total = $"Summa: {totalPrice}\n";
            
            foreach (KeyValuePair<string, int> r in cart.shoppingCart)
            {
                int price = productlist.Where(p => p.info == r.Key).Select(p => p.price).First() * r.Value;
                receipt += r.Key.ToString() + "\n" + "Antal: " + r.Value.ToString() + "  Pris: ";
                receipt += String.Format("{0: ### ### ### ###}\n\n", price);
            }

            MessageBox.Show(buy + mark + receipt + mark + total + mark + thanks);
            //Har kommenterat bort denna bara för att det skulle gå snabbare att kolla på kvittot hur det såg ut ;)
            //if (File.Exists(CartFilePath))
            //{
            //    File.Delete(CartFilePath);
            //}
        }
        private void DataGridProductClick(object sender, MouseEventArgs e)
        {
            DataGrid data = (DataGrid)sender;

            if (data.SelectedIndex > -1)
            {
                var temp = data.SelectedItem;
                shopIndex = dataGrid.SelectedIndex;

                imageSource = new BitmapImage(new Uri(productlist[data.SelectedIndex].soruce.ToString(), UriKind.Relative));

                grid.Children.Remove(Image);

                Image = new Image
                {
                    Source = imageSource,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = defaultMargin
                };

                grid.Children.Add(Image);
                Grid.SetColumn(Image, 4);
                Grid.SetRow(Image, 3);
                Grid.SetColumn(Image, 2);
                Grid.SetColumnSpan(Image, 2);
                Grid.SetRow(Image, 2);
                Grid.SetRowSpan(Image, 6);
            }
        }
        private void Discount_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            if (usedCodes.Contains(discount.Text))
            {
                MessageBox.Show("Koden har redan använts");
                result = true;
            }
            else
            {
                foreach (KeyValuePair<string, int> pair in discountCodes)
                {
                    if (pair.Key == discount.Text)
                    {
                        result = true;
                        if (showCart.Items.Count > 0)
                        {
                            totalPrice -= pair.Value;
                            totalPriceBlock.Text = $"Totalt Pris: {totalPrice}";
                            usedCodes.Add(discount.Text);
                            showCart.Items.Add(pair.Key);
                        }
                        else
                        {
                            MessageBox.Show("Du måste ha produkter i kundvagnen för att kunna använda en rabattkod");
                        }
                    }
                }
            }
            if (result == false)
            {
                MessageBox.Show("Koden känns inte igen");
            }
        }
        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            cart.shoppingCart.Clear();
            totalPrice = 0;
            totalPriceBlock.Text = $"Totaltpris: {totalPrice} kr";
            if (usedCodes.Count > 0)
            {
                for (int i = 0; i <= usedCodes.Count; i++)
                {
                    usedCodes.Remove(usedCodes[i]);
                }
            }
            showCart.Items.Clear();
            buy.IsEnabled = false;
        }
        private void MinusProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                shopAmount = int.Parse(nrProducts.Text);
                if (shopAmount != 0)
                {
                    shopAmount--;
                    nrProducts.Text = shopAmount.ToString();
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
                shopAmount = int.Parse(nrProducts.Text);
                shopAmount++;
                nrProducts.Text = shopAmount.ToString();
            }
            catch
            {
                MessageBox.Show("You must enter a number");
            }
        }
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string remove = showCart.SelectedItem.ToString();

                foreach (KeyValuePair<string, int> key in cart.shoppingCart)
                {
                    if (key.ToString() == remove)
                    {
                        int price = productlist.Where(p => p.info.Contains(key.Key)).Select(p => p.price).First();

                        totalPrice -= key.Value * price;
                        cart.shoppingCart.Remove(key.Key);
                        showCart.Items.Remove(showCart.SelectedItem);
                        //totalPriceBlock.Text = $"Totaltpris: {totalPrice}";
                        totalPriceBlock.Text = string.Format("Totalpris: {0: ### ### ###}", totalPrice + " kr");
                        if (showCart.Items.IsEmpty)
                        {
                            buy.IsEnabled = false;
                        }
                        break;
                    }
                }
                foreach (KeyValuePair<string, int> pair in discountCodes)
                {
                    string temp = (string)showCart.SelectedItem;
                    if (temp == pair.Key)
                    {
                        totalPrice += pair.Value;
                        usedCodes.Remove(pair.Key);
                        showCart.Items.Remove(showCart.SelectedItem);
                        //totalPriceBlock.Text = $"Totalt Pris: {totalPrice}";
                        totalPriceBlock.Text = string.Format("Totalpris: {0: ### ### ###}", totalPrice +" kr");
                        break;
                    }
                }
            }
            catch
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
        private void SaveCart_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(CartFilePath))
            {
                MessageBoxButton fileFound = MessageBoxButton.YesNo;
                string title = "Kundvagn hittad";
                string text = "Det finns redan en Kundvagnsfil, vill du ersätta den med aktuell kundvagn ?";
                var message = MessageBox.Show(text, title, fileFound);
                if (message == MessageBoxResult.Yes)
                {
                    List<string> cartList = new List<string>();
                    foreach (KeyValuePair<string, int> pair in cart.shoppingCart)
                    {
                        string name = pair.Key;
                        int amount = pair.Value;
                        cartList.Add(name + ";" + amount);
                    }
                    File.WriteAllLines(CartFilePath, cartList);
                    MessageBox.Show("Din kundvagn är nu sparad.");
                }
            }
            else
            {
                List<string> cartList = new List<string>();
                foreach (KeyValuePair<string, int> pair in cart.shoppingCart)
                {
                    string name = pair.Key;
                    int amount = pair.Value;
                    cartList.Add(name + ";" + amount);
                }
                File.WriteAllLines(CartFilePath, cartList);
                MessageBox.Show("Din kundvagn är nu sparad.");
            }
        }
        
        //Read file methods
        public void CreateDiscount()
        {
            string[] path = File.ReadAllLines("discount.txt");
            discountCodes = new Dictionary<string, int>();
            for (int i = 0; i < path.Length; i++)
            {
                string[] discountName = path[i].Split(';');
                int discountAmount = int.Parse(discountName[1]);
                discountCodes.Add(discountName[0], discountAmount);
            }
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
                    System.Media.SystemSounds.Exclamation.Play();
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
        }
        public void LoadProducts()
        {
            if (File.Exists(CartFilePath))
            {
                MessageBoxButton fileFound = MessageBoxButton.YesNo;
                string title = "Kundvagn hittad";
                string text = "Det finns redan en Kundvagnsfil sparad, vill du öppna den?";
                System.Media.SystemSounds.Exclamation.Play();
                var message = MessageBox.Show(text, title, fileFound);
                if (message == MessageBoxResult.Yes)
                {
                    string[] loadCart = File.ReadAllLines(CartFilePath);
                    for (int i = 0; i < loadCart.Length; i++)
                    {
                        string[] temp = loadCart[i].Split(';');
                        cart.shoppingCart.Add(temp[0], int.Parse(temp[1]));
                    }
                    foreach (KeyValuePair<string, int> c in cart.shoppingCart)
                    {
                        int getPrice = productlist.Where(p => p.info.Contains(c.Key)).Select(p => p.price).First();
                        totalPrice += c.Value * getPrice;

                        showCart.Items.Add(c);
                    }
                    buy.IsEnabled = true;
                    //totalPriceBlock.Text = $"Totalt Pris: {totalPrice}kr";
                    totalPriceBlock.Text = string.Format("Totalpris: {0: ### ### ###}", totalPrice);
                }
            }
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
public class Cart
{
    public Dictionary<string, int> shoppingCart = new Dictionary<string, int>();
}