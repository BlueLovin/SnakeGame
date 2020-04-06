using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;//for sound effects
using System.Windows.Forms;

namespace SnakeGameRetry
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private List<char> PreviousMovements = new List<char>();
        public char Direction = 'D';
        bool GameOver = true;
        private Circle food = new Circle();
        static Random random = new Random();
        static int randomFoodX = random.Next(0, 18);
        static int randomFoodY = random.Next(0, 17);
        private int score;
        private int highScore;

        public Form1()
        {
            InitializeComponent();
            timer.Interval = 150;
        }
        private void BeginGame()
        {
            //reset variables
            startButton.Enabled = false;
            comboBox1.Enabled = false;
            GameOver = false;
            GameOverLabel.Visible = false;
            score = 0;
            CurrentScore.Text = score.ToString();
            HighScore.Text = (highScore * 100).ToString();
            Snake.Clear();
            randomFoodX = random.Next(0, 18);
            randomFoodY = random.Next(0, 17);



            //restarting game
            timer.Start();
            Circle head = new Circle { X = 0, Y = 0 };
            Snake.Add(head);
            Direction = 'D';
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!GameOver) //if gameover is false, draw snake
            {
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0) //IF SNAKE === head then draw a different color
                    {
                        canvas.FillEllipse(Brushes.DarkCyan,
                            Snake[i].X * 25, Snake[i].Y * 25, 25, 25);
                    }
                    else // tail
                        canvas.FillEllipse(Brushes.DarkSlateGray,
                            Snake[i].X * 25, Snake[i].Y * 25, 25, 25);
                }
                //////////
                ///FOOD///
                //////////
                canvas.FillRectangle(Brushes.Indigo,
                    randomFoodX * 25, randomFoodY * 25, 25, 25);
            }
        }
        private void BeginButtonClicked(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)//easy setting
            {
                timer.Interval = 200;
                BeginGame();
            }
            if (comboBox1.SelectedIndex == 1)//medium setting
            {
                timer.Interval = 100;
                BeginGame();
            }
            if (comboBox1.SelectedIndex == 2)//hard setting
            {
                timer.Interval = 75;
                BeginGame();
            }
            if (comboBox1.SelectedIndex == 3)//wtf setting
            {
                timer.Interval = 45;
                BeginGame();
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Please select a difficulty", "No Difficulty Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void timer_Tick(object sender, EventArgs e)//tick tock
        {
            movePlayer();
            Refresh();//redraws elements on screen

            /////////////////
            //window bounds//
            /////x = 18//////
            /////y = 17//////
            /////////////////
            if (Snake[0].X >= 18 && Direction == 'R' || Snake[0].X < 0 && Direction == 'L'
                || Snake[0].Y >= 17 && Direction == 'D' || Snake[0].Y < 0 && Direction == 'U')
            {
                die();
            }
            ////
            ////
            ////
        }
        /// <summary>
        /// key is pressed
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && Direction != 'U')
            {
                Direction = 'D';
            }
            if (e.KeyCode == Keys.Up && Direction != 'D')
            {
                Direction = 'U';
            }
            if (e.KeyCode == Keys.Left && Direction != 'R')
            {
                Direction = 'L';
            }
            if (e.KeyCode == Keys.Right && Direction != 'L')
            {
                Direction = 'R';
            }
            //if (e.KeyCode == Keys.A)
            //{
            //    eat();
            //}
            //^ for debugging purposes
        }
        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)//iterating through the snake pieces
            {
                if (i == 0)//snake head
                {
                    if (Direction == 'U')//up
                        Snake[i].Y--;
                    if (Direction == 'D')//down
                        Snake[i].Y++;
                    if (Direction == 'R')//right
                        Snake[i].X++;
                    if (Direction == 'L')//left
                        Snake[i].X--;

                    for (int j = 1; j < Snake.Count; j++)//collision with tail
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }

                    }
                    if (Snake[i].X == randomFoodX && Snake[i].Y == randomFoodY)
                    {
                        eat();
                    }
                }

                //draw snake tail. based on previous position of the snake piece that was drawn

                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;

                }
            }
        }
        private void eat()
        {
            score++;
            CurrentScore.Text = (score * 100).ToString();//update current score
            Circle Body = new Circle // add new snake tail piece
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(Body);
            //generate new food
            randomFoodX = random.Next(0, 18);
            randomFoodY = random.Next(0, 17);

        }
        private void die()
        {
            //check for new high score
            if (score * 100 > Convert.ToInt32(HighScore.Text))
            {
                highScore = score;
            }
            //
            //reset variables
            startButton.Enabled = true;
            GameOverLabel.Visible = true;
            comboBox1.Enabled = true;
            timer.Stop();
            GameOver = true;
            SystemSounds.Beep.Play();
        }
    }
}