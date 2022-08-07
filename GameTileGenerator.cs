using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public class GameTileGenerator
    {

        public GameTile getGameTileGenerator(string tyleType)
        {
            GameTile tile = null;
            return tile;
        }

        public int adjacentMines { get; set; }
        public bool isMine { get; set; }
        public bool isRevealed { get; set; }
        public bool isMarked { get; set; }
        public Button button { get; set; }
        public int row { get; set; }
        public int col { get; set; }   


        public GameTile()
        {
            isMine = false;
            isRevealed = false;
            isMarked = false;
            adjacentMines = 0;
            button = new Button();
        }

        public GameTile(int row, int col)
        {
            isMine = false;
            isRevealed = false;
            isMarked = false;
            adjacentMines = 0;
            button = new Button();
            this.row = row;
            this.col = col;
        }

        public static void setAdjacentMines(GameTile[,] board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int sum = 0;

                    sum = j < 9 && board[i, j + 1].isMine ? sum + 1 : sum;
                    sum = j > 0 && board[i, j - 1].isMine ? sum + 1 : sum;

                    sum = i < 9 && board[i + 1, j].isMine ? sum + 1 : sum;
                    sum = i < 9 && j < 9 && board[i + 1, j + 1].isMine ? sum + 1 : sum;
                    sum = i < 9 && j > 0 && board[i + 1, j - 1].isMine ? sum + 1 : sum;

                    sum = i > 0 && board[i - 1, j].isMine ? sum + 1 : sum;
                    sum = i > 0 && j < 9 && board[i - 1, j + 1].isMine ? sum + 1 : sum;
                    sum = i > 0 && j > 0 && board[i - 1, j - 1].isMine ? sum + 1 : sum;

                    board[i, j].adjacentMines = sum;

                } // for j
            } // for i
        } // set adjacentMines

        public static GameTile getTile(List<GameTile> tiles, int row, int col)
        {
            foreach(GameTile tile in tiles)
            {
                if (tile.row == row && tile.col == col)
                    return tile;
            }
            return null;
        }

        public static void setMines(GameTile[,] board)
        {
            var rand = new Random();

            int mineCount = 0;

            do
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (rand.Next(101) > 90 && mineCount < 15)
                        {
                            board[i, j].isMine = true;
                            mineCount++;
                        }
                            
                    } // for j
                } // for i

            } while (mineCount < 15);

        } // setMines

        public static void clearMines(GameTile[,] board)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j].isMine = false;
                    board[i, j].adjacentMines = 0;
                    board[i, j].isMarked = false;
                    board[i, j].isRevealed = false;
                } // for j
            } // for i
        }

    } // GameTile
}
