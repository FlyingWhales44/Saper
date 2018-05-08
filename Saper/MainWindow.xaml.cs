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

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SaperEngine engine;
        List<Button> ButtonList;

        public MainWindow()
        {
            engine = new SaperEngine();
            InitializeComponent();
            createBoard(15, 13, 14);
        }

        private void OnClick(object sender, EventArgs e)
        {
            int x = 0, y = 0;
            var button = (Button)sender;

            GetCord(button.Name, out x, out y);
            if (engine.BombField[y][x] == 0)
            {               
                OpenFields(x , y);
            }
            else
                button.Content = engine.BombField[y][x];        
        }
        
        private void OpenFields(int x, int y)
        {
          
            int xz = engine.BombField[0].Count();
            int yz = engine.BombField.Count();

            if (x < 0 || x >= engine.BombField[0].Count()) return;
            if (y < 0 || y >= engine.BombField.Count()) return;

            var button = ButtonList.SingleOrDefault(r => r.Name == "I" + y + "I" + x);
            if (button.Content == null) return;

            if (engine.BombField[y][x] == 0)
                button.Content = null;

            if (engine.BombField[y][x] != 9 && engine.BombField[y][x] != 0)
                button.Content = engine.BombField[y][x];

            if (engine.BombField[y][x] != 0) return;

            OpenFields(x - 1, y - 1);
            OpenFields(x - 1, y);
            OpenFields(x - 1, y + 1);
            OpenFields(x + 1, y - 1);
            OpenFields(x + 1, y);
            OpenFields(x + 1, y + 1);
            OpenFields(x, y - 1);
            OpenFields(x, y);
            OpenFields(x, y + 1);
        }

        private void GetCord(string name, out int x, out int y)
        {
            string[] Cord = name.Split('I');
            y = Convert.ToInt32(Cord[1]);
            x = Convert.ToInt32(Cord[2]);
        }

        private void createBoard(int x,int y, int Bombs)
        {
            Button pole;
            ButtonList = new List<Button>();
            Board.RowDefinitions.Clear();
            Board.ColumnDefinitions.Clear();
            Board.Children.Clear();

            for (int j = 0; j < x; j++)
                Board.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < y; i++)
                Board.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                {
                    pole = new Button();
                    pole.Background = Brushes.Gray;
                    pole.Name ="I" + i + "I" + j;
                    pole.SetValue(Grid.RowProperty, i);
                    pole.SetValue(Grid.ColumnProperty, j);
                    pole.Click += new RoutedEventHandler(OnClick);
                    pole.Content = pole.Name;
                    ButtonList.Add(pole);
                    Board.Children.Add(pole);
                    
                }
            engine.SetBoard(x, y, Bombs);
        }
    }
}
