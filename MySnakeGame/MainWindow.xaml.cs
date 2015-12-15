using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MySnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Objects
        int HighScore { get { return int.Parse(HighScoreBox.Content.ToString()); } set { HighScoreBox.Content = value.ToString(); } }
        bool GoodMode;
        static Random Ran;
        List<Point> TheSnake = new List<Point>();
        Point PointsToTake = new Point();
        Point CurrentPosition;
        int StartTime { get { return int.Parse(VelocityBox.Content.ToString()); } set { VelocityBox.Content = value.ToString(); } }
        DispatcherTimer Speed;

        int SnakeLenght { get { return int.Parse(SizeBox.Content.ToString()); } set { SizeBox.Content = value.ToString(); } }
        int Score { get { return int.Parse(ScoreBox.Content.ToString()); } set { ScoreBox.Content = value.ToString(); } }
        enum Direction { STOPED, UP, DOWN, LEFT, RIGHT }
        Direction CurrentDirection;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            HighScore = 0;
            Ran = new Random();
            this.KeyDown += Keyboard;
            NewGame(false);


        }
        void NewGame(bool renew)
        {
            TheSnake.Clear();
            //Reset the game, the "while" will erase all points in the carval
            while (Area.Children.Count > 0)
            {
                //The difference maximum that can be is 1 so the program will delete a point
                Area.Children.RemoveAt(0);

            }
            CurrentPosition = new Point(80, 80);//The start position in the carval
            GoodMode = false;
            StartTime = 0; // The snake velocity
            CurrentDirection = Direction.STOPED; // start the game with the snake stopped 
            Score = 0;
            SnakeLenght = 10;
            #region new game
            if (!renew) //if is a new game
            {
                Ran = new Random(); // start the object random
                Speed = new DispatcherTimer(); // 
                Speed.Interval = new TimeSpan(0, 0, 0, 0, 60); // Speed Game
                Speed.Tick += new EventHandler(MainGameFunctionReference); // 





                Speed.Start();


            }
            else
            {
                //if is a "renewed game"then only will change the speed interval
                Speed.Interval = new TimeSpan(0, 0, 0, 0, 60);
            }
            #endregion

            DrawSnake();
            NewPoint();
        }

        private void Keyboard(object sender, KeyEventArgs e)
        {


            switch (e.Key)
            {
                case Key.Up:
                    if (CurrentDirection != Direction.DOWN)
                        CurrentDirection = Direction.UP;
                    break;
                case Key.Down:
                    if (CurrentDirection != Direction.UP)
                        CurrentDirection = Direction.DOWN;
                    break;
                case Key.Right:
                    if (CurrentDirection != Direction.LEFT)
                        CurrentDirection = Direction.RIGHT;
                    break;
                case Key.P:
                    CurrentDirection = Direction.STOPED;
                    break;
                case Key.Left:
                    if (CurrentDirection != Direction.RIGHT)
                        CurrentDirection = Direction.LEFT;
                    break;
                case Key.F9:
                    CurrentDirection = Direction.STOPED;
                    MessageBox.Show("My snake game version 1.0\nCreate by David Aben Athar © 2015", "About my snake game", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case Key.F2:
                    CurrentDirection = Direction.STOPED;
                    if (!GoodMode)
                    {
                        MessageBox.Show("Good mode activated!\nWhen you lose, the snake will stop and hen you can go slowly to safe place", "Good Mode", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        GoodMode = true;
                    }
                    else
                    {
                        MessageBox.Show("Good mode deactivated!");
                        GoodMode = false;
                    }
                    break;
                default: break;

            }
        }

        void DrawSnake()
        {
            Ellipse snakeDraw = new Ellipse();
            snakeDraw.Fill = Brushes.Green;
            snakeDraw.Width = 8;
            snakeDraw.Height = 8;
            Canvas.SetTop(snakeDraw, CurrentPosition.Y);
            Canvas.SetLeft(snakeDraw, CurrentPosition.X);
            Area.Children.Add(snakeDraw);
            TheSnake.Add(CurrentPosition);
            int PointInTheArea = Area.Children.Count;
            while (PointInTheArea > SnakeLenght)
            {
                //The difference maximum that can be is 1 so the program will delete a point
                Area.Children.RemoveAt(1);
                TheSnake.RemoveAt(0);
                PointInTheArea--;
            }



        }

        void NewPoint()
        {
            //Because the snake are 8x8, the point need to be in places that are multiplies of 8
            Point RandomPlace = new Point(8 * Ran.Next(0, ((int)Area.Width - 20) / 8), (8 * Ran.Next(0, ((int)Area.Height - 20) / 8)));
            Ellipse point = new Ellipse();
            point.Fill = Brushes.Red;
            //The point have the same size than snake
            point.Width = 8;
            point.Height = 8;
            //--
            Canvas.SetTop(point, RandomPlace.Y);
            Canvas.SetLeft(point, RandomPlace.X);
            Area.Children.Insert(0, point);
            PointsToTake = RandomPlace;





        }
        void MainGameFunctionReference(object sender, EventArgs e)
        {
            MainGameFunction(0);
        }
        void MainGameFunction(int counter)
        {
            if (counter > 0)
                MainGameFunction(--counter);
            switch (CurrentDirection)
            {
                case Direction.STOPED:
                    break;
                case Direction.UP:
                    CurrentPosition.Y -= 8;
                    DrawSnake();
                    break;
                case Direction.DOWN:
                    CurrentPosition.Y += 8;
                    DrawSnake();
                    break;
                case Direction.LEFT:
                    CurrentPosition.X -= 8;
                    DrawSnake();
                    break;
                case Direction.RIGHT:
                    CurrentPosition.X += 8;
                    DrawSnake();
                    break;
                default:
                    break;
            }

            bool StopInWall = false;
            if (StopInWall)
            {
                if (CurrentPosition.X < 0 || CurrentPosition.Y < 0 || CurrentPosition.X > 480 || CurrentPosition.Y > Area.Height)
                    GameOver();
            }
            else
            {
                if (CurrentPosition.X < 0)
                {
                    CurrentPosition.X = 480;

                }
                if (CurrentPosition.Y < 0)
                {
                    CurrentPosition.Y = Area.Height;

                }
                if (CurrentPosition.X > 480)
                {
                    CurrentPosition.X = 0;

                }
                if (CurrentPosition.Y > Area.Height)
                {
                    CurrentPosition.Y = 0;

                }
            }
            for (int i = 0; i < (TheSnake.Count - 2); i++)
            {

                if ((Math.Abs(TheSnake[i].X - CurrentPosition.X) < 8) && (Math.Abs(TheSnake[i].Y - CurrentPosition.Y) < 8))
                {
                    GameOver();
                    break;
                }


            }
            int lastIndex = (TheSnake.Count > 1) ? TheSnake.Count - 1 : 0;
            if (Math.Abs(PointsToTake.Y - TheSnake[lastIndex].Y) < 4 && Math.Abs(PointsToTake.X - TheSnake[lastIndex].X) < 8)
            {
                int increase = (int)((((float)(Score) % 1000) / (float)100) * 2) + 1; //increasing speed 
                //speed maximum 7 (60 - 10*3 = 30 ms)
                StartTime = (StartTime < increase && StartTime < 11) ? increase : StartTime;
                Speed.Interval = new TimeSpan(0, 0, 0, 0, 60 - StartTime * 3);
                SnakeLenght += 1;
                Score += 3 + StartTime * 2;
                Area.Children.RemoveAt(0);
                NewPoint();
            }

        }
        void GameOver()
        {
            if (GoodMode)
                CurrentDirection = Direction.STOPED;
            else
            {

                MessageBox.Show("Game Over\nScore: " + Score, "Game over", MessageBoxButton.OK, MessageBoxImage.Stop);
                //   VelocityIncrease.Stop();
                if (Score > HighScore)
                    HighScore = Score;
                NewGame(true);
            }
        }







    }
}
