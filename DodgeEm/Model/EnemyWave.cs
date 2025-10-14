using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
{
    public class EnemyWave
    {
        public Color BallColor { get; } = Colors.White;
        public Direction BallDirection { get; }

        private Canvas CurrentCanvas;
        private double CanvasWidth;
        private double CanvasHeight;

        private DispatcherTimer timer;
        private int tickCount;
        private int ticksUntilNextBall = 1;
        private int delayMilliseconds; 
        private TimeSpan TickInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        public IList<EnemyBall> EnemyBalls { get; } = new List<EnemyBall>();

        private readonly Random Rand = new Random();


        public EnemyWave(Color color, Direction direction, int startWave, Canvas gameCanvas, double width, double height )
        {
            this.timer = new DispatcherTimer { Interval = this.TickInterval };

            this.BallColor = color;
            this.BallDirection = direction;

            this.CurrentCanvas = gameCanvas;
            this.CanvasWidth = width;
            this.CanvasHeight = height;
            this.delayMilliseconds = startWave;


            this.StartTimer();
        }

        /// <summary>
        /// Starts the internal timer for the wave.
        /// Precondition: None.
        /// Postcondition: Timer is running and ticks will occur.
        /// </summary>
        private void StartTimer()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Tick -= this.Timer_Tick;
            }

            
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
        }

        /// <summary>
        /// Stops the internal timer for the wave.
        /// Precondition: None.
        /// Postcondition: Timer is stopped and no further ticks will occur.
        /// </summary>
        public void StopTimer()
        {
            this.timer.Stop();
            this.timer.Tick -= this.Timer_Tick;
            
        }

        private void addRandomBallsToCanvas()
        {
            if (this.tickCount >= this.ticksUntilNextBall)
            {
                this.GenerateEnemyBall();

                this.getRandomTick(this.Rand);
                this.tickCount = 0;
            }
            else
            {
                this.tickCount++;
            }
        }

        private void getRandomTick(Random random) => this.ticksUntilNextBall = random.Next(GameSettings.MinTicksUntilNextBall, GameSettings.MaxTicksUntilNextBall);

        private void Timer_Tick(object sender, object e)
        { 
            if (this.delayMilliseconds > 0)
            {
                this.delayMilliseconds -= (int)this.TickInterval.TotalMilliseconds;
            }
            else
            {
                this.OnTick();
            }
        }

        private void OnTick()
        {
            this.addRandomBallsToCanvas();
            this.MoveEnemyBalls();

            if (this.CurrentCanvas != null)
            {
                this.RemoveOutOfBoundsBalls();
            }
        }

        private void RemoveOutOfBoundsBalls()
        {
            for (var i = this.EnemyBalls.Count - 1; i >= 0; i--)
            {
                var enemyBall = this.EnemyBalls[i];
                if (this.isOutOfBounds(enemyBall, this.CanvasWidth, this.CanvasHeight))
                {
                    this.CurrentCanvas.Children.Remove(enemyBall.Sprite);
                    this.EnemyBalls.RemoveAt(i);
                }
            }
        }

        private bool isOutOfBounds(EnemyBall ball, double width, double height)
        {
            switch (this.BallDirection)
            {
                case Direction.TopToBottom:
                    return ball.Y > height;
                case Direction.BottomToTop:
                    return ball.Y + GameSettings.PlayerBallMargin < 0;
                case Direction.LeftToRight:
                    return ball.X + GameSettings.PlayerBallMargin < 0;
                case Direction.RightToLeft:
                    return ball.X > width;
                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        private void MoveEnemyBalls()
        {
            foreach (var ball in this.EnemyBalls)
            {
                ball.Move();
            }
        }

        private void GenerateEnemyBall()
        {

            var speed = this.Rand.Next(GameSettings.MinSpeed, GameSettings.MaxSpeed);

            var ball = new EnemyBall(this.BallColor, this.BallDirection, speed);

            this.EnemyBalls.Add(ball);
            this.SetInitialPositions(ball);
            this.CurrentCanvas.Children.Add(ball.Sprite);
        }

        private void SetInitialPositions(EnemyBall ball)
        {
            switch (this.BallDirection)
            {
                case Direction.TopToBottom:
                    ball.X = Rand.Next(GameSettings.PlayerBallMargin, (int)(CanvasWidth - GameSettings.PlayerBallMargin));
                    ball.Y = -GameSettings.PlayerBallMargin;
                    break;

                case Direction.BottomToTop:
                    ball.X = Rand.Next(GameSettings.PlayerBallMargin, (int)(this.CanvasWidth - GameSettings.PlayerBallMargin));
                    ball.Y = this.CanvasHeight + GameSettings.PlayerBallMargin;
                    break;

                case Direction.RightToLeft:
                    ball.X = -GameSettings.PlayerBallMargin;
                    ball.Y = Rand.Next(GameSettings.PlayerBallMargin, (int)(CanvasHeight - GameSettings.PlayerBallMargin));
                    break;

                case Direction.LeftToRight:
                    ball.X = this.CanvasHeight + GameSettings.PlayerBallMargin;
                    ball.Y = Rand.Next(GameSettings.PlayerBallMargin, (int)(CanvasHeight - GameSettings.PlayerBallMargin));
                    break;

                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }
    }
}