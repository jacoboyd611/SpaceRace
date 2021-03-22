using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region Variables
        int width = 683;
        int height = 460;

        int player1X = 250;
        int player1Y = 450;


        int player2X = 450;
        int player2Y = 450;

        int playerWidth = 10;
        int playerHeight = 30;
        int playerSpeed = 7;

        int asteroidWidth = 5;
        int asteroidHeight = 5;
        int asteroidSpeed = 5;

        bool wDown = false;
        bool sDown = false;

        bool upArrowDown = false;
        bool downArrowDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        List<int> asteroidY = new List<int>();
        List<int> asteroidX = new List<int>();
        List<int> direction = new List<int>();

        Random randGen = new Random();
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
            //randomize starting positions and directions
            if (asteroidY.Count == 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    asteroidY.Add(randGen.Next(50, 400));
                    asteroidX.Add(randGen.Next(0, width));
                    int positiveNegative = randGen.Next(0, 2);
                    if (positiveNegative == 1) { direction.Add(asteroidSpeed); }
                    else { direction.Add(asteroidSpeed*-1); }
                }
            }

            //move ball
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
                }
            }

            #endregion

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw players
            e.Graphics.FillRectangle(whiteBrush, player1X, player1Y, playerWidth, playerHeight);
            e.Graphics.FillRectangle(whiteBrush, player2X, player2Y, playerWidth, playerHeight);

            e.Graphics.FillRectangle(whiteBrush, 355, 50, 10, 800);

            //draw asteroids
            for (int i = 0; i < asteroidY.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, asteroidX[i], asteroidY[i], asteroidWidth, asteroidHeight);         
            }
        }
    }
}
