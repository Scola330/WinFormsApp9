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
                Top = (this.ClientSize.Height / 2) - 25
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            Form1 gameForm = new();
            gameForm.Show();
            this.Hide();
        }
    }
}
