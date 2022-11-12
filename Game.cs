namespace Minesweeper
{
    public class Game
    {
        private Gameboard _gameboard;

        private readonly int _hardDifficultyTiles = 17;
        private readonly int _hardDifficultyMines = 35;

        private readonly int _mediumDifficultyTiles = 12;
        private readonly int _mediumDifficultyMines = 15;

        private readonly int _easyDifficultyTiles = 10;
        private readonly int _easyDifficultyMines = 10;

        public Game(string difficulty)
        {
            switch (difficulty.ToLower())
            {
                case "easy":
                   _gameboard = new Gameboard(_easyDifficultyTiles, _easyDifficultyMines);
                    break;
                case "medium":
                    _gameboard = new Gameboard(_mediumDifficultyTiles, _mediumDifficultyMines);
                    break;
                case "hard":
                    _gameboard = new Gameboard(_hardDifficultyTiles, _hardDifficultyMines);
                    break;
                default:
                    _gameboard = new Gameboard(_easyDifficultyTiles, _easyDifficultyMines);
                    break;
            }

            _gameboard.PlaceMines();

            _gameboard.SetAdjacentMines();
        }

        public Gameboard GetGameboard()
        {
            return _gameboard;
        }

    }
}
