namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle apples = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();

        bool goLeft, goRight, goUp, goDown;

        public Form1()
        {
            InitializeComponent();

            new Settings();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void GameTimer(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }

            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "right":
                            Snake[i].x++;
                            break;
                        case "left":
                            Snake[i].x--;
                            break;
                        case "up":
                            Snake[i].y--;
                            break;
                        case "down":
                            Snake[i].y++;
                            break;
                    }

                    if (Snake[i].x < 0)
                    {
                        Snake[i].x = maxWidth;
                    }
                    if (Snake[i].x > maxWidth)
                    {
                        Snake[i].x = 0;
                    }
                    if(Snake[i].y < 0)
                    {
                        Snake[i].y = maxHeight;
                    }
                    if (Snake[i].y > maxHeight)
                    {
                        Snake[i].y = 0;
                    }

                    if (Snake[i].x == apples.x && Snake[i].y == apples.y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j <Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            GameOver();
                        }
                    }
                }
                else
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }

                Canvas.Invalidate();
            }
        }

        private void UpdatePicture(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColour;
            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Purple;
                }
                else
                {
                    snakeColour = Brushes.Pink;
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snake[i].x * Settings.Width,
                    Snake[i].y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));

                canvas.FillEllipse(Brushes.Yellow, new Rectangle
                    (
                    apples.x * Settings.Width,
                    apples.y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }
        }

        private void RestartGame()
        {
            maxWidth = Canvas.Width / Settings.Width - 1;
            maxHeight = Canvas.Height / Settings.Height - 1;

            Snake.Clear(); 

            StartButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { x = 10, y = 15 };
            Snake.Add(head);
            Circle boby = new Circle();
            Snake.Add(boby);

            apples = new Circle { x = rand.Next(1, maxWidth), y = rand.Next(1, maxHeight) };
            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 1;

            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                x = Snake[Snake.Count - 1].x,
                y = Snake[Snake.Count - 1].y
            };

            Snake.Add(body);

            apples = new Circle
            {
                x = rand.Next(1, maxWidth),
                y = rand.Next(1, maxHeight)
            };
        }

        private void GameOver()
        {
            gameTimer.Stop();
            StartButton.Enabled = true;

            if(score > highScore)
            {
                highScore = score;

                txtHighScore.Text = "High Score: " + highScore;
                txtHighScore.ForeColor = Color.OrangeRed;
            }
        }
    }
}