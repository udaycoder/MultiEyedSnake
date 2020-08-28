using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEyedSnake
{
    class player : Tank
    {
        private static player singleton_player;
        private player(int boardMaxX,int boardMaxY) : base(boardMaxX,boardMaxY)
        { 
            type = 0; //type for player
            setOrientation(0);
        }

        public static player get_instance(int rowSize,int colSize)
        {
            if(singleton_player==null)
            {
                singleton_player = new player(rowSize, colSize);
            }
            return singleton_player;
        }

        public override int getType()
        {
            return type;
        }

        public void TakeControls(Windows.System.VirtualKey k, ref String[,] board)
        {
            if (k == Windows.System.VirtualKey.Left)
            {
                moveLeft(ref board);
            }
            if (k == Windows.System.VirtualKey.Right)
            {
                moveRight(ref board);
            }
            if (k == Windows.System.VirtualKey.Down)
            {
                moveDown(ref board);
            }
            if (k == Windows.System.VirtualKey.Up)
            {
                moveUp(ref board);
            }
            if (k == Windows.System.VirtualKey.Control)
            {
                shoot();
            }
        }

        public override void TakeControls(ref String[,] board)
        {
            
        }
    }
}
