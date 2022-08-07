using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            newGame();
        }

        public static void newGame()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new Form1());

            // old shit
            /*
            GameTile[,] board = new GameTile[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = new GameTile();
                }
            }

            GameTile.setMines(board);
            GameTile.setAdjacentMines(board);
            */

            // Application.Run(new Form1(board));
        }

    }
}
