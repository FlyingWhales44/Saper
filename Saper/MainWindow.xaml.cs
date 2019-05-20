using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SaperEngine engine;
        List<Button> ButtonList;

        private int bombQ;

        public MainWindow()
        {
            InitializeComponent();

            engine = new SaperEngine();        
            Start.Click += new RoutedEventHandler(SetNewGame);
            createBoard(Convert.ToInt32(BoardSizeX.Text), Convert.ToInt32(BoardSizeY.Text), Convert.ToInt32(BombQuantity.Text));

            bombQ = Convert.ToInt32(BombQuantity.Text);
         
        }

        private async Task CheckifWin()
        {
            int counter = 0;

            foreach (Button b in ButtonList)
            {
                if (b.Background == Brushes.Blue)
                    counter++;
            }

            if (bombQ == counter)
                MessageBox.Show("YOU WIN!");
        }

        private void SetNewGame(object sender, RoutedEventArgs e)
        {
            createBoard(Convert.ToInt32(BoardSizeX.Text), Convert.ToInt32(BoardSizeY.Text), Convert.ToInt32(BombQuantity.Text));
            ButtonList.ForEach(r => r.IsEnabled = true);

            bombQ = Convert.ToInt32(BombQuantity.Text);
        }       

        private void OnClick(object sender, MouseEventArgs e)
        {          
            if (e.RightButton == MouseButtonState.Released)
            {
                int x = 0, y = 0;
                var button = (Button)sender;

                GetCord(button.Name, out x, out y);

                if (!engine.Flags[y][x])
                    return;

                if (engine.BombField[y][x] == 0)
                {
                    OpenFields(x, y);
                }
                else
                {
                    if (engine.BombField[y][x] == 9)
                    {
                        MessageBox.Show("GAME OVER");
                        FrozeAllButtons();                      
                    }
                    else
                    {
                        button.Content = engine.BombField[y][x];
                        button.Background = Brushes.Gray;
                    }
                }
            }
            if (e.LeftButton == MouseButtonState.Released)//zamień na flagi w silniku
            {
                Image img = new Image();

                img.Source = new BitmapImage(new Uri(@"pack://application:,,,/Saper;component/flag.png"));

                int x = 0, y = 0;
                var button = (Button)sender;

                GetCord(button.Name, out x, out y);

                bool move = true;

                if (!engine.Flags[y][x] && move && button.Background != Brushes.Gray)
                {
                    engine.Flags[y][x] = true;
                    button.Content = null;
                    move = false;
                }
                if (engine.Flags[y][x] && button.Background!=Brushes.Gray && move)
                {
                    engine.Flags[y][x] = false;
                    button.Content = img;
                }

                move = true;
            }
            CheckifWin().Wait();
        }

        private void FrozeAllButtons()
        {
            ButtonList.ForEach(r=>r.IsEnabled = false);
        }
        
        private void OpenFields(int x, int y)
        {
          
            int xz = engine.BombField[0].Count();
            int yz = engine.BombField.Count();

            if (x < 0 || x >= engine.BombField[0].Count()) return;
            if (y < 0 || y >= engine.BombField.Count()) return;

            var button = ButtonList.SingleOrDefault(r => r.Name == "I" + y + "I" + x);
            if (button.Background == Brushes.Gray) return;

            if (engine.BombField[y][x] == 0)
                button.Background = Brushes.Gray;

            if (engine.BombField[y][x] != 9 && engine.BombField[y][x] != 0)
            {
                button.Background = Brushes.Gray;
                button.Content = engine.BombField[y][x];
            }

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
                    pole.Background = Brushes.Blue;//zakryte darkblue
                    pole.Name ="I" + i + "I" + j;
                    pole.SetValue(Grid.RowProperty, i);
                    pole.SetValue(Grid.ColumnProperty, j);
                    pole.PreviewMouseDown += new MouseButtonEventHandler(OnClick);
                    ButtonList.Add(pole);
                    Board.Children.Add(pole);
                    
                }
            engine.SetBoard(x, y, Bombs);
        }

    }
}
