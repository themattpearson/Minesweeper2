using System;
using System.Drawing;
using System.Windows.Forms;
using static Minesweeper.Game;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Game _game;
        private Gameboard _gameboard;

        public Form1()
        {
            _game = new Game(Difficulty.EASY);
            _gameboard = _game.GetGameboard();

            InitializeComponent();
            CreateButtons();
        }

        public void CreateButtons()
        {
            int buttonSize = 40; // maybe should be related to board size?
            int boardSize = _gameboard.GetNumTiles();
            Color backColor = Color.LightGray;

            foreach(GameTile tile in _gameboard.GetGameTiles())
            {
                tile._button.Height = buttonSize;
                tile._button.Width = buttonSize;
                tile._button.BackColor = backColor;
                tile._button.Location = new Point(tile.GetRow() * buttonSize, tile.GetCol() * buttonSize);
                tile._button.MouseDown += new MouseEventHandler(Button_Click);
                Controls.Add(tile._button);
            }
        }

        private void PromptForNewGame(string result)
        {
            string message = "";
            string caption = "";

            if (result.Equals("loss"))
            {
                message = "Oof! Toughie.. please play again? UwU";
                caption = "Sad";
            }
            else
            {
                message = "Winner Winner, Chicken Dinner! Play again..?";
                caption = "Nice";
            }

            var prompt = MessageBox.Show(message, caption,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Exclamation);

            if (prompt == DialogResult.No)
            {
                this.Close();
            }
            if (prompt == DialogResult.Yes)
            {
                RestartGame();
            }
        }

        public void GameOver()
        {
            System.IO.Stream explosionSoundStream = Properties.Resources.explosion_x;
            System.Media.SoundPlayer explosionSound = new System.Media.SoundPlayer(explosionSoundStream);

            RevealMines();
            explosionSound.Play();
            PromptForNewGame("loss");
        }

        public void PlaceFlag(GameTile tile)
        {
            tile.SetIsMarked(true);

            tile._button.Font = new Font("Courier New", 20, FontStyle.Bold);
            tile._button.Text = "⚑";
            tile._button.BackColor = Color.Black;
            tile._button.ForeColor = Color.Yellow;
        }

        public void RemoveFlag(GameTile tile)
        {
            setButtonTextAndColor(tile, Color.LightGray, Color.LightGray, "");
            tile.SetIsMarked(false);
        }

        public void RevealTile(GameTile tile)
        {
            setButtonTextAndColor(tile, Color.DarkGray, Color.LightGray, tile.GetAdjacentMines().ToString());
            tile.SetIsRevealed(true);

            if (tile.GetAdjacentMines() == 0)
            {
                _gameboard.FloodZeroes(tile);
            }
            RefreshBoard();
        }

        public void RefreshBoard()
        {
            foreach(GameTile tile in _gameboard.GetGameTiles())
            {
                if(tile.GetIsRevealed() == true)
                {
                    Color backgroundColor = Color.LightGray;
                    Color textColor = Color.DarkGray;

                    tile._button.Font = new Font("Courier New", 20, FontStyle.Bold);
                    tile._button.BackColor = backgroundColor;
                    tile._button.ForeColor = textColor;

                    tile._button.Text = tile.GetAdjacentMines().ToString();

                    switch (tile.GetAdjacentMines())
                    {
                        case 0:
                            tile._button.ForeColor = textColor;
                            tile._button.BackColor = textColor;
                            tile._button.Text = "";
                            break;
                        case 1:
                            tile._button.ForeColor = Color.DarkBlue;
                            break;
                        case 2:
                            tile._button.ForeColor = Color.DarkGreen;
                            break;
                        case 3:
                            tile._button.ForeColor = Color.DarkRed;
                            break;
                        case 4:
                            tile._button.ForeColor = Color.DarkOrange;
                            break;
                        case 5:
                            tile._button.ForeColor = Color.DarkBlue;
                            break;
                        case 6:
                            tile._button.ForeColor = Color.DarkGreen;
                            break;
                        case 7:
                            tile._button.ForeColor = Color.DarkRed;
                            break;
                        case 8:
                            tile._button.ForeColor = Color.DarkOrange;
                            break;
                        default:
                            tile._button.ForeColor = Color.Blue;
                            break;
                    }
                }
            }
        }

        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button senderButton = (Button)sender;

            senderButton.Font = new Font("Courier New", 20, FontStyle.Bold);

            int buttonSize = 40;

            int x = senderButton.Location.X / buttonSize;
            int y = senderButton.Location.Y / buttonSize;

            GameTile clickedTile = _gameboard.GetGameTile(x, y);

            switch (e.Button)
            {
                case MouseButtons.Left:

                    if (clickedTile.GetIsMine() == true)
                    {
                        GameOver();
                    }
                    else if(clickedTile.GetIsRevealed() == false)
                    {
                        RevealTile(clickedTile);
                    }
                    break;

                case MouseButtons.Right:

                    if (clickedTile.GetIsMarked() == false && clickedTile.GetIsRevealed() == false)
                    {
                        PlaceFlag(clickedTile);                        
                    }
                    else if (clickedTile.GetIsMarked() == true && clickedTile.GetIsRevealed() == false)
                    {
                        RemoveFlag(clickedTile);
                    }
                    break;

                default:
                    break;
            }
            
            CheckForWin();
        }

        public void RevealMines()
        {
            foreach(GameTile tile in _gameboard.GetGameTiles())
            {
                if (tile.GetIsMine())
                {
                    setButtonTextAndColor(tile, tile.GetIsMarked() ? Color.Orange : Color.IndianRed, Color.Black, "💣");
                }
            }
        }

        public void CheckForWin()
        {
            foreach(GameTile tile in _gameboard.GetGameTiles())
            {
                // all mines must be marked
                if (tile.GetIsMine() && !tile.GetIsMarked())
                    return;

                // no flags can be placed on non-mine tiles
                if (!tile.GetIsMine() && !tile.GetIsMarked())
                    return;

                // all tiles must be revealed or marked
                if (!tile.GetIsRevealed() && !tile.GetIsMarked())
                    return;
            }
            PromptForNewGame("win");
        }

        public void clearBoard()
        {
            foreach (GameTile tile in _gameboard.GetGameTiles())
            {
                setButtonTextAndColor(tile, Color.LightGray, Color.LightGray, "");
            }
        }

        public void setButtonTextAndColor(GameTile tile, Color backColor, Color foreColor, String text)
        {
            tile._button.BackColor = backColor;
            tile._button.ForeColor = foreColor;
            
            tile._button.Text = text;

            if (!tile.GetIsMarked() && !tile.GetIsMine())
            {
                switch (tile.GetAdjacentMines())
                {
                    case 0:
                        tile._button.ForeColor = backColor;
                        tile._button.Text = "";
                        break;
                    case 1:
                        tile._button.ForeColor = Color.DarkBlue;
                        break;
                    case 2:
                        tile._button.ForeColor = Color.DarkGreen;
                        break;
                    case 3:
                        tile._button.ForeColor = Color.DarkRed;
                        break;
                    case 4:
                        tile._button.ForeColor = Color.DarkOrange;
                        break;
                    case 5:
                        tile._button.ForeColor = Color.DarkBlue;
                        break;
                    case 6:
                        tile._button.ForeColor = Color.DarkGreen;
                        break;
                    case 7:
                        tile._button.ForeColor = Color.DarkRed;
                        break;
                    case 8:
                        tile._button.ForeColor = Color.DarkOrange;
                        break;
                    default:
                        tile._button.ForeColor = Color.Blue;
                        break;
                }
            }
        }

        public void RestartGame()
        {
            _gameboard.ResetBoard();
            clearBoard();
        } // restartGame

    }


}
