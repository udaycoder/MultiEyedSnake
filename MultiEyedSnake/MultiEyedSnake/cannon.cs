using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class cannon
    {
        private Tuple<int, int> pos;
        private bool isAlive;
        private int orientation;
        private int launchedBy; // to store whether this is a cannon launched by player tank(0) or enemy tank(1)
        protected int type;
        protected String id;
        public cannon(int orient, Tuple<int, int> tankCenter)
        {
            orientation = orient;
            type = 3;
            id = DateTime.Now.Millisecond.ToString();
            if (orientation == 0)
            {
                pos = new Tuple<int, int>(tankCenter.Item1 - 2, tankCenter.Item2);
            }
            else if (orientation == 1)
            {
                pos = new Tuple<int, int>(tankCenter.Item1, tankCenter.Item2 + 2);
            }
            else if (orientation == 2)
            {
                pos = new Tuple<int, int>(tankCenter.Item1 + 2, tankCenter.Item2);
            }
            else if (orientation == 3)
            {
                pos = new Tuple<int, int>(tankCenter.Item1, tankCenter.Item2 - 2);
            }
            isAlive = true;
        }

        public int getLaunchedBy()
        {
            return launchedBy;
        }

        public void setLaunchedBy(int t)
        {
            launchedBy = t;
        }

        public String getId()
        {
            return id;
        }

        public int getType()
        {
            return type;
        }

        public void nextPos()
        {
            if (orientation == 0)
            {
                pos = new Tuple<int, int>(pos.Item1 - 1, pos.Item2);
            }
            else if (orientation == 1)
            {
                pos = new Tuple<int, int>(pos.Item1, pos.Item2 + 1);
            }
            else if (orientation == 2)
            {
                pos = new Tuple<int, int>(pos.Item1 + 1, pos.Item2);
            }
            else if (orientation == 3)
            {
                pos = new Tuple<int, int>(pos.Item1, pos.Item2 - 1);
            }
        }

        public void setAlive(bool alive)
        {
            isAlive = alive;
        }
        public bool getAlive()
        {
            return isAlive;
        }
        public Tuple<int, int> getPos()
        {
            return pos;
        }
    }
}
