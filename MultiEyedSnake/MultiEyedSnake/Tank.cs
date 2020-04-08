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
        protected int readyToFire; //is tank ready to shoot, will shoot at next opportunity, 1 means ready and 0 is not
        protected Tuple<int, int> center; //center of tank around which the entire tank will be based
        protected int type;    // type of tank -> 0 for player and 1 for enemy tank
        protected bool alive;  // boolean variable to indicate if Tank is alive or not
        protected int maxX, maxY; //indicates the maximum x and y coordinates that the Tank is allowed to operate in
        protected int minX, minY; //indicates the minimum x and y coordinates that the Tank is allowed to operate in

        public Tank(int boardMaxX, int boardMaxY)
        {
            alive = true;
            //minX and minY are one more than 0 keeping in mind center of tank
            minX = 1;
            minY = 1;
            // maxX and maxY are two less keeping in mind center of tank and index max being one less than board size
            maxX = boardMaxX - 2;
            maxY = boardMaxY - 2; 
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

        public int getReadyToFire()
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

        public void setReadyToFire(int fire)
        {
            readyToFire = fire;
        }

        public void setCenter(Tuple<int,int> c)
        {
            center = c;
        }

        public void shoot()
        {
            readyToFire = 1;
        }

        private bool IsValidPos(int x, int y)
        {
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                return true;
            else
                return false;
        }

        public void moveLeft()
        {
            if (orientation == 3 && IsValidPos(center.Item1, center.Item2 - 1))
                center = new Tuple<int,int>(center.Item1,center.Item2-1);
            else
                setOrientation(3);
        }

        public void moveRight()
        {
            if (orientation == 1 && IsValidPos(center.Item1, center.Item2 + 1))
                center = new Tuple<int, int>(center.Item1, center.Item2 + 1);
            else
                setOrientation(1);
        }

        public void moveUp()
        {
            if (orientation == 0 && IsValidPos(center.Item1 - 1, center.Item2))
                center = new Tuple<int, int>(center.Item1 - 1, center.Item2);
            else
                setOrientation(0);
        }

        public void moveDown()
        {
            if (orientation == 2 && IsValidPos(center.Item1 + 1, center.Item2))
                center = new Tuple<int, int>(center.Item1 + 1, center.Item2);
            else
                setOrientation(2);
        }

        public abstract void TakeControls();
        public abstract int getType();
    }
}
