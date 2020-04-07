using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class player : Tank
    {
        public player()
        {
            type = 0; //type for player
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
                moveRight();
            }
            if (k == Windows.System.VirtualKey.Up)
            {
                moveRight();
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
