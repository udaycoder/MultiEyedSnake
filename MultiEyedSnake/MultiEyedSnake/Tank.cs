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

        public void moveLeft()
        {
            if (orientation == 3)
                center = new Tuple<int,int>(center.Item1,center.Item2-1);
            else
                orientation = 3;
        }

        public void moveRight()
        {
            if (orientation == 1)
                center = new Tuple<int, int>(center.Item1, center.Item2 + 1);
            else
                orientation = 1;
        }

        public void moveUp()
        {
            if (orientation == 0)
                center = new Tuple<int, int>(center.Item1 - 1, center.Item2);
            else
                orientation = 0;
        }

        public void moveDown()
        {
            if (orientation == 2)
                center = new Tuple<int, int>(center.Item1 + 1, center.Item2);
            else
                orientation = 2;
        }

        public abstract void TakeControls();
        public abstract int getType();
    }
}
