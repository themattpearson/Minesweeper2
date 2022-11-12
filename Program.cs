using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    internal static class Program
    {
        public static string _difficulty;
        private static DifficultySelectorForm _difficultySelectorForm;
        private static MinesweeperForm _minesweeperForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NewGame();
        }

        public static void NewGame()
        {
            _difficultySelectorForm = new DifficultySelectorForm();
            _difficultySelectorForm.ShowDialog();

            _minesweeperForm = new MinesweeperForm(_difficulty);
            _minesweeperForm.ShowDialog();

            if (!_difficultySelectorForm.IsDisposed)
                _difficultySelectorForm.Close();

            if (!_minesweeperForm.IsDisposed)
                _minesweeperForm.Close();
        }

        public static void CloseAll()
        {
            Application.Exit();
        }
    }
}
