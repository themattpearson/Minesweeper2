using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public GameTile[,] formBoard = new GameTile[10, 10];

        public Form1(GameTile [,] board)
        {
            formBoard = board;
            InitializeComponent();
            createButtons();
        }

        public void createButtons()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    formBoard[i, j].button.Height = 40;
                    formBoard[i, j].button.Width = 40;
                    formBoard[i, j].button.BackColor = Color.LightGray;
                    formBoard[i, j].button.Location = new Point(i * 40, j * 40);
                    formBoard[i, j].button.MouseDown += new MouseEventHandler(Button_Click);
                    Controls.Add(formBoard[i, j].button);
                } // for j
            } // for i
        } // createButtons

        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button senderButton = (Button)sender;

            int x = senderButton.Location.X / 40;
            int y = senderButton.Location.Y / 40;

            switch (e.Button)
            {
                case MouseButtons.Left:

                    senderButton.Font = new Font("Courier New", 15, FontStyle.Bold);
                    if(formBoard[x, y].isMine)
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
                        setButtonTextAndColor(formBoard[x, y], Color.DarkGray, Color.LightGray, formBoard[x, y].adjacentMines.ToString());
                        formBoard[x, y].isRevealed = true;

                        if (formBoard[x,y].adjacentMines == 0)
                        {
                            floodZeroes(x, y);
                        } // flip adjacent 0s
                        
                        if (checkForWin())
                            youWon();
                    }
                    break;

                case MouseButtons.Right:

                    if(!formBoard[x, y].isMarked)
                    {
                        formBoard[x, y].isMarked = true;
                        setButtonTextAndColor(formBoard[x, y], Color.Yellow, Color.Black, "⚑");
                        
                        if (checkForWin())
                            youWon();
                    }
                    else
                    {
                        setButtonTextAndColor(formBoard[x, y], Color.LightGray, Color.LightGray, "");
                        formBoard[x, y].isMarked = false;
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
                    if(formBoard[i, j].isMine)
                    {
                        setButtonTextAndColor(formBoard[i, j], formBoard[i, j].isMarked ? Color.Orange : Color.IndianRed, Color.Black, "💣");
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

                    setButtonTextAndColor(formBoard[p.X, p.Y], Color.DarkGray, Color.LightGray, formBoard[p.X, p.Y].adjacentMines.ToString());
                    formBoard[p.X, p.Y].isRevealed = true;

                    if(formBoard[p.X, p.Y].adjacentMines == 0)
                    {
                        if (p.X > 0 && !formBoard[p.X - 1, p.Y].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y));
                        if (p.X < 9 && !formBoard[p.X + 1, p.Y].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y));
                        if (p.Y > 0 && !formBoard[p.X, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X, p.Y - 1));
                        if (p.Y < 9 && !formBoard[p.X, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X, p.Y + 1));

                        if (p.X < 9 && p.Y < 9 && !formBoard[p.X + 1, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y + 1));
                        if (p.X > 0 && p.Y > 0 && !formBoard[p.X - 1, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y - 1));
                        if (p.X > 0 && p.Y < 9 && !formBoard[p.X - 1, p.Y + 1].isRevealed) s.Enqueue(new Point(p.X - 1, p.Y + 1));
                        if (p.X < 9 && p.Y > 0 && !formBoard[p.X + 1, p.Y - 1].isRevealed) s.Enqueue(new Point(p.X + 1, p.Y - 1));
                }

            } // while

        }

        public bool checkForWin()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (formBoard[i, j].isMine && !formBoard[i,j].isMarked)
                        return false;
                    
                    // prevents marking all tiles to win
                    if (!formBoard[i, j].isMine && formBoard[i, j].isMarked)
                        return false;

                    if (!formBoard[i, j].isRevealed && !formBoard[i, j].isMarked)
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
                    setButtonTextAndColor(formBoard[i, j], Color.LightGray, Color.LightGray, "");
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
            tile.button.BackColor = backColor;
            tile.button.ForeColor = foreColor;
            tile.button.Font = text.Equals("⚑") ? new Font("Courier New", 20, FontStyle.Bold) : new Font("Courier New", 15, FontStyle.Bold);
            tile.button.Text = text;

            if (!tile.isMarked && !tile.isMine)
            {
                switch (tile.adjacentMines)
                {
                    case 0:
                        tile.button.ForeColor = backColor;
                        tile.button.Text = "";
                        break;
                    case 1:
                        tile.button.ForeColor = Color.DarkBlue;
                        break;
                    case 2:
                        tile.button.ForeColor = Color.DarkGreen;
                        break;
                    case 3:
                        tile.button.ForeColor = Color.DarkRed;
                        break;
                    case 4:
                        tile.button.ForeColor = Color.DarkOrange;
                        break;
                    case 5:
                        tile.button.ForeColor = Color.DarkBlue;
                        break;
                    case 6:
                        tile.button.ForeColor = Color.DarkGreen;
                        break;
                    case 7:
                        tile.button.ForeColor = Color.DarkRed;
                        break;
                    case 8:
                        tile.button.ForeColor = Color.DarkOrange;
                        break;
                    default:
                        tile.button.ForeColor = Color.Blue;
                        break;
                } // switch
            } // if
        } // setButtonTextAndColor

        public void restartGame()
        {
            GameTile.clearMines(formBoard);
            GameTile.setMines(formBoard);
            GameTile.setAdjacentMines(formBoard);
            clearBoard();
        } // restartGame

    }


}
