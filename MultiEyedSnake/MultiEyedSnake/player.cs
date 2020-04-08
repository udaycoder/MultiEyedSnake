using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class player : Tank
    {
        public player(int boardMaxX,int boardMaxY) : base(boardMaxX,boardMaxY)
        { 
            type = 0; //type for player
            setOrientation(0);
        }

        public override int getType()
        {
            return type;
        }

        public void TakeControls(Windows.System.VirtualKey k)
        {
            if (k == Windows.System.VirtualKey.Left)
            {
                moveLeft();
            }
            if (k == Windows.System.VirtualKey.Right)
            {
                moveRight();
            }
            if (k == Windows.System.VirtualKey.Down)
            {
                moveDown();
            }
            if (k == Windows.System.VirtualKey.Up)
            {
                moveUp();
            }
            if (k == Windows.System.VirtualKey.Space)
            {
                shoot();
            }
        }

        public override void TakeControls()
        {
            
        }
    }
}
