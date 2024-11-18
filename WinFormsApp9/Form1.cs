namespace WinFormsApp9
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer = null!;
        private PictureBox ball = null!;
        private PictureBox paddle = null!;
        private PictureBox[] blocks = null!;
        private int ballSpeedX = 4;
        private int ballSpeedY = 4;
        private int paddlePositionX; // Dodanie zmiennej do przechowywania pozycji paletki
        private readonly int paddleSpeed = 20; // Dodanie zmiennej do kontrolowania prêdkoœci paletki

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Ustawienia formularza
            if (Screen.PrimaryScreen != null)
            {
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            this.Height = 600;
            this.Text = "Arkanoid";

            // Tworzenie pi³ki
            ball = new PictureBox
            {
                Width = this.ClientSize.Width / 40,
                Height = this.ClientSize.Width / 40,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle, // Dodanie obramowania
                Left = this.ClientSize.Width / 2,
                Top = this.ClientSize.Height / 2
            };
            this.Controls.Add(ball);

            // Dodanie zielonego obramowania do pi³ki
            ball.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, ball.ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
            };

            // Tworzenie paletki
            paddle = new PictureBox
            {
                Width = this.ClientSize.Width / 8,
                Height = this.ClientSize.Height / 30,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle,
                Left = (this.ClientSize.Width - (this.ClientSize.Width / 8)) / 2, // Ustawienie paletki na œrodku
                Top = this.ClientSize.Height - (this.ClientSize.Height / 12)
            };
            this.Controls.Add(paddle);

            // Dodanie zielonego obramowania do paletki
            paddle.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, paddle.ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
            };

            // Tworzenie bloków
            int blockWidth = this.ClientSize.Width / 10;
            int blockHeight = this.ClientSize.Height / 20;
            int blockRows = 5;
            int blockCols = 10;
            blocks = new PictureBox[blockRows * blockCols];

            for (int row = 0; row < blockRows; row++)
            {
                for (int col = 0; col < blockCols; col++)
                {
                    PictureBox block = new PictureBox
                    {
                        Width = blockWidth,
                        Height = blockHeight,
                        BackColor = Color.Transparent,
                        BorderStyle = BorderStyle.FixedSingle,
                        Left = col * blockWidth,
                        Top = row * blockHeight
                    };
                    block.Paint += (s, e) =>
                    {
                        ControlPaint.DrawBorder(e.Graphics, block.ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
                    };
                    blocks[row * blockCols + col] = block;
                    this.Controls.Add(block);
                }
            }

            // Tworzenie timera
            timer = new System.Windows.Forms.Timer
            {
                Interval = 20
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            // Obs³uga zdarzeñ klawiatury
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            MoveBall();
            CheckCollision();
        }

        private void MoveBall()
        {
            ball.Left += ballSpeedX;
            ball.Top += ballSpeedY;

            // Odbicie od œcian
            if (ball.Left <= 0 || ball.Right >= this.ClientSize.Width)
            {
                ballSpeedX = -ballSpeedX;
                Console.Beep(400, 20);
            }
            if (ball.Top <= 0)
            {
                ballSpeedY = -ballSpeedY;
                Console.Beep(400, 20);
            }
            if (ball.Bottom >= this.ClientSize.Height)
            {
                timer.Stop();
                Console.Beep(300, 700);
                MessageBox.Show("Game Over");
            }
        }

        private void CheckCollision()
        {
            // Odbicie od paletki
            if (ball.Bounds.IntersectsWith(paddle.Bounds))
            {
                Console.Beep(700, 20);
                ballSpeedY = -ballSpeedY;
            }

            // Odbicie od bloków
            for (int i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                if (block != null && ball.Bounds.IntersectsWith(block.Bounds))
                {
                    ballSpeedY = -ballSpeedY;
                    this.Controls.Remove(block);
                    Console.Beep(500, 20);
                    blocks[i] = default!; // Usuniêcie klocka z tablicy
                    break;
                }
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                paddle.Left -= paddleSpeed;
            }
            else if (e.KeyCode == Keys.Right)
            {
                paddle.Left += paddleSpeed;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                paddlePositionX = paddle.Left; // Zapisz pozycjê paletki przed resetem
                ResetGame();
            }

            // Dopasowanie po³o¿enia paletki do wysokoœci ekranu
            if (paddle.Left < 0)
            {
                paddle.Left = 0;
            }
            if (paddle.Right > this.ClientSize.Width)
            {
                paddle.Left = this.ClientSize.Width - paddle.Width;
            }
        }

        private void ResetGame()
        {
            timer.Stop();
            this.Controls.Clear();
            InitializeGame();
            timer.Start(); // Uruchomienie timera po ponownej inicjalizacji gry
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            // Mo¿na dodaæ dodatkowe funkcjonalnoœci
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            // Mo¿na dodaæ dodatkowe funkcjonalnoœci
        }
    }
}
