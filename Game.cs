using System;

namespace Minesweeper
{
    public enum Difficulty { EASY, MEDIUM, HARD, CUSTOM };

    public class Game
    {
        private Gameboard _gameboard;
        
        private readonly int _hardDifficultyTiles = 20;
        private readonly int _hardDifficultyMines = 30;

        private readonly int _mediumDifficultyTiles = 15;
        private readonly int _mediumDifficultyMines = 20;

        private readonly int _easyDifficultyTiles = 10;
        private readonly int _easyDifficultyMines = 10;

        public Game(Enum difficultySetting)
        {
            switch (difficultySetting)
            {
                case Difficulty.HARD: 
                    HardGame();
                    break;
                case Difficulty.MEDIUM:
                    MediumGame();
                    break;
                case Difficulty.EASY:
                    EasyGame();
                    break;
                default:
                    EasyGame();
                    break;
            }    
        }

        private void HardGame()
        {
            _gameboard = new Gameboard(_hardDifficultyTiles, _hardDifficultyMines);
        }

        private void MediumGame()
        {
            _gameboard = new Gameboard(_mediumDifficultyTiles, _mediumDifficultyMines);
        }

        private void EasyGame()
        {
            _gameboard = new Gameboard(_easyDifficultyTiles, _easyDifficultyMines);
            _gameboard.SeedGameboard();
        }

        private void CustomGame(int rows, int cols, int mines)
        {

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
