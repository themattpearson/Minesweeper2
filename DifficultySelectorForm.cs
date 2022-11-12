using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class DifficultySelectorForm : Form
    {
        private Button _hardGameButton;
        private Button _mediumGameButton;
        private Button _easyGameButton;

        public DifficultySelectorForm()
        {
            _easyGameButton = new Button();
            _mediumGameButton = new Button();
            _hardGameButton = new Button();

            _easyGameButton.Width = 75;
            _mediumGameButton.Width = 75;
            _hardGameButton.Width = 75;

            _easyGameButton.Text = "Easy";
            _mediumGameButton.Text = "Medium";
            _hardGameButton.Text = "Hard";

            _easyGameButton.Location = new Point(0, 100);
            _mediumGameButton.Location = new Point(75, 100);
            _hardGameButton.Location = new Point(150, 100);

            _easyGameButton.MouseDown += new MouseEventHandler(Button_Click);
            _mediumGameButton.MouseDown += new MouseEventHandler(Button_Click);
            _hardGameButton.MouseDown += new MouseEventHandler(Button_Click);

            Label label = new Label();
            label.Text = "Select difficulty";
            label.Width = 150;
            label.Font = new Font("Comic Sans MS", 12);
            label.Location = new Point(50, 25);

            InitializeComponent();

            Controls.Add(label);
            Controls.Add(_easyGameButton);
            Controls.Add(_mediumGameButton);
            Controls.Add(_hardGameButton);

        }
        private void Button_Click(object sender, MouseEventArgs e)
        {

            Button senderButton = (Button)sender;

            if (senderButton == _easyGameButton)
            {
                Program._difficulty = "easy";
            }
            if (senderButton == _mediumGameButton)
            {
                Program._difficulty = "medium";
            }
            if (senderButton == _hardGameButton)
            {
                Program._difficulty = "hard";
            }

            this.Dispose();
        }
    }
}
