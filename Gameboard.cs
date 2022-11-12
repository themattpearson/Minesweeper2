using System;
using System.Collections.Generic;
using System.Drawing;

namespace Minesweeper
{
    public class Gameboard
    {
        private int _numTiles;
        private int _numRows;
        private int _numCols;

        private int _numMines;
        private GameTile[,] _gameTiles;

        public Gameboard(int numTiles, int numMines)
        {
            _numTiles = numTiles;
            _numMines = numMines;
            _numCols = numTiles;
            _numRows = numTiles;

            _gameTiles = new GameTile[_numTiles, _numTiles];

            for (int i = 0; i < _numTiles; i++)
            {
                for (int j = 0; j < _numTiles; j++)
                {
                    _gameTiles[i, j] = new GameTile(i, j);
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

        public GameTile GetGameTileAtLocation(int row, int col)
        {
            if (IsOutOfBounds(row, col) == false)
                return _gameTiles[row, col];

            return new GameTile();
        }

        public int GetNumTiles()
        {
            return _numTiles;
        }

        public int GetNumMines()
        {
            return _numMines;
        }

        private bool IsOutOfBounds(int row, int col)
        {
            if (row < 0)
                return true;

            if (col < 0)
                return true;

            if (col > _numCols - 1)
                return true;

            if (row > _numRows - 1)
                return true;

            return false;
        }

        public void SetAdjacentMines()
        {
            List<GameTile> adjacentTiles = new List<GameTile>();
            int currentRow; 
            int currentCol;

            foreach (GameTile tile in _gameTiles)
            {
                currentRow = tile.GetRow();
                currentCol = tile.GetCol();                    

                for (int i = currentRow - 1; i <= currentRow + 1; i++)
                {
                    for (int j = currentCol - 1; j <= currentCol + 1; j++)
                    {
                        if (IsOutOfBounds(i, j) == false && _gameTiles[i, j].GetIsMine() == true)
                        {
                            tile.IncrementAdjacentMines();
                        }   
                    }
                }
            }
        }

        public void FloodZeroes(GameTile tile)
        {
            int x = tile.GetRow();
            int y = tile.GetCol();

            int xLowerBound = 0;
            int yLowerBound = 0;

            int xUpperBound = _numRows - 1;
            int yUpperBound = _numCols - 1;

            GameTile currentTile;

            Queue<Point> s = new Queue<Point>();
            Point p = new Point(x, y);
            s.Enqueue(p);

            while (s.Count > 0)
            {
                p = s.Dequeue();

                Console.WriteLine(p);

                currentTile = _gameTiles[p.X, p.Y];

                currentTile.SetIsRevealed(true);

                if (currentTile.GetAdjacentMines() == 0)
                {
                    if (p.X > xLowerBound && !_gameTiles[p.X - 1, p.Y].GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y));
                    if (p.X < xUpperBound && !_gameTiles[p.X + 1, p.Y].GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y));
                    if (p.Y > yLowerBound && !_gameTiles[p.X, p.Y - 1].GetIsRevealed()) s.Enqueue(new Point(p.X, p.Y - 1));
                    if (p.Y < yUpperBound && !_gameTiles[p.X, p.Y + 1].GetIsRevealed()) s.Enqueue(new Point(p.X, p.Y + 1));

                    if (p.X < xUpperBound && p.Y < yUpperBound && !_gameTiles[p.X + 1, p.Y + 1].GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y + 1));
                    if (p.X > xLowerBound && p.Y > yLowerBound && !_gameTiles[p.X - 1, p.Y - 1].GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y - 1));
                    if (p.X > xLowerBound && p.Y < yUpperBound && !_gameTiles[p.X - 1, p.Y + 1].GetIsRevealed()) s.Enqueue(new Point(p.X - 1, p.Y + 1));
                    if (p.X < xUpperBound && p.Y > yLowerBound && !_gameTiles[p.X + 1, p.Y - 1].GetIsRevealed()) s.Enqueue(new Point(p.X + 1, p.Y - 1));
                }
            }
        }
        public void PlaceMines()
        {
            var rand = new Random();
            int placedMines = 0;
            GameTile tile;

            do
            {
                int xCoordinate = rand.Next(0, _numCols);
                int yCoordinate = rand.Next(0, _numRows);

                tile = _gameTiles[xCoordinate, yCoordinate];
                tile.SetIsMine(true);

                placedMines++;

            } while (placedMines <= _numMines);
        }


    }
}
