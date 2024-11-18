using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp9
{
    public partial class StartForm : Form
    {
        private NumericUpDown numericUpDownBallSpeed = null!;
        private NumericUpDown numericUpDownPaddleSpeed = null!;
        private NumericUpDown numericUpDownPowerUpCount = null!;
        private ComboBox difficultyComboBox = null!;

        public StartForm()
        {
            InitializeComponent();
            InitializeStartMenu();
        }

        private void InitializeStartMenu()
        {
            this.Width = 800;
            this.Height = 600;
            this.Text = "Arkanoid - Menu Startowe";

            Button startButton = new()
            {
                Text = "Start",
                Width = 200,
                Height = 50,
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 25,
                ForeColor = Color.White // Ustawienie koloru tekstu na biały
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            // Label for ball speed
            Label labelBallSpeed = new()
            {
                Text = "Prędkość piłki:",
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 100,
                Width = 200,
                ForeColor = Color.White // Ustawienie koloru tekstu na biały
            };
            this.Controls.Add(labelBallSpeed);

            // NumericUpDown for ball speed
            numericUpDownBallSpeed = new NumericUpDown()
            {
                Minimum = 1,
                Maximum = 20,
                Value = 5,
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 75
            };
            this.Controls.Add(numericUpDownBallSpeed);

            // Label for paddle speed
            Label labelPaddleSpeed = new()
            {
                Text = "Prędkość paletki:",
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 150,
                Width = 200,
                ForeColor = Color.White // Ustawienie koloru tekstu na biały
            };
            this.Controls.Add(labelPaddleSpeed);

            // NumericUpDown for paddle speed
            numericUpDownPaddleSpeed = new NumericUpDown()
            {
                Minimum = 1,
                Maximum = 20,
                Value = 5,
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 125
            };
            this.Controls.Add(numericUpDownPaddleSpeed);

            // Label for number of power-ups
            Label labelPowerUpCount = new()
            {
                Text = "Liczba power-upów:",
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 200,
                Width = 200,
                ForeColor = Color.White // Ustawienie koloru tekstu na biały
            };
            this.Controls.Add(labelPowerUpCount);

            // NumericUpDown for number of power-ups
            numericUpDownPowerUpCount = new NumericUpDown()
            {
                Minimum = 0,
                Maximum = 10,
                Value = 3,
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 175
            };
            this.Controls.Add(numericUpDownPowerUpCount);

            // ComboBox for difficulty levels
            difficultyComboBox = new ComboBox()
            {
                Left = (this.ClientSize.Width / 2) - 100,
                Top = (this.ClientSize.Height / 2) - 225,
                Width = 200
            };
            difficultyComboBox.Items.AddRange(new string[] { "Łatwy", "Średni", "Trudny", "Własny" });
            difficultyComboBox.SelectedIndex = 0;
            difficultyComboBox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;
            this.Controls.Add(difficultyComboBox);
        }

        private void DifficultyComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (difficultyComboBox.SelectedItem is null)
                return;

            bool isCustom = difficultyComboBox.SelectedItem.ToString() == "Własny";
            numericUpDownBallSpeed.Enabled = isCustom;
            numericUpDownPaddleSpeed.Enabled = isCustom;
            numericUpDownPowerUpCount.Enabled = isCustom;

            if (!isCustom)
            {
                switch (difficultyComboBox.SelectedItem.ToString())
                {
                    case "Łatwy":
                        numericUpDownBallSpeed.Value = 3;
                        numericUpDownPaddleSpeed.Value = 10;
                        numericUpDownPowerUpCount.Value = 5;
                        break;
                    case "Średni":
                        numericUpDownBallSpeed.Value = 5;
                        numericUpDownPaddleSpeed.Value = 15;
                        numericUpDownPowerUpCount.Value = 3;
                        break;
                    case "Trudny":
                        numericUpDownBallSpeed.Value = 7;
                        numericUpDownPaddleSpeed.Value = 20;
                        numericUpDownPowerUpCount.Value = 1;
                        break;
                }
            }
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            int ballSpeed = (int)numericUpDownBallSpeed.Value;
            int paddleSpeed = (int)numericUpDownPaddleSpeed.Value;
            int powerUpCount = (int)numericUpDownPowerUpCount.Value;

            Form1 gameForm = new Form1(ballSpeed, paddleSpeed, powerUpCount);
            gameForm.Show();
            this.Hide();
        }
    }
}
