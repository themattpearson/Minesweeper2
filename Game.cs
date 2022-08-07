using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Game
    {
        private Gameboard _gameboard;

        private readonly int _hardDifficultyTiles = 20;
        private readonly int _hardDifficultyMines = 30;

        private readonly int _mediumDifficultyTiles = 15;
        private readonly int _mediumDifficultyMines = 20;

        private readonly int _easyDifficultyTiles = 10;
        private readonly int _easyDifficultyMines = 10;

        public Game(string difficulty)
        {
            _gameboard = new Gameboard(difficulty);
        }

        private void StartRound()
        {

        }

        public Gameboard GetGameboard()
        {
            return _gameboard;
        }

    }
}
