using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Gameboard
    {
        private int _numTiles;
        private int _numMines;
        private GameTile[,] _gameTiles;

        private readonly int _hardDifficultyTiles = 20;
        private readonly int _hardDifficultyMines = 30;

        private readonly int _mediumDifficultyTiles = 15;
        private readonly int _mediumDifficultyMines = 20;

        private readonly int _easyDifficultyTiles = 10;
        private readonly int _easyDifficultyMines = 10;

        public Gameboard(string difficulty)
        {

            if(difficulty.ToLower() == "hard")
            {
                SetHardSettings();
            }
            if (difficulty.ToLower() == "medium")
            {
                SetMediumSettings();
            }
            if (difficulty.ToLower() == "easy")
            {
                SetEasySettings();
            }

        }

        private void SetHardSettings()
        {
            _numTiles = _hardDifficultyTiles;
            _numMines = _hardDifficultyMines;
        }

        private void SetMediumSettings()
        {
            _numTiles = _mediumDifficultyTiles;
            _numMines = _mediumDifficultyMines;
        }

        private void SetEasySettings()
        {
            _numTiles = _easyDifficultyTiles;
            _numMines = _easyDifficultyMines;
        }

        private void CreateEmptyBoard()
        {
            _gameTiles = new GameTile[_numTiles, _numTiles];
        }

        private void ResetBoard()
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

        private void UpdateBoard()
        {

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
        }

        public void SetAdjacentMines()
        {
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
