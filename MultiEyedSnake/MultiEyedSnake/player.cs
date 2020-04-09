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

        public void TakeControls(Windows.System.VirtualKey k, String[,] board)
        {
            if (k == Windows.System.VirtualKey.Left)
            {
                moveLeft(board);
            }
            if (k == Windows.System.VirtualKey.Right)
            {
                moveRight(board);
            }
            if (k == Windows.System.VirtualKey.Down)
            {
                moveDown(board);
            }
            if (k == Windows.System.VirtualKey.Up)
            {
                moveUp(board);
            }
            if (k == Windows.System.VirtualKey.Space)
            {
                shoot();
            }
        }

        public override void TakeControls(String[,] board)
        {
            
        }
    }
}
