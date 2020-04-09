using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    abstract class Tank
    {
        protected int orientation; //can take value from 0-3 to indicate N,E,S,W ie clockwise directions
        protected bool readyToFire; //is tank ready to shoot, will shoot at next opportunity
        protected Tuple<int, int> center; //center of tank around which the entire tank will be based
        protected int type;    // type of tank -> 0 for player and 1 for enemy tank
        protected bool alive;  // boolean variable to indicate if Tank is alive or not
        protected int maxX, maxY; //indicates the maximum x and y coordinates that the Tank is allowed to operate in
        protected int minX, minY; //indicates the minimum x and y coordinates that the Tank is allowed to operate in
        protected Random rand;  // A random variable that can be used in various applications
        protected String id;    // An id which can be used as a shorthand for identification in various scenarious

        public Tank(int boardMaxX, int boardMaxY)
        {
            alive = true;
            //minX and minY are one more than 0 keeping in mind center of tank
            minX = 1;
            minY = 1;
            // maxX and maxY are two less keeping in mind center of tank and index max being one less than board size
            maxX = boardMaxX - 2;
            maxY = boardMaxY - 2;

            rand = new Random(DateTime.Now.Millisecond);

            id = DateTime.Now.Millisecond.ToString();
        }

        public String getId()
        {
            return id;
        }

        public void setAlive(bool life)
        {
            alive = life;
        }

        public bool isAlive()
        {
            return alive;
        }

        public int getOrientation()
        {
            return orientation;
        }

        public bool getReadyToFire()
        {
            return readyToFire;
        }

        public Tuple<int,int> getCenter()
        {
            return center;
        }

        public void setOrientation(int o)
        {
            orientation = o;
        }

        public void setReadyToFire(bool fire)
        {
            readyToFire = fire;
        }

        public void setCenter(Tuple<int,int> c)
        {
            center = c;
        }

        public void shoot()
        {
            readyToFire = true;
        }

        protected Tuple<int, int> findPosOrient(Tuple<int, int> position, Tuple<int, int> center, int orientation)
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

        protected bool isBlank(Tuple<int,int> point,String[,] board)
        {
            int x = point.Item1;
            int y = point.Item2;
            if (x >= 0 && x <= maxX+1 && y >= 0 && y <= maxY+1 && string.IsNullOrEmpty(board[x, y]))
                return true;
            else
                return false;
        }

        protected bool isfullTankValid(Tuple<int,int> center,String[,] board)
        {
            if (isBlank(findPosOrient(new Tuple<int, int>(center.Item1 - 1, center.Item2), center, orientation),board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1, center.Item2-1), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1, center.Item2), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1, center.Item2 + 1), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1+1, center.Item2 - 1), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1+1, center.Item2), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1+1, center.Item2 + 1), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1 - 1, center.Item2 - 1), center, orientation), board)
                && isBlank(findPosOrient(new Tuple<int, int>(center.Item1 - 1, center.Item2 + 1), center, orientation), board)
                )
                return true;
            else
                return false;
        }

        private bool IsValidPos(int x, int y, String[,] board)
        {
            if (x >= minX && x <= maxX && y >= minY && y <= maxY && isfullTankValid(new Tuple<int,int>(x,y),board))
                return true;
            else
                return false;
        }

        public void moveLeft(String[,] board)
        {
            if (orientation == 3 && IsValidPos(center.Item1, center.Item2 - 1,board))
                center = new Tuple<int,int>(center.Item1,center.Item2-1);
            else
                setOrientation(3);
        }

        public void moveRight(String[,] board)
        {
            if (orientation == 1 && IsValidPos(center.Item1, center.Item2 + 1,board))
                center = new Tuple<int, int>(center.Item1, center.Item2 + 1);
            else
                setOrientation(1);
        }

        public void moveUp(String[,] board)
        {
            if (orientation == 0 && IsValidPos(center.Item1 - 1, center.Item2,board))
                center = new Tuple<int, int>(center.Item1 - 1, center.Item2);
            else
                setOrientation(0);
        }

        public void moveDown(String[,] board)
        {
            if (orientation == 2 && IsValidPos(center.Item1 + 1, center.Item2,board))
                center = new Tuple<int, int>(center.Item1 + 1, center.Item2);
            else
                setOrientation(2);
        }

        public abstract void TakeControls(String[,] board);
        public abstract int getType();
    }
}
