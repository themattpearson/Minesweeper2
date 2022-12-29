using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class DifficultySelectorForm : Form
    {
        private Label _label;
        private Button _hardGameButton;
        private Button _mediumGameButton;
        private Button _easyGameButton;
        private CheckBox _enableJulchModCheckBox;

        public DifficultySelectorForm()
        {
            _label = new Label();
            _easyGameButton = new Button();
            _mediumGameButton = new Button();
            _hardGameButton = new Button();
            _enableJulchModCheckBox = new CheckBox();

            _easyGameButton.Width = 75;
            _mediumGameButton.Width = 75;
            _hardGameButton.Width = 75;
            _enableJulchModCheckBox.Width = 200;

            _easyGameButton.Text = "Easy";
            _mediumGameButton.Text = "Medium";
            _hardGameButton.Text = "Hard";

            _enableJulchModCheckBox.Text = "Enable JulchMod? (Experimental)";

            _easyGameButton.Location = new Point(0, 100);
            _mediumGameButton.Location = new Point(75, 100);
            _hardGameButton.Location = new Point(150, 100);
            _enableJulchModCheckBox.Location = new Point(25, 125);

            _easyGameButton.MouseDown += new MouseEventHandler(Button_Click);
            _mediumGameButton.MouseDown += new MouseEventHandler(Button_Click);
            _hardGameButton.MouseDown += new MouseEventHandler(Button_Click);
            
            _enableJulchModCheckBox.MouseDown += new MouseEventHandler(CheckBox_Click);

            _label.Text = "Select difficulty";
            _label.Width = 150;
            _label.Font = new Font("Comic Sans MS", 12);
            _label.Location = new Point(50, 25);

            InitializeComponent();

            Controls.Add(_label);
            Controls.Add(_enableJulchModCheckBox);
            Controls.Add(_easyGameButton);
            Controls.Add(_mediumGameButton);
            Controls.Add(_hardGameButton);

        }
        private void Button_Click(object sender, MouseEventArgs e)
        {

            Button senderButton = (Button)sender;

            if (senderButton == _easyGameButton)
            {
                Program._difficulty = 0;
            }
            if (senderButton == _mediumGameButton)
            {
                Program._difficulty = 1;
            }
            if (senderButton == _hardGameButton)
            {
                Program._difficulty = 2;
            }

            Program._julchModEnabled = _enableJulchModCheckBox.Checked;

            this.Dispose();
        }

        private void CheckBox_Click(Object sender, EventArgs e)
        {
            Program._julchModEnabled = _enableJulchModCheckBox.Checked;

            if (!Program._julchModEnabled)
            {
                _easyGameButton.Text = "baby";
                _mediumGameButton.Text = "not bad";
                _hardGameButton.Text = "serious";
            }
            else
            {
                _easyGameButton.Text = "Easy";
                _mediumGameButton.Text = "Medium";
                _hardGameButton.Text = "Hard";
            }
        }

    }
}
