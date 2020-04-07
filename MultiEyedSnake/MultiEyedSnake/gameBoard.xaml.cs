using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public gameBoard()
        {
            rowSize = 20;
            colSize = 10;
            this.InitializeComponent();

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
                    rect = new Rectangle();
                    rect.Stroke = new SolidColorBrush(Windows.UI.Colors.Red);
                    rect.StrokeThickness = 1;
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    rect.Fill = new SolidColorBrush(Windows.UI.Colors.Black);
                    AnswerGrid.Children.Add(rect);
                    _rectDict[i * colSize + j] = rect;
                }
            }

            battleGround = new int[rowSize, colSize];
            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < colSize; j++)
                    battleGround[i, j] = 0;

            startGame();
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

        private void startGame()
        {
            
        }

        private void KeyDownHelper(object sender, KeyRoutedEventArgs e) // Control Player tank actions when user presses button 
        {
            
        }
    }
}
