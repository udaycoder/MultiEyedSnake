using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiEyedSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class gameBoard
    {
        int rowSize;
        int colSize;
        Dictionary<int, Rectangle> _rectDict = new Dictionary<int, Rectangle>();
        Rectangle rect;
        int[,] battleGround;
        player playerTank;
        Windows.UI.Color enemyColor, playerColor, borderColor, fillColor;

        public gameBoard()
        {
            rowSize = 20;
            colSize = 10;
            this.InitializeComponent();

            //setting colors
            enemyColor = Windows.UI.Colors.Violet;
            playerColor = Windows.UI.Colors.Green;
            borderColor = Windows.UI.Colors.Red;
            fillColor = Windows.UI.Colors.Black;


            playerTank = new player(rowSize,colSize);
            playerTank.setCenter(new Tuple<int, int>(rowSize / 2 - 1, colSize / 2 - 1));

            for (int i = 0; i < rowSize; i++)
            {
                RowDefinition row = new RowDefinition();

                AnswerGrid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < colSize; i++)
            {
                AnswerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //AnswerGrid.ShowGridLines = true;
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < colSize; j++)
                {
                    colorCell(borderColor, fillColor,new Tuple<int, int>(i,j));
                }
            }

            battleGround = new int[rowSize, colSize];
            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < colSize; j++)
                    battleGround[i, j] = 0;

            startGameAsync();
        }

        private void colorCell(Windows.UI.Color border, Windows.UI.Color fill,Tuple<int,int> pos)
        {
            rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(border);
            rect.StrokeThickness = 1;
            Grid.SetRow(rect, pos.Item1);
            Grid.SetColumn(rect, pos.Item2);
            rect.Fill = new SolidColorBrush(fill);
            AnswerGrid.Children.Add(rect);
            _rectDict[pos.Item1 * colSize + pos.Item2] = rect;
        }

        private Tuple<int,int> findPosOrient(Tuple<int, int> position, Tuple<int,int> center, int orientation)
        {
            int x = position.Item1;
            int y = position.Item2;

            int centerx = center.Item1;
            int centery = center.Item2;

            int l, m;

            for (int i = 1; i <= orientation; i++)
            {
                x -= centerx;
                y -= centery;

                l = (0 * x) + (1 * y);
                m = (-1 * x) + (0 * y);

                x = l + centerx;
                y = m + centery;

            }

            return new Tuple<int, int>(x, y);
        }

        private void unrenderTank(Tank t)
        {
            int orientation = t.getOrientation();
            colorCell(borderColor, fillColor, findPosOrient(t.getCenter(), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 - 1), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 + 1), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 - 1), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2), t.getCenter(), orientation));
            colorCell(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 + 1), t.getCenter(), orientation));
        }

        private void renderTank(Tank t)
        {
            if (t.getType() == 1)
            {

            }
            else if(t.getType()==0)
            {
                int orientation = t.getOrientation();
                colorCell(playerColor, playerColor, findPosOrient(t.getCenter(),t.getCenter(),orientation)  );
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int,int>(t.getCenter().Item1 - 1,t.getCenter().Item2), t.getCenter(), orientation));
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2-1), t.getCenter(), orientation));
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2+1), t.getCenter(), orientation));
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2-1), t.getCenter(), orientation));
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2), t.getCenter(), orientation));
                colorCell(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2+1), t.getCenter(), orientation));
            }
        }

        private int getSpawnPoint()  // returns a value ranging from 0 to 5 in random to denote directions starting from North edge then clockwise
        {
            int spawn = 0;
            Random rand = new Random();
            spawn = rand.Next() % 6;
            return spawn;
        }

        private int getOrientation()  // returns a random value ranging from 0 to 4 to denote orientation of tank starting from North to clockwise
        {
            int orient = 0;
            Random rand = new Random();
            orient = rand.Next() % 4;
            return orient;
        }

        private async void startGameAsync()
        {
            while (playerTank.isAlive())
            {
                await Task.Delay(500);
                renderTank(playerTank);
            }
        }

        private void KeyDownHelper(object sender, KeyRoutedEventArgs e) // Control Player tank actions when user presses button 
        {
            unrenderTank(playerTank);
            playerTank.TakeControls(e.Key);
            renderTank(playerTank);
            
        }
    }
}
