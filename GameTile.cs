using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class GameTile
    {
        public int _adjacentMines { get; set; }
        public bool _isMine { get; set; }
        public bool _isRevealed { get; set; }
        public bool _isMarked { get; set; }
        public Button _button { get; set; }
        public int _row { get; set; }
        public int _col { get; set; }

        public GameTile(int row, int col)
        {
            _isMine = false;
            _isRevealed = false;
            _isMarked = false;
            _adjacentMines = 0;
            _button = new Button();
            _row = row;
            _col = col;
        }

        public GameTile()
        {
            _isMine = false;
            _isRevealed = false;
            _isMarked = false;
            _adjacentMines = 0;
            _button = new Button();
        }

        public void SetIsMine(bool isMine)
        {
            _isMine = isMine;
        }

        public bool GetIsMine()
        {
            return _isMine;
        }

        public void SetAdjacentMines(int mines)
        {
            _adjacentMines = mines;
        }

        public int GetAdjacentMines()
        {
            return _adjacentMines;
        }

        public void SetIsMarked(bool isMarked)
        {
            _isMarked = isMarked;
        }

        public bool GetIsMarked()
        {
            return _isMarked;
        }

        public void SetLocation(int row, int col)
        {
            _row = row;
            _col = col;
        }

        public void ResetGameTile()
        {
            _adjacentMines = 0;
            _isMine = false;
            _isMarked = false;
            _isRevealed = false;
        }
    }
}
