using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class MinesweeperForm : Form
    {
        private Game _game;
        private Gameboard _gameboard;

        public Color _buttonBackgroundColor = Color.LightGray;

        public MinesweeperForm(string difficulty)
        {
            NewGame(difficulty);
        }
            

        public void NewGame(string difficulty)
        {

            _game = new Game(difficulty);
            _gameboard = _game.GetGameboard();

            InitializeComponent();
            CreateButtons();
        }

        public void Restart()
        {
            this.Dispose();

            Program._difficulty = null;

            Program.NewGame();
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

            //Changed captions for JulchMod
            if (result.Equals("loss"))
            {
                message = "Lick my nuts, bucko! (Try Again?)";
                caption = "YOU LOSE";
            }
            else
            {
                message = "Suck my dick! (Run it back?)";
                caption = "YOU WIN";
            }

            var prompt = MessageBox.Show(message, caption,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Exclamation);

            if (prompt == DialogResult.No)
            {
                Program.CloseAll();
            }
            if (prompt == DialogResult.Yes)
            {
                Restart();
            }
        }

        public void GameOver()
        {
            System.IO.Stream explosionSoundStream = Properties.Resources.lors_explosion; // JulchMod changes the explosion noise to a Lors "GARGH" noise
            System.Media.SoundPlayer explosionSound = new System.Media.SoundPlayer(explosionSoundStream);

            RevealMines();
            explosionSound.Play();
            PromptForNewGame("loss");
        }

        public void PlaceFlag(GameTile tile)
        {
            tile.SetIsMarked(true);

            tile._button.Font = new Font("Courier New", 20, FontStyle.Bold);
            tile._button.BackColor = Color.Black;
            tile._button.ForeColor = Color.Red;
                        tile._button.Text = "ඞ"; // swapped for JulchMod
        }

        public void GuessTile(GameTile tile)
        {
            if (tile.GetIsMine() == true)
            {
                GameOver();
            }
            else if (tile.GetIsRevealed() == false)
            {
                RevealTile(tile);
                CheckForWin();
            }
        }

        public void RevealTile(GameTile tile)
        {
            SetButtonTextAndColor(tile, tile.GetAdjacentMines().ToString());
            tile.SetIsRevealed(true);

            if (tile.GetAdjacentMines() == 0)
            {
                _gameboard.FloodZeroes(tile);
            }
            RefreshBoard();
        }

        public void PlaceOrRemoveFlag(GameTile tile)
        {
            if (tile.GetIsMarked() == false && tile.GetIsRevealed() == false)
            {
                PlaceFlag(tile);
                CheckForWin();
            }
            else if (tile.GetIsMarked() == true && tile.GetIsRevealed() == false)
            {
                SetButtonTextAndColor(tile, "");
                tile.SetIsMarked(false);
            }
        }

        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button senderButton = (Button)sender;

            senderButton.Font = new Font("Courier New", 20, FontStyle.Bold);

            int buttonSize = 40;

            int x = senderButton.Location.X / buttonSize;
            int y = senderButton.Location.Y / buttonSize;

            GameTile clickedTile = _gameboard.GetGameTileAtLocation(x, y);

            switch (e.Button)
            {
                case MouseButtons.Left: GuessTile(clickedTile);
                    break;

                case MouseButtons.Right: PlaceOrRemoveFlag(clickedTile);
                    break;

                default:
                    break;
            }
        }

        public void RevealMines()
        {
            foreach(GameTile tile in _gameboard.GetGameTiles())
            {
                if (tile.GetIsMine())
                {
                    SetButtonTextAndColor(tile, "🍖");
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

                // a non-mine tile is currently flagged
                if (tile.GetIsMine() == false && tile.GetIsMarked() == true)
                    return;

                if (!tile.GetIsRevealed() && !tile.GetIsMarked())
                    return;
            }
            PromptForNewGame("win");
        }

        public void RefreshBoard()
        {
            foreach (GameTile tile in _gameboard.GetGameTiles())
            {
                if (tile.GetIsMarked())
                {
                    SetButtonTextAndColor(tile, "ඞ"); //swapped for JulchMod               
                }
                else
                {
                    SetButtonTextAndColor(tile, "");
                }
            }
        }

        public void SetButtonTextAndColor(GameTile tile, String text)
        {
            tile._button.Font = new Font("Courier New", 20, FontStyle.Bold);
            if (text.Equals("ඞ")) //changed to space for JulchMod
            {
                tile._button.Text = "ඞ"; //changed for JulchMod
                tile._button.BackColor = Color.Black;
                tile._button.ForeColor = Color.Red;
            }
            else if (text.Equals("🍖"))
            {
                tile._button.BackColor = tile.GetIsMarked() ? Color.Orange : Color.IndianRed;
                tile._button.ForeColor = Color.Black;
                tile._button.Text = "🍖";
            }
            else if (tile.GetIsRevealed() == true)
            {
                tile._button.BackColor = Color.LightGray;

                int adjacentMines = tile.GetAdjacentMines();

                if (adjacentMines == 0)
                {
                    tile._button.ForeColor = Color.LightGray;
                    tile._button.Text = "";
                    //Add lors face for JulchMod
                    tile._button.Image = Minesweeper.Properties.Resources.lors40;


                }

                else
                {
                    tile._button.ForeColor = GetTextColor(adjacentMines);
                    tile._button.Text = adjacentMines.ToString();

                    //added this check to see if we're next to a mine and label the tile "SUS"
                
                    if(adjacentMines == 1)
                    {
                        tile._button.Text = "1"; 
                    }
                }
            }
            else
            {
                tile._button.BackColor = Color.DarkGray;
                tile._button.ForeColor = Color.LightGray;
                tile._button.Text = "";
            }
        }

        public Color GetTextColor(int adjacentMines)
        {
            switch (adjacentMines)
            {
                case 1:
                    return Color.DarkBlue;
                case 2:
                    return Color.DarkGreen;
                case 3:
                    return Color.DarkRed;
                case 4:
                    return Color.DarkOrange;
                case 5:
                    return Color.DarkBlue;
                case 6:
                    return Color.DarkGreen;
                case 7:
                    return Color.DarkRed;
                case 8:
                    return Color.DarkOrange;
                default:
                    return Color.Blue;
            }
        }

       
    }
}
