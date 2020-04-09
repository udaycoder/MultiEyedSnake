using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class enemy : Tank
    {
        public enemy(int boardMaxX,int boardMaxY, String[,] board) : base(boardMaxX,boardMaxY)
        {
            type = 1; //type of enemy
            orientation = rand.Next() % 4; // value ranging from 0 to 7 in random to denote directions starting from North edge then clockwise

            while (getCenter() == null)
            {
                int spawn = rand.Next() % 8;  // random value ranging from 0 to 4 to denote orientation of tank starting from North to clockwise
                //set centres for all 8 random positions possible
                if (spawn == 0 && isfullTankValid(new Tuple<int, int>(1, boardMaxY / 2), board))
                {
                    setCenter(new Tuple<int, int>(1, boardMaxY / 2));
                }
                if (spawn == 1 && isfullTankValid(new Tuple<int, int>(1, boardMaxY - 2), board))
                {
                    setCenter(new Tuple<int, int>(1, boardMaxY - 2));
                }
                if (spawn == 2 && isfullTankValid(new Tuple<int, int>(boardMaxX / 2, boardMaxY - 2), board))
                {
                    setCenter(new Tuple<int, int>(boardMaxX / 2, boardMaxY - 2));
                }
                if (spawn == 3 && isfullTankValid(new Tuple<int, int>(boardMaxX - 2, boardMaxY - 2), board))
                {
                    setCenter(new Tuple<int, int>(boardMaxX - 2, boardMaxY - 2));
                }
                if (spawn == 4 && isfullTankValid(new Tuple<int, int>(boardMaxX - 2, boardMaxY / 2), board))
                {
                    setCenter(new Tuple<int, int>(boardMaxX - 2, boardMaxY / 2));
                }
                if (spawn == 5 && isfullTankValid(new Tuple<int, int>(boardMaxX - 2, 1), board))
                {
                    setCenter(new Tuple<int, int>(boardMaxX - 2, 1));
                }
                if (spawn == 6 && isfullTankValid(new Tuple<int, int>(boardMaxX / 2, 1), board))
                {
                    setCenter(new Tuple<int, int>(boardMaxX / 2, 1));
                }
                if (spawn == 7 && isfullTankValid(new Tuple<int, int>(1, 1), board))
                {
                    setCenter(new Tuple<int, int>(1, 1));
                }

            }
        }

        public override int getType()
        {
            return type;
        }

        public override void TakeControls(String[,] board)
        {
            int dir = rand.Next() % 5;
            if (dir == 0)
                moveUp(board);
            if (dir == 1)
                moveRight(board);
            if (dir == 2)
                moveDown(board);
            if (dir == 3)
                moveLeft(board);
            if (dir == 4)
                shoot();
        }
    }
}
