///Jacob Boyd
///Mr T.
///March 23
///Recreation of classic arcade game Space Race

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Variables
        bool running = false;

        bool endscreen = false;

        int p1Score = 0;
        int p2Score = 0;

        int width = 715;
        int height = 460;

        int player1X = 250;
        int player1Y = 450;

        int player2X = 450;
        int player2Y = 450;

        int playerWidth = 13;
        int playerHeight = 30;
        int playerSpeed = 7;

        int asteroidWidth = 5;
        int asteroidHeight = 5;

        int timerLine = 37;

        bool wDown = false;
        bool sDown = false;

        bool upArrowDown = false;
        bool downArrowDown = false;

        Pen drawPen = new Pen(Color.Blue, 3);

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush OrangeBrush = new SolidBrush(Color.OrangeRed);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush greenBrush = new SolidBrush(Color.Green);

        List<int> asteroidY = new List<int>();
        List<int> asteroidX = new List<int>();
        List<int> direction = new List<int>();

        Random randGen = new Random();

        SoundPlayer explosion = new SoundPlayer(Properties.Resources.explosion);
        SoundPlayer airHorn = new SoundPlayer(Properties.Resources.airHorn);
        SoundPlayer finishLine = new SoundPlayer(Properties.Resources.finishLine);

        #endregion

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;

                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;

                case Keys.Space:                   
                    running = true;
                    outputLabel.Visible = false;
                    instructionLabel.Visible = false;
                    p1ScoreLabel.Visible = true;
                    p2ScoreLabel.Visible = true;
                    break;

                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;

                case Keys.S:
                    sDown = false;
                    break;

                case Keys.Up:
                    upArrowDown = false;
                    break;

                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (running)
            {
                //start countdown timer
                countTimer.Enabled = true;

                //randomize starting positions and directions
                if (asteroidY.Count == 0)
                {
                    airHorn.Play();
                    for (int i = 0; i < 25; i++)
                    {
                        asteroidY.Add(randGen.Next(50, 400));
                        asteroidX.Add(randGen.Next(0, width));
                        int asteroidSpeed = randGen.Next(3, 10);
                        int positiveNegative = randGen.Next(0, 2);
                        if (positiveNegative == 1) { direction.Add(asteroidSpeed); }
                        else { direction.Add(asteroidSpeed * -1); }
                    }
                }

                //move asteroids
                for (int i = 0; i < asteroidX.Count; i++)
                {
                    asteroidX[i] += direction[i];
                    if (asteroidX[i] < 0) { direction[i] = direction[i] * -1; }
                    if (asteroidX[i] > width) { direction[i] = direction[i] * -1; }
                }

                #region Player Movement
                //move player1
                if (wDown == true && player1Y > 0)
                {
                    player1Y -= playerSpeed;
                }

                if (sDown == true && player1Y < height - playerHeight)
                {
                    player1Y += playerSpeed;
                }

                //move player2
                if (upArrowDown == true && player2Y > 0)
                {
                    player2Y -= playerSpeed;
                }

                if (downArrowDown == true && player2Y < height - playerHeight)
                {
                    player2Y += playerSpeed;
                }
                #endregion

                #region Player Intersection
                //P1
                Rectangle player1 = new Rectangle(player1X, player1Y, playerWidth, playerHeight);
                for (int i = 0; i < asteroidX.Count; i++)
                {
                    Rectangle asteroid = new Rectangle(asteroidX[i], asteroidY[i], asteroidWidth, asteroidHeight);
                    if (player1.IntersectsWith(asteroid))
                    {
                        player1X = 250;
                        player1Y = 450;
                        explosion.Play();
                    }
                }

                //P2
                Rectangle player2 = new Rectangle(player2X, player2Y, playerWidth, playerHeight);
                for (int i = 0; i < asteroidX.Count; i++)
                {
                    Rectangle asteroid = new Rectangle(asteroidX[i], asteroidY[i], asteroidWidth, asteroidHeight);
                    if (player2.IntersectsWith(asteroid))
                    {
                        player2X = 450;
                        player2Y = 450;
                        explosion.Play();
                    }
                }

                //end line
                Rectangle endLine = new Rectangle(0, 0, width, 50);
                if (player1.IntersectsWith(endLine))
                {
                    p1Score++;
                    player1X = 250;
                    player1Y = 450;
                    finishLine.Play();
                }
                if (player2.IntersectsWith(endLine))
                {
                    p2Score++;
                    player2X = 450;
                    player2Y = 450;
                    finishLine.Play();
                }
                #endregion

                p1ScoreLabel.Text = $"{p1Score}";
                p2ScoreLabel.Text = $"{p2Score}";                
            }
            else if (running == false)
            {
                #region clear and output winner
                player1X = 250;
                player1Y = 450;

                player2X = 450;
                player2Y = 450;

                timerLine = 37;

                

                asteroidX.Clear();
                asteroidY.Clear();
                direction.Clear();

                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                outputLabel.Visible = true;
                instructionLabel.Visible = true;

                if (endscreen)
                {
                    if (p1Score > p2Score) { outputLabel.Text = "PLAYER 1 WINS"; }
                    else if (p1Score < p2Score) { outputLabel.Text = "PLAYER 2 WINS"; }
                    else if (p1Score == p2Score) { outputLabel.Text = "DRAW"; }
                    endscreen = false;
                    p1Score = 0;
                    p2Score = 0;
                }              
                #endregion 
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (running)
            {
                //end line  
                e.Graphics.FillRectangle(greenBrush, 0, 50, width, 5);

                //draw players
                #region P1
                e.Graphics.DrawLine(drawPen, player1X + 5, player1Y, player1X + playerWidth -5, player1Y);
                e.Graphics.DrawLine(drawPen, player1X + 5, player1Y, player1X , player1Y + 5);
                e.Graphics.DrawLine(drawPen, player1X + playerWidth - 5, player1Y, player1X + playerWidth, player1Y + 5);

                e.Graphics.DrawLine(drawPen, player1X , player1Y + 5, player1X, player1Y + 20);
                e.Graphics.DrawLine(drawPen, player1X, player1Y + 20, player1X - 5, player1Y + 10);
                e.Graphics.DrawLine(drawPen, player1X + 3, player1Y + playerHeight, player1X - 5, player1Y + 20);
                e.Graphics.DrawLine(drawPen, player1X - 5, player1Y + 10, player1X - 5, player1Y + 20);

                e.Graphics.DrawLine(drawPen, player1X + playerWidth, player1Y + 5, player1X + playerWidth, player1Y + 20);
                e.Graphics.DrawLine(drawPen, player1X + playerWidth, player1Y + 20, player1X + playerWidth + 5, player1Y + 10);
                e.Graphics.DrawLine(drawPen, player1X + playerWidth - 3, player1Y + playerHeight, player1X + playerWidth + 5, player1Y + 20);
                e.Graphics.DrawLine(drawPen, player1X + playerWidth + 5, player1Y + 10, player1X + playerWidth + 5, player1Y + 20);

                e.Graphics.DrawLine(drawPen, player1X + playerWidth - 5, player1Y + playerHeight, player1X + 5, player1Y + playerHeight);
                #endregion
                drawPen.Color = Color.Red;
                #region P2
                e.Graphics.DrawLine(drawPen, player2X + 5, player2Y, player2X + playerWidth - 5, player2Y);
                e.Graphics.DrawLine(drawPen, player2X + 5, player2Y, player2X, player2Y + 5);
                e.Graphics.DrawLine(drawPen, player2X + playerWidth - 5, player2Y, player2X + playerWidth, player2Y + 5);

                e.Graphics.DrawLine(drawPen, player2X, player2Y + 5, player2X, player2Y + 20);
                e.Graphics.DrawLine(drawPen, player2X, player2Y + 20, player2X - 5, player2Y + 10);
                e.Graphics.DrawLine(drawPen, player2X + 3, player2Y + playerHeight, player2X - 5, player2Y + 20);
                e.Graphics.DrawLine(drawPen, player2X - 5, player2Y + 10, player2X - 5, player2Y + 20);

                e.Graphics.DrawLine(drawPen, player2X + playerWidth, player2Y + 5, player2X + playerWidth, player2Y + 20);
                e.Graphics.DrawLine(drawPen, player2X + playerWidth, player2Y + 20, player2X + playerWidth + 5, player2Y + 10);
                e.Graphics.DrawLine(drawPen, player2X + playerWidth - 3, player2Y + playerHeight, player2X + playerWidth + 5, player2Y + 20);
                e.Graphics.DrawLine(drawPen, player2X + playerWidth + 5, player2Y + 10, player2X + playerWidth + 5, player2Y + 20);

                e.Graphics.DrawLine(drawPen, player2X + playerWidth - 5, player2Y + playerHeight, player2X + 5, player2Y + playerHeight);
                #endregion
                drawPen.Color = Color.Blue;

                //draw asteroids
                for (int i = 0; i < asteroidY.Count; i++)
                {
                    int colourFlash = randGen.Next(0, 2);
                    if (colourFlash == 1)
                    { e.Graphics.FillRectangle(OrangeBrush, asteroidX[i], asteroidY[i], asteroidWidth, asteroidHeight); }
                    else
                    { e.Graphics.FillRectangle(whiteBrush, asteroidX[i], asteroidY[i], asteroidWidth, asteroidHeight); }
                }
                e.Graphics.FillRectangle(whiteBrush, 355, timerLine, 10, height);
            }
        }

        private void CountTimer_Tick(object sender, EventArgs e)
        {
            //makes the line move down and detects when it is off the screen
            timerLine++;
            if (timerLine > 500)
            {
                airHorn.Play();
                countTimer.Enabled = false;
                running = false;
                endscreen = true;
            }
        }
    }
}

