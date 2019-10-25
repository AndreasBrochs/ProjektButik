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
        Button button;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        public void Start()
        {
            grid = (Grid)Content;

            for (int i = 0; i<6;i++) 
            {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 10;i++) 
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

           

        }
    }
}
