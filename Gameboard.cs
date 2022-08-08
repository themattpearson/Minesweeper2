using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
    public class Gameboard
    {
        private int _numTiles;
        private int _numRows;
        private int _numCols;

        private int _numMines;
        private GameTile[,] _gameTiles;


        // for custom games
        public Gameboard(int numRows, int numCols, int numMines)
        {

        }



        public Gameboard(int numTiles, int numMines)
        {
            _numTiles = numTiles;
            _numMines = numMines;

            CreateEmptyBoard();
        }

        private void CreateEmptyBoard()
        {
            _gameTiles = new GameTile[_numTiles, _numTiles];

            for (int i = 0; i < _numTiles; i++)
            {
                for (int j = 0; j < _numTiles; j++)
                {
                    _gameTiles[i, j] = new GameTile(i,j); 
                }
            }
        }

        public void ResetBoard()
        {
            _gameTiles = null;
        }

        public GameTile[,] GetGameTiles()
        {
            return _gameTiles;
        }

        public GameTile GetGameTile(int row, int col)
        {
            return _gameTiles[row, col];
        }

        public int GetNumTiles()
        {
            return _numTiles;
        }

        public int GetNumMines()
        {
            return _numMines;
        }

        private void UpdateBoard()
        {

        }

        public List<GameTile> GetAdjacentGameTiles(GameTile tile)
        {
            List<GameTile> returnList = new List<GameTile>();

            int currentColumn = tile.GetCol();
            int currentRow = tile.GetRow();

            int gameBoardHeight = _numTiles - 1;
            int gameBoardWidth = _numTiles - 1;

            bool isOnTopEdge = currentRow == 0;
            bool isOnLeftEdge = currentColumn == 0;
            bool isOnBottomEdge = currentRow == gameBoardHeight;
            bool isOnRightEdge = currentColumn == gameBoardWidth;

            int aboveOfCurrent = currentRow - 1;
            int belowCurrent = currentRow + 1;
            int rightOfCurrent = currentColumn + 1;
            int leftOfCurrent = currentColumn - 1;

            if (isOnTopEdge == false)
            {
                returnList.Add(GetGameTile(aboveOfCurrent, currentColumn));
            }
            if (!isOnBottomEdge)
            {
                returnList.Add(GetGameTile(belowCurrent, currentColumn));
            }
            if (!isOnLeftEdge)
            {
                returnList.Add(GetGameTile(currentRow, leftOfCurrent));
            }
            if (!isOnRightEdge)
            {
                returnList.Add(GetGameTile(currentRow, rightOfCurrent));
            }
            if (!isOnTopEdge && !isOnLeftEdge)
            {
                returnList.Add(GetGameTile(aboveOfCurrent, leftOfCurrent));
            }
            if (!isOnTopEdge && !isOnRightEdge)
            {
                returnList.Add(GetGameTile(aboveOfCurrent, rightOfCurrent));
            }
            if (!isOnBottomEdge && !isOnLeftEdge)
            {
                returnList.Add(GetGameTile(belowCurrent, leftOfCurrent));
            }
            if (!isOnBottomEdge && !isOnRightEdge)
            {
                returnList.Add(GetGameTile(belowCurrent, rightOfCurrent));
            }

            return returnList;
        }

        public void FloodZeroes(GameTile tile)
        {
            int x = tile.GetRow();
            int y = tile.GetCol();

            GameTile currentTile;

            Queue<Point> s = new Queue<Point>();
            Point p = new Point(x, y);
            s.Enqueue(p);

            while (s.Count > 0)
            {
                p = s.Dequeue();

                Console.WriteLine(p);

                currentTile = GetGameTile(p.X, p.Y);

                currentTile.SetIsRevealed(true);

                if (currentTile.GetAdjacentMines() == 0)
                {
                    if (p.X > 0 && !GetGameTile(p.X - 1, p.Y).GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y));
                    if (p.X < 9 && !GetGameTile(p.X + 1, p.Y).GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y));
                    if (p.Y > 0 && !GetGameTile(p.X, p.Y - 1).GetIsRevealed()) s.Enqueue(new Point(p.X, p.Y - 1));
                    if (p.Y < 9 && !GetGameTile(p.X, p.Y + 1).GetIsRevealed()) s.Enqueue(new Point(p.X, p.Y + 1));

                    if (p.X < 9 && p.Y < 9 && !GetGameTile(p.X + 1, p.Y + 1).GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y + 1));
                    if (p.X > 0 && p.Y > 0 && !GetGameTile(p.X - 1, p.Y - 1).GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y - 1));
                    if (p.X > 0 && p.Y < 9 && !GetGameTile(p.X - 1, p.Y + 1).GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y + 1));
                    if (p.X < 9 && p.Y > 0 && !GetGameTile(p.X + 1, p.Y - 1).GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y - 1));

                }
            }
        }

        public void SeedGameboard()
        {
            var rand = new Random();
            int placedMines = 0;
            int mineProbability = 90;

            do
            {
                for (int i = 0; i < _numTiles; i++)
                {
                    for (int j = 0; j < _numTiles; j++)
                    {
                        if (rand.Next(101) > mineProbability && placedMines < _numMines)
                        {
                            _gameTiles[i, j].SetIsMine(true);
                            placedMines++;
                        }
                    }
                }

            } while (placedMines < _numMines);

            SetAdjacentMines();
        }

        public void SetAdjacentMines()
        {
            List<GameTile> adjacentTileList = new List<GameTile>();

            foreach(GameTile tile in _gameTiles)
            {
                adjacentTileList = GetAdjacentGameTiles(tile);

                int sum = 0;

                foreach (GameTile adjacentTile in adjacentTileList)
                {
                    if (adjacentTile.GetIsMine() == true)
                    {
                        sum++;
                    }
                }
                tile.SetAdjacentMines(sum);
            }

            /*
            int borderValue = _numTiles - 1;

            for (int i = 0; i < _numTiles; i++)
            {
                for (int j = 0; j < _numTiles; j++)
                {
                    int sum = 0;

                    sum = j < borderValue && _gameTiles[i, j + 1].GetIsMine() ? sum + 1 : sum;
                    sum = j > 0 && _gameTiles[i, j - 1].GetIsMine() ? sum + 1 : sum;

                    sum = i < borderValue && _gameTiles[i + 1, j].GetIsMine() ? sum + 1 : sum;
                    sum = i < borderValue && j < borderValue && _gameTiles[i + 1, j + 1].GetIsMine() ? sum + 1 : sum;
                    sum = i < borderValue && j > 0 && _gameTiles[i + 1, j - 1].GetIsMine() ? sum + 1 : sum;

                    sum = i > 0 && _gameTiles[i - 1, j].GetIsMine() ? sum + 1 : sum;
                    sum = i > 0 && j < borderValue && _gameTiles[i - 1, j + 1].GetIsMine() ? sum + 1 : sum;
                    sum = i > 0 && j > 0 && _gameTiles[i - 1, j - 1].GetIsMine() ? sum + 1 : sum;

                    _gameTiles[i, j].SetAdjacentMines(sum);

                }
            }
            */
        }

        public void ClearMines()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _gameTiles[i, j].ResetGameTile();
                }
            }
        }

    }
}
