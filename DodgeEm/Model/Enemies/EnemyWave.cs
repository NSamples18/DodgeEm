using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    ///     Represents a wave of enemy balls.
    /// </summary>
    public class EnemyWave
    {
        #region Data members

        private readonly Color ballColor;
        private readonly Direction ballDirection;
        public readonly LevelId levelId;

        private readonly Canvas currentCanvas;
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        private DispatcherTimer timer;
        private int tickCount;
        private int ticksUntilNextBall = 1;
        private int delayMilliseconds;
        private int currentDelay;
        private DateTime lastTickTime = DateTime.Now;
        private readonly TimeSpan tickInterval = TimeSpan.FromMilliseconds(20);

        private bool stopGeneratingBalls = false;

        private readonly Random random = new Random();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the list of enemy balls in the wave.
        /// </summary>
        public IList<EnemyBall> EnemyBalls { get; } = new List<EnemyBall>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyWave" /> class.
        /// </summary>
        /// <param name="color">The color of the enemy balls.</param>
        /// <param name="direction">The Direction of the enemy balls.</param>
        /// <param name="startWave">The starting wave number.</param>
        /// <param name="gameCanvas">The canvas to draw the enemy balls on.</param>
        /// <param name="width">The width of the game area.</param>
        /// <param name="height">The height of the game area.</param>
        public EnemyWave(LevelId levelId,Color color, Direction direction, int startWave, Canvas gameCanvas, double width,
            double height)
        {
            this.timer = new DispatcherTimer { Interval = this.tickInterval };

            this.ballColor = color;
            this.ballDirection = direction;
            this.levelId = levelId;

            this.currentCanvas = gameCanvas;
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.delayMilliseconds = startWave;
            this.currentDelay = startWave;
        }

        #endregion

        #region Methods

        public void RemoveAllBalls()
        {
            foreach (var enemyBall in this.EnemyBalls)
            {
                this.currentCanvas.Children.Remove(enemyBall.Sprite);
            }
            this.EnemyBalls.Clear();
        }

        public void resetWaveTimer()
        {
            this.StopTimer();
            this.currentDelay = this.delayMilliseconds;
            this.ticksUntilNextBall = 1;
            this.lastTickTime = DateTime.Now;
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
            this.StartWave();
        }

        /// <summary>
        ///     Stops the internal timer for the wave.
        ///     Precondition: None.
        ///     Postcondition: Timer is stopped and no further ticks will occur.
        /// </summary>
        public void StopTimer()
        {
            this.timer.Stop();
            this.timer.Tick -= this.Timer_Tick;
        }



        /// <summary>
        ///     Starts the internal timer for the wave.
        ///     Precondition: None.
        ///     Postcondition: Timer is running and ticks will occur.
        /// </summary>
        public void StartWave()
        {
            if (this.timer != null)
            {
                this.timer.Tick += this.Timer_Tick;
                this.timer.Start();
            }
        }

        private void addRandomBallsToCanvas()
        {
            if (this.tickCount >= this.ticksUntilNextBall)
            {
                this.generateEnemyBall();

                this.getRandomTick(this.random);
                this.tickCount = 0;
            }
            else
            {
                this.tickCount++;
            }
        }

        private void getRandomTick(Random randomTick)
        {
            this.ticksUntilNextBall =
                randomTick.Next(GameSettings.MinTicksUntilNextBall, GameSettings.MaxTicksUntilNextBall);
        }

        private void Timer_Tick(object sender, object e)
        {
            var currTime = DateTime.Now;
            var timerTotalMilliseconds = (currTime - this.lastTickTime).TotalMilliseconds;
            this.lastTickTime = currTime;

            if (this.currentDelay > 0)
            {
                this.currentDelay -= (int)timerTotalMilliseconds;
            }
            else
            {
                this.OnTick();
            }
        }

        private void OnTick()
        {
            this.addRandomBallsToCanvas();
            this.moveEnemyBalls();

            if (this.currentCanvas != null)
            {
                this.removeOutOfBoundsBalls();
            }
        }

        private void removeOutOfBoundsBalls()
        {
            for (var i = this.EnemyBalls.Count - 1; i >= 0; i--)
            {
                var enemyBall = this.EnemyBalls[i];
                if (isOutOfBounds(enemyBall, this.canvasWidth, this.canvasHeight))
                {
                    this.currentCanvas.Children.Remove(enemyBall.Sprite);
                    this.EnemyBalls.RemoveAt(i);
                }
            }
        }

        private static bool isOutOfBounds(EnemyBall ball, double width, double height)
        {
            switch (ball.Direction)
            {
                case Direction.TopToBottom:
                    return ball.Y > height;

                case Direction.BottomToTop:
                    return ball.Y + ball.Height < 0;

                case Direction.LeftToRight:
                    return ball.X > width;

                case Direction.RightToLeft:
                    return ball.X + ball.Width < 0;
                case Direction.VerticalMixed:
                    throw new InvalidOperationException(
                        "VerticalMixed should be replaced with a specific direction before calling Move().");

                default:
                    throw new InvalidOperationException("Unknown Direction");
            }
        }

        private void moveEnemyBalls()
        {
            foreach (var ball in this.EnemyBalls)
            {
                ball.Move();
            }
        }

        private void generateEnemyBall()
        {
                var direction2 = this.ballDirection;
                var speed = this.random.Next(GameSettings.MinSpeed, GameSettings.MaxSpeed);
                if (this.ballDirection == Direction.VerticalMixed)
                {
                    direction2 = this.randomBlitzDirection();
                    speed = this.random.Next(GameSettings.MinSpeed, GameSettings.BlitzSpeed);
                }

                var ball = new EnemyBall(this.ballColor, direction2, speed);

                this.EnemyBalls.Add(ball);
                this.setInitialPositions(ball);
                this.currentCanvas.Children.Add(ball.Sprite);
            
        }

        private Direction randomBlitzDirection()
        {
            Direction[] blitzDirections = { Direction.TopToBottom, Direction.BottomToTop };

            return blitzDirections[this.random.Next(blitzDirections.Length)];
        }

        private void setInitialPositions(EnemyBall ball)
        {
            var marginX = this.canvasWidth - ball.Width;
            var marginY = this.canvasHeight - ball.Height;

            switch (ball.Direction)
            {
                case Direction.TopToBottom:
                    ball.X = this.random.Next((int)ball.Width, (int)marginX);
                    ball.Y = -ball.Height;
                    break;

                case Direction.BottomToTop:
                    ball.X = this.random.Next((int)ball.Width, (int)marginX);
                    ball.Y = this.canvasHeight + ball.Height;
                    break;

                case Direction.RightToLeft:
                    ball.X = this.canvasWidth + ball.Width;
                    ball.Y = this.random.Next((int)ball.Height, (int)marginY);
                    break;

                case Direction.LeftToRight:
                    ball.X = -ball.Width;
                    ball.Y = this.random.Next((int)ball.Height, (int)marginY);
                    break;
                case Direction.VerticalMixed:
                    throw new InvalidOperationException(
                        "VerticalMixed should be replaced with a specific direction before calling Move().");

                default:
                    throw new InvalidOperationException("Unknown Direction");
            }
        }

        #endregion
    }
}