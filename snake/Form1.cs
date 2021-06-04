using System;
using System.Drawing;
using System.Windows.Forms;

namespace snake
{
    public partial class frmSnake : Form
    {
        Random rand;
        // Een Enum voor het type vakje op het bord. 
        enum GameBoardFields
        {
            Free,
            Snake,
            Bonus, 
            Bomb
        }; 
        // Een eenum voor de type directions. 
        enum Directions
        {
            Up, 
            Down, 
            Left, 
            Right
        };
        // Een structure voor de cordinaten van de snake. 
        struct SnakeCoordinates
        {
            public int x;
            public int y; 
        }
        
        GameBoardFields[,] GameBoardField;
        SnakeCoordinates[] snakeXY;
        int snakeLength;
        Directions direction;
        Graphics g;


        public frmSnake()
        {
            InitializeComponent();
            GameBoardField = new GameBoardFields[11, 11];
            snakeXY = new SnakeCoordinates[100];
            rand = new Random(); 

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Wat doet dit?
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picGameBoard.Image = new Bitmap(420, 420);
            g = Graphics.FromImage(picGameBoard.Image);
            g.Clear(Color.White);
            // Maakt het picture 420x420. Een vakje is 35 pixels, dus het speelveeld is 10x10 vakjes en het totale bord met rand is 12x12.

            for (int i = 1; i <= 10; i++)
            {
                g.DrawImage(imgList.Images[3], i * 35, 0);
                g.DrawImage(imgList.Images[3], i * 35, 385);
            }

            for (int i = 0; i <= 11; i++)
            {
                g.DrawImage(imgList.Images[3], 0,  i * 35);
                g.DrawImage(imgList.Images[3], 385, i * 35);
            }
            // Deze twee tekenen de rand van het speelvelt met de juiste afbeelding. 

            // Dit stelt vast waar de slang begint. Het hoofd [0] begint op 5,5 en het lichaam 1 en 2 komen daar onder. 
            snakeXY[0].x = 5; 
            snakeXY[0].y = 5;
            snakeXY[1].x = 5; 
            snakeXY[1].y = 6;
            snakeXY[2].x = 5; 
            snakeXY[2].y = 7;

            // Dit tekent de beginslang. 
            g.DrawImage(imgList.Images[2], 5 * 35, 5 * 35);
            g.DrawImage(imgList.Images[1], 5 * 35, 6 * 35); 
            g.DrawImage(imgList.Images[1], 5 * 35, 7 * 35); 

            // Dit zet de vakjes waar de slang start vast als Snake. 
            GameBoardField[5, 5] = GameBoardFields.Snake;
            GameBoardField[5, 6] = GameBoardFields.Snake;
            GameBoardField[5, 7] = GameBoardFields.Snake;

            // Dit stelt de start lengte en richting in. 
            direction = Directions.Up;
            snakeLength = 3; 

            // Dit tekent 4 appels aan het begin van het spel. 
            for(int i = 0; i < 4; i++)
            {
                Bonus();
            }
        }

        // Dit is de methode voor het kiezen van een random vakje op het veld. Vervolgens kijkt het of het vakje leeg is. 
        // Als dat klopt maakt het het vakje Bonus en tekent het de bonus op het bord.
        private void Bonus()
        {
            int x, y;

            do
            {
                x = rand.Next(1, 10);
                y = rand.Next(1, 10);

            }
            while (GameBoardField[x, y] != GameBoardFields.Free);

            GameBoardField[x, y] = GameBoardFields.Bonus;
            g.DrawImage(imgList.Images[0], x * 35, y * 35); 
        }

        // Het zelfde as hierboven, maar dan een bomb in plaats van een bonus. 
        private void Bomb()
        {
            int x, y;

            do
            {
                x = rand.Next(1, 10);
                y = rand.Next(1, 10);

            }
            while (GameBoardField[x, y] != GameBoardFields.Free);

            GameBoardField[x, y] = GameBoardFields.Bomb;
            g.DrawImage(imgList.Images[4], x * 35, y * 35);
        }

        // Dit zet de direction van de slang wat correspondeerd met de pijltjestoetsen. 
            private void frmSnake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = Directions.Up;
                    break;
                case Keys.Down:
                    direction = Directions.Down;
                    break;
                case Keys.Left:
                    direction = Directions.Left;
                    break;
                case Keys.Right:
                    direction = Directions.Right;
                    break; 
            }
        }

        // Een methode voor als je game over bent. 
        private void GameOver()
        {
            timer1.Enabled = false;
            MessageBox.Show("GAME OVER"); 
        }

        // Zolang de timer actief is. Dit gebeurd elke 500 miliseconden door een Tick event.
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Teken het laatste vakje van de slang wit. Zet vervolgens het vakje als Free. 
            g.FillRectangle(Brushes.White, snakeXY[snakeLength - 1].x * 35, 
                snakeXY[snakeLength - 1].y*35, 35, 35);
            GameBoardField[snakeXY[snakeLength - 1].x, snakeXY[snakeLength - 1].y] = GameBoardFields.Free;

            // De coordinaten van het laatste segment worden verandert naar de coordinaten van het eenalaatse segment, en zo voort. Tot aan het hoofd.
            for (int i = snakeLength; i >= 1; i--)
            {
                snakeXY[i].x = snakeXY[i - 1].x;
                snakeXY[i].y = snakeXY[i - 1].y;
            }
            // Het lichaam wordt getekend voor de slang. 
            g.DrawImage(imgList.Images[1], snakeXY[0].x * 35, snakeXY[0].y * 35);

            // De locatie van het hoofd wordt aangepast aan de hand van de richting. 
            switch (direction)
            {
                case Directions.Up:
                    snakeXY[0].y = snakeXY[0].y - 1;
                    break;
                case Directions.Down:
                    snakeXY[0].y = snakeXY[0].y + 1;
                    break;
                case Directions.Left:
                    snakeXY[0].x = snakeXY[0].x - 1;
                    break;
                case Directions.Right:
                    snakeXY[0].x = snakeXY[0].x + 1;
                    break;
            }

            // Check of de slang buiten het speelveld valt en dus tegen de rand is. Als dat zo is, game over. 
             if (snakeXY[0].x < 1 || snakeXY[0].x > 10 || snakeXY[0].y < 1 || snakeXY[0].y >10)
            {
                GameOver();
                picGameBoard.Refresh();
                return; 
            }

            // Check of het hoofd van de slang overlapt met een deel van de slang. Als dat zo is, game over. 
            if (GameBoardField[snakeXY[0].x, snakeXY[0].y] == GameBoardFields.Snake)
            {
                GameOver();
                picGameBoard.Refresh();
                return; 
            }

            // Check of het hoofd van de slang overlapt met een deel van de slang. Als dat zo is, game over. 
            if (GameBoardField[snakeXY[0].x, snakeXY[0].y] == GameBoardFields.Bomb)
            {
                GameOver();
                picGameBoard.Refresh();
                return;
            }

            // Check of het hoofd van de slang overlapt met een deel van de slang. Als dat zo is, teken een extra slang segment
            // (wat zichbaar wordt op de refresh en dus op de juiste locatie is, ipv van -1 of zo iets). Zet dat vakje als een Snake en maak de length langer. 
            // Vervolgens kijken of het bord niet te vol is en dan een nieuwe bonus spawnen. 
            // Als de slang lang genoeg is, spawn een bomb. Als laatste de titel van het form zetten naar de score, de length min het begin 3 segmenten.
            if (GameBoardField[snakeXY[0].x, snakeXY[0].y] == GameBoardFields.Bonus)
            {
                g.DrawImage(imgList.Images[1], snakeXY[snakeLength].x * 35,
                    snakeXY[snakeLength].y * 35);
                GameBoardField[snakeXY[snakeLength].x, snakeXY[snakeLength].y] = GameBoardFields.Snake;

                snakeLength++;

                if (snakeLength < 96)
                    Bonus();
                if (12 < snakeLength || snakeLength < 96)
                    Bomb(); 
                this.Text = "Snake - score: " + (snakeLength -3);
               

            }

            // Als laatste tekent het een hoofd van de slang en zet het het vakje naar Snake.  
            g.DrawImage(imgList.Images[2], snakeXY[0].x * 35, snakeXY[0].y * 35);
            GameBoardField[snakeXY[0].x, snakeXY[0].y] = GameBoardFields.Snake;
            // Het wordt gerefreshed en de veranderingen in de visuals worden zichtbaar. 
            picGameBoard.Refresh(); 
        }
    }
}
