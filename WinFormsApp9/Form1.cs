namespace WinFormsApp9
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer = null!;
        private System.Windows.Forms.Timer powerUpTimer = null!;
        private PictureBox ball = null!;
        private PictureBox paddle = null!;
        private PictureBox[] blocks = null!;
        private List<PowerUp> activePowerUps = new List<PowerUp>();
        private int ballSpeedX;
        private int ballSpeedY;
        private int paddlePositionX;
        private readonly int paddleSpeed;
        private bool isMovingLeft = false;
        private bool isMovingRight = false;
        private int powerUpCount;

        public Form1(int ballSpeed, int paddleSpeed, int powerUpCount)
        {
            this.ballSpeedX = ballSpeed;
            this.ballSpeedY = ballSpeed;
            this.paddleSpeed = paddleSpeed;
            this.powerUpCount = powerUpCount;

            InitializeComponent();
            InitializeGame();
            this.Resize += Form1_Resize;
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            ResizeGameElements();
        }

        private void InitializeGame()
        {
            // Ustawienia formularza
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;

            // Tworzenie pi³ki
            ball = new PictureBox
            {
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle
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
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(paddle);

            // Dodanie zielonego obramowania do paletki
            paddle.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, paddle.ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
            };

            // Tworzenie bloków
            int blockRows = 5;
            int blockCols = 10;
            blocks = new PictureBox[blockRows * blockCols];

            for (int row = 0; row < blockRows; row++)
            {
                for (int col = 0; col < blockCols; col++)
                {
                    PictureBox block = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        BorderStyle = BorderStyle.FixedSingle
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

            // Tworzenie timera dla power-upów
            powerUpTimer = new System.Windows.Forms.Timer
            {
                Interval = 50
            };
            powerUpTimer.Tick += PowerUpTimer_Tick;
            powerUpTimer.Start();

            // Obs³uga zdarzeñ klawiatury
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

            // Dostosowanie rozmiaru i pozycji elementów
            ResizeGameElements();
        }

        private void ResizeGameElements()
        {
            // Dostosowanie rozmiaru i pozycji pi³ki
            ball.Width = this.ClientSize.Width / 40;
            ball.Height = this.ClientSize.Width / 40;
            ball.Left = this.ClientSize.Width / 2 - (this.ClientSize.Width / 80);
            ball.Top = this.ClientSize.Height / 2 - (this.ClientSize.Width / 80);

            // Dostosowanie rozmiaru i pozycji paletki
            paddle.Width = this.ClientSize.Width / 8;
            paddle.Height = this.ClientSize.Height / 30;
            paddle.Left = (this.ClientSize.Width - paddle.Width) / 2;
            paddle.Top = this.ClientSize.Height - (this.ClientSize.Height / 12);

            // Dostosowanie rozmiaru i pozycji bloków
            int blockRows = 5;
            int blockCols = 10;
            int blockWidth = this.ClientSize.Width / blockCols;
            int blockHeight = this.ClientSize.Height / 20;

            for (int row = 0; row < blockRows; row++)
            {
                for (int col = 0; col < blockCols; col++)
                {
                    var block = blocks[row * blockCols + col];
                    if (block != null)
                    {
                        block.Width = blockWidth;
                        block.Height = blockHeight;
                        block.Left = col * blockWidth;
                        block.Top = row * blockHeight;
                    }
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            MoveBall();
            MovePaddle();
            CheckCollision();
        }

        private void PowerUpTimer_Tick(object? sender, EventArgs e)
        {
            MovePowerUps();
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

        private void MovePaddle()
        {
            if (isMovingLeft && paddle.Left > 0)
            {
                paddle.Left -= paddleSpeed;
            }
            if (isMovingRight && paddle.Right < this.ClientSize.Width)
            {
                paddle.Left += paddleSpeed;
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
                    blocks[i] = default!;

                    // Losowe przypisanie power-upów do klocków
                    Random random = new Random();
                    if (random.Next(4) == 0 && powerUpCount > 0) // 1 na 4 klocki
                    {
                        PowerUpType powerUpType = (PowerUpType)random.Next(2);
                        PowerUp powerUp = new PowerUp(powerUpType, block.Left, block.Top, block.Width, block.Height);
                        activePowerUps.Add(powerUp);
                        this.Controls.Add(powerUp.PowerUpPictureBox);
                        powerUpCount--;
                    }
                    break;
                }
            }
        }

        private void MovePowerUps()
        {
            foreach (var powerUp in activePowerUps.ToList())
            {
                powerUp.PowerUpPictureBox.Top += powerUp.PowerUpSpeed;

                // Miganie power-upów
                powerUp.PowerUpPictureBox.Visible = !powerUp.PowerUpPictureBox.Visible;

                // Usuwanie power-upów, które spad³y na dó³
                if (powerUp.PowerUpPictureBox.Top >= this.ClientSize.Height)
                {
                    this.Controls.Remove(powerUp.PowerUpPictureBox);
                    activePowerUps.Remove(powerUp);
                }
                // Kolizja z paletk¹
                else if (paddle.Bounds.IntersectsWith(powerUp.PowerUpPictureBox.Bounds))
                {
                    ApplyPowerUp(powerUp);
                    this.Controls.Remove(powerUp.PowerUpPictureBox);
                    activePowerUps.Remove(powerUp);
                }
            }
        }

        private void ApplyPowerUp(PowerUp powerUp)
        {
            if (powerUp.PowerUpType == PowerUpType.SlowBall)
            {
                ballSpeedX /= 2;
                ballSpeedY /= 2;
                Console.Beep(200, 20);
            }
            else if (powerUp.PowerUpType == PowerUpType.ExpandPaddle)
            {
                paddle.Width *= 2;
                Console.Beep(200, 20);
            }

            // Ustawienie timera dla power-upa
            var powerUpTimer = new System.Windows.Forms.Timer
            {
                Interval = 55000 // 55 sekund
            };
            powerUpTimer.Tick += (s, e) => RemovePowerUpEffect(powerUp, powerUpTimer);
            powerUpTimer.Start();
        }

        private void RemovePowerUpEffect(PowerUp powerUp, System.Windows.Forms.Timer powerUpTimer)
        {
            if (powerUp.PowerUpType == PowerUpType.SlowBall)
            {
                ballSpeedX *= 2;
                ballSpeedY *= 2;
            }
            else if (powerUp.PowerUpType == PowerUpType.ExpandPaddle)
            {
                paddle.Width /= 2;
            }

            powerUpTimer.Stop();
            powerUpTimer.Dispose();
        }
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                isMovingLeft = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                isMovingRight = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                paddlePositionX = paddle.Left;
                ResetGame();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                isMovingLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                isMovingRight = false;
            }
        }

        private void ResetGame()
        {
            timer.Stop();
            powerUpTimer.Stop();
            this.Controls.Clear();
            InitializeGame();
            timer.Start();
            powerUpTimer.Start();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            // Mo¿na dodaæ dodatkowe funkcjonalnoœci
        }
    }
    }
    

public class PowerUp
{
    public PowerUpType PowerUpType { get; }
    public PictureBox PowerUpPictureBox { get; }
    public int PowerUpSpeed { get; }

    public PowerUp(PowerUpType powerUpType, int x, int y, int width, int height)
    {
        PowerUpType = powerUpType;
        PowerUpSpeed = 5;
        PowerUpPictureBox = new PictureBox
        {
            Width = width,
            Height = height,
            Left = x,
            Top = y,
            BackColor = powerUpType == PowerUpType.SlowBall ? Color.Blue : Color.Red
        };
    }
}

public enum PowerUpType
{
    SlowBall,
    ExpandPaddle
}
