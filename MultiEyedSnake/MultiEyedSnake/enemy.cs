using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class enemy : Tank
    {
        public enemy()
        {
            type = 1; //type of enemy
        }
        public override int getType()
        {
            return type;
        }

        public override void TakeControls()
        {
            Random rand = new Random();
            int dir = rand.Next() % 5;
            if (dir == 0)
                moveUp();
            if (dir == 1)
                moveRight();
            if (dir == 2)
                moveDown();
            if (dir == 3)
                moveLeft();
            if (dir == 4)
                shoot();
        }
    }
}
