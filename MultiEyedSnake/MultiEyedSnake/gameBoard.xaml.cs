using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
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
        Dictionary<String, Tank> tankDict = new Dictionary<String, Tank>();
        Rectangle rect;
        String[,] battleGround;
        player playerTank;
        Windows.UI.Color enemyColor, playerColor, borderColor, fillColor;
        List<enemy> enemyList;
        List<cannon> cannonList;
        int maxEnemies;
        Random rand;
        int score;

        public gameBoard()
        {
            score = 0;
            rowSize = 20;
            colSize = 10;
            maxEnemies = 4;
            enemyList = new List<enemy>();
            cannonList = new List<cannon>();

            this.InitializeComponent();

            rand = new Random();

            Score_Value.Text = " " + Convert.ToString(score);

            //setting colors
            enemyColor = Windows.UI.Colors.Navy;
            playerColor = Windows.UI.Colors.Green;
            borderColor = Windows.UI.Colors.Red;
            fillColor = Windows.UI.Colors.Black;


            playerTank = new player(rowSize,colSize);
            tankDict.Add(playerTank.getId(), playerTank);
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

            battleGround = new String[rowSize, colSize];
            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < colSize; j++)
                    battleGround[i, j] = "";

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
            setBattleGroundTank(borderColor, fillColor, findPosOrient(t.getCenter(), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 - 1), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 + 1), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 - 1), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 + 1), t.getCenter(), orientation),"");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 - 1), t.getCenter(), orientation), "");
            setBattleGroundTank(borderColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 + 1), t.getCenter(), orientation), "");
            tankDict.Remove(t.getId());
            t = null;
        }


        private void setBattleGroundTank(Windows.UI.Color border, Windows.UI.Color fill, Tuple<int,int> point,String id)
        {
            colorCell(border, fill, point);
            battleGround[point.Item1, point.Item2] = id;
        }

        private void renderShot(cannon c)
        {
            colorCell(Windows.UI.Colors.Gray, Windows.UI.Colors.Gray, c.getPos());
            if(battleGround[c.getPos().Item1, c.getPos().Item2] != "")
            {
                if(battleGround[c.getPos().Item1, c.getPos().Item2] != "3")
                {
                    tankDict[battleGround[c.getPos().Item1, c.getPos().Item2]].setAlive(false);
                    unrenderTank(tankDict[battleGround[c.getPos().Item1, c.getPos().Item2]]);
                    if(c.getLaunchedBy()==0)
                    {
                        score += 10;  //palyer score logic
                        Score_Value.Text = " " + Convert.ToString(score);
                    }
                }
                unrenderShot(c);
                c.setAlive(false);
            }
            else
            {
                battleGround[c.getPos().Item1, c.getPos().Item2] = c.getType().ToString();   // using type in battleground for cannon and not id like in tanks,
                                                                                             // helps in logic also there is not need for id here
            }
        }

        private void unrenderShot(cannon c)
        {
            colorCell(borderColor, fillColor, c.getPos());
            battleGround[c.getPos().Item1, c.getPos().Item2] = "";
        }

        private void renderTank(Tank t)
        {
            int orientation = t.getOrientation();
            if (t.getType() == 1)
            {
                
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(t.getCenter(), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 - 1), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2 + 1), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 - 1), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(fillColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(enemyColor, enemyColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2 + 1), t.getCenter(), orientation),t.getId());
                setBattleGroundTank(fillColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 - 1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(fillColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 + 1), t.getCenter(), orientation), t.getId());
            }
            else if(t.getType()==0)
            {
                
                setBattleGroundTank(playerColor, playerColor, findPosOrient(t.getCenter(),t.getCenter(),orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int,int>(t.getCenter().Item1 - 1,t.getCenter().Item2), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2-1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1, t.getCenter().Item2+1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2-1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(playerColor, playerColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 + 1, t.getCenter().Item2+1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(fillColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 - 1), t.getCenter(), orientation), t.getId());
                setBattleGroundTank(fillColor, fillColor, findPosOrient(new Tuple<int, int>(t.getCenter().Item1 - 1, t.getCenter().Item2 + 1), t.getCenter(), orientation), t.getId());
            }
            tankDict[t.getId()] = t;
            t = null;

        }

        private async void startGameAsync()
        {
            while (playerTank.isAlive())
            {
                await Task.Delay(500);
                int canCount = 0;
                canCount = cannonList.Count();
                for(int i = 0; i < canCount; i++)
                {
                    if(cannonList[i].getPos().Item1<0 || cannonList[i].getPos().Item1>= rowSize || cannonList[i].getPos().Item2 < 0 || cannonList[i].getPos().Item2 >= colSize)
                    {
                        cannonList[i].setAlive(false);
                        cannonList.Remove(cannonList[i]);
                        i--;
                        canCount = cannonList.Count();
                        continue;
                    }
                    if (cannonList[i].getAlive())
                    {
                        unrenderShot(cannonList[i]);
                        cannonList[i].nextPos();
                        if (cannonList[i].getPos().Item1 >= 0 && cannonList[i].getPos().Item1 < rowSize && cannonList[i].getPos().Item2 >= 0 && cannonList[i].getPos().Item2 < colSize)
                            renderShot(cannonList[i]);
                    }
                    else
                    {
                        unrenderShot(cannonList[i]);
                        cannonList.Remove(cannonList[i]);
                        i--;
                        canCount = cannonList.Count();
                        continue;
                    }
                }
                renderTank(playerTank);
                int numberOfEnemiesToRender = maxEnemies - enemyList.Count();
                for (int i = 0; i < numberOfEnemiesToRender; i++)
                {
                    enemy newenemy = new enemy(rowSize, colSize, ref battleGround);
                    renderTank(newenemy);
                    enemyList.Add(newenemy);
                }
                int listOfEnemies = enemyList.Count();
                for (int i = 0; i < listOfEnemies; i++)
                {
                    if (enemyList[i].isAlive())
                    {
                        unrenderTank(enemyList[i]);
                        enemyList[i].TakeControls(ref battleGround);
                        if (enemyList[i].getReadyToFire())
                        {
                            cannon shot = new cannon(enemyList[i].getOrientation(), enemyList[i].getCenter());
                            shot.setLaunchedBy(1);
                            if (shot.getPos().Item1 >= 0 && shot.getPos().Item1 < rowSize && shot.getPos().Item2 >= 0 && shot.getPos().Item2 < colSize)
                            {
                                cannonList.Add(shot);
                                renderShot(shot);
                            }
                            enemyList[i].setReadyToFire(false);
                        }
                        renderTank(enemyList[i]);
                    }
                    else
                    {
                        enemyList.Remove(enemyList[i]);
                        i--;
                        listOfEnemies = enemyList.Count();
                        continue;
                    }
                }
            }

            //Game Over Phase
            CleanUpBoard();
        }

        private void CleanUpBoard()
        {
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < colSize; j++)
                {
                    colorCell(borderColor, fillColor, new Tuple<int, int>(i, j));
                    battleGround[i, j] = "";
                }
            }

            cannonList.Clear();
            enemyList.Clear();
            tankDict.Clear();
            score = 0;
        }

        private void resetGame(object sender, RoutedEventArgs e)
        {
            if (!playerTank.isAlive())   // Currently can only reset game while game is not active
            {
                CleanUpBoard();
                playerTank = null;
                Frame MainFrame = Window.Current.Content as Frame;
                Window.Current.Content = MainFrame;
                MainFrame.Navigate(typeof(MainPage), e);
                Window.Current.Activate();
            }
        }

        private async void KeyDownHelper(object sender, KeyRoutedEventArgs e) // Control Player tank actions when user presses button 
        {
            unrenderTank(playerTank);
            playerTank.TakeControls(e.Key,ref battleGround);
            if (playerTank.getReadyToFire())
            {
                cannon shot = new cannon(playerTank.getOrientation(), playerTank.getCenter());
                shot.setLaunchedBy(0);
                if (shot.getPos().Item1 >= 0 && shot.getPos().Item1 < rowSize && shot.getPos().Item2 >= 0 && shot.getPos().Item2 < colSize)
                {
                    cannonList.Add(shot);
                    renderShot(shot);
                }
                playerTank.setReadyToFire(false);
            }
            renderTank(playerTank);
            await Task.Delay(500);

        }
    }
}
