using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private Game _game;
        private Gameboard _gameboard;

        public Form1()
        {
            _game = new Game("easy");
            _gameboard = _game.GetGameboard();

            InitializeComponent();
            CreateButtons();
        }

        public void CreateButtons()
        {
            int buttonSize = 40;
            Color backColor = Color.LightGray;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _gameboard.GetGameTile(i,j)._button.Height = buttonSize;
                    _gameboard.GetGameTile(i, j)._button.Width = buttonSize;
                    _gameboard.GetGameTile(i, j)._button.BackColor = backColor;
                    _gameboard.GetGameTile(i, j)._button.Location = new Point(i * buttonSize, j * buttonSize);
                    _gameboard.GetGameTile(i, j)._button.MouseDown += new MouseEventHandler(Button_Click);
                    Controls.Add(_gameboard.GetGameTile(i, j)._button);
                } 
            }
        }

        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button senderButton = (Button)sender;

            int x = senderButton.Location.X / 40;
            int y = senderButton.Location.Y / 40;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    senderButton.Font = new Font("Courier New", 15, FontStyle.Bold);
                    if (_gameboard.GetGameTiles()[x, y].isMine)
                    {

                        revealMines();

                        System.IO.Stream str = Properties.Resources.explosion_x;
                        System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                        snd.Play();

                        string message = "Oof! Toughie.. please play again? UwU";
                        const string caption = "Sad";

                        var result = MessageBox.Show(message, caption,
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Exclamation);

                        if (result == DialogResult.No)
                        {
                            this.Close();
                        }
                        if (result == DialogResult.Yes)
                        {
                            restartGame();
                        }

                    }
                    else
                    {
                        setButtonTextAndColor(_gameboard.GetGameTiles()[x, y], Color.DarkGray, Color.LightGray, _gameboard.GetGameTiles()[x, y].adjacentMines.ToString());
                        _gameboard.GetGameTiles()[x, y].isRevealed = true;

                        if (_gameboard.GetGameTiles()[x, y].adjacentMines == 0)
                        {
                            floodZeroes(x, y);
                        } // flip adjacent 0s

                        if (checkForWin())
                            youWon();
                    }
                    break;

                case MouseButtons.Right:

                    if (!_gameboard.GetGameTiles()[x, y].isMarked)
                    {
                        _gameboard.GetGameTiles()[x, y].isMarked = true;
                        setButtonTextAndColor(_gameboard.GetGameTiles()[x, y], Color.Yellow, Color.Black, "⚑");

                        if (checkForWin())
                            youWon();
                    }
                    else
                    {
                        setButtonTextAndColor(_gameboard.GetGameTiles()[x, y], Color.LightGray, Color.LightGray, "");
                        _gameboard.GetGameTiles()[x, y].isMarked = false;
                    }
                    break;

                default:
                    break;

            } // switch Button press
        } // Button_Click

        public void revealMines()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_gameboard.GetGameTiles()[i, j].isMine)
                    {
                        setButtonTextAndColor(_gameboard.GetGameTiles()[i, j], _gameboard.GetGameTiles()[i, j].isMarked ? Color.Orange : Color.IndianRed, Color.Black, "💣");
                    }

                }
            }
        } // revealMines

        public void floodZeroes(int x, int y)
        {
            Queue<Point> s = new Queue<Point>();
            Point p = new Point(x, y);
            s.Enqueue(p);

            while (s.Count > 0)
            {
                p = s.Dequeue();

                Console.WriteLine(p);

                setButtonTextAndColor(_gameboard.GetGameTiles()[p.X, p.Y], Color.DarkGray, Color.LightGray, _gameboard.GetGameTiles()[p.X, p.Y].adjacentMines.ToString());
                _gameboard.GetGameTiles()[p.X, p.Y].isRevealed = true;

                if (_gameboard.GetGameTiles()[p.X, p.Y].adjacentMines == 0)
                {
                    if (p.X > 0 && !_gameboard.GetGameTiles()[p.X - 1, p.Y].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y));
                    if (p.X < 9 && !_gameboard.GetGameTiles()[p.X + 1, p.Y].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y));
                    if (p.Y > 0 && !_gameboard.GetGameTiles()[p.X, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X, p.Y - 1));
                    if (p.Y < 9 && !_gameboard.GetGameTiles()[p.X, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X, p.Y + 1));

                    if (p.X < 9 && p.Y < 9 && !_gameboard.GetGameTiles()[p.X + 1, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y + 1));
                    if (p.X > 0 && p.Y > 0 && !_gameboard.GetGameTiles()[p.X - 1, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y - 1));
                    if (p.X > 0 && p.Y < 9 && !_gameboard.GetGameTiles()[p.X - 1, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y + 1));
                    if (p.X < 9 && p.Y > 0 && !_gameboard.GetGameTiles()[p.X + 1, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y - 1));
                }

            } // while

        }

        public bool checkForWin()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_gameboard.GetGameTiles()[i, j].isMine && !_gameboard.GetGameTiles()[i, j].isMarked)
                        return false;

                    // prevents marking all tiles to win
                    if (!_gameboard.GetGameTiles()[i, j].isMine && _gameboard.GetGameTiles()[i, j].isMarked)
                        return false;

                    if (!_gameboard.GetGameTiles()[i, j].isRevealed && !_gameboard.GetGameTiles()[i, j].isMarked)
                        return false;

                }
            }
            return true;
        } // checkForWin

        public void clearBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    setButtonTextAndColor(_gameboard.GetGameTiles()[i, j], Color.LightGray, Color.LightGray, "");
                } // for j
            } // for i
        } // clearBoard

        public void youWon()
        {
            string message = "WINNER! Play again..?";
            const string caption = "Nice";

            var result = MessageBox.Show(message, caption,
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                this.Close();
            }
            if (result == DialogResult.Yes)
            {
                restartGame();
            }
        } // youWon

        public void setButtonTextAndColor(GameTile tile, Color backColor, Color foreColor, String text)
        {
            tile._button.BackColor = backColor;
            tile._button.ForeColor = foreColor;
            tile._button.Font = text.Equals("⚑") ? new Font("Courier New", 20, FontStyle.Bold) : new Font("Courier New", 15, FontStyle.Bold);
            tile._button.Text = text;

            if (!tile.isMarked && !tile.isMine)
            {
                switch (tile.adjacentMines)
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
                } // switch
            } // if
        } // setButtonTextAndColor

        public void restartGame()
        {
            GameTile.clearMines(_gameboard);
            GameTile.setMines(_gameboard);
            GameTile.setAdjacentMines(_gameboard);
            clearBoard();
        } // restartGame

    }


}
