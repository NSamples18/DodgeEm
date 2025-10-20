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

        private readonly Canvas currentCanvas;
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private int ticksUntilNextBall = 1;
        private int delayMilliseconds;
        private DateTime lastTickTime = DateTime.Now;
        private readonly TimeSpan tickInterval = TimeSpan.FromMilliseconds(20);

        private readonly Random random = new Random();

        #endregion

        #region Properties

        public IList<EnemyBall> EnemyBalls { get; } = new List<EnemyBall>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyWave" /> class.
        /// </summary>
        /// <param name="color">The color of the enemy balls.</param>
        /// <param name="direction">The direction of the enemy balls.</param>
        /// <param name="startWave">The starting wave number.</param>
        /// <param name="gameCanvas">The canvas to draw the enemy balls on.</param>
        /// <param name="width">The width of the game area.</param>
        /// <param name="height">The height of the game area.</param>
        public EnemyWave(Color color, Direction direction, int startWave, Canvas gameCanvas, double width,
            double height)
        {
            this.timer = new DispatcherTimer { Interval = this.tickInterval };

            this.ballColor = color;
            this.ballDirection = direction;

            this.currentCanvas = gameCanvas;
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.delayMilliseconds = startWave;

            this.startTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Starts the internal timer for the wave.
        ///     Precondition: None.
        ///     Postcondition: Timer is running and ticks will occur.
        /// </summary>
        private void startTimer()
        {
            if (this.timer != null)
            {
                this.timer.Tick += this.Timer_Tick;
                this.timer.Start();
            }
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

        private void getRandomTick(Random random)
        {
            this.ticksUntilNextBall =
                random.Next(GameSettings.MinTicksUntilNextBall, GameSettings.MaxTicksUntilNextBall);
        }

        private void Timer_Tick(object sender, object e)
        {
            var currTime = DateTime.Now;
            var timerTotalMilliseconds = (currTime - this.lastTickTime).TotalMilliseconds;
            this.lastTickTime = currTime;

            if (this.delayMilliseconds > 0)
            {
                this.delayMilliseconds -= (int)timerTotalMilliseconds;
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
                if (this.isOutOfBounds(enemyBall, this.canvasWidth, this.canvasHeight))
                {
                    this.currentCanvas.Children.Remove(enemyBall.Sprite);
                    this.EnemyBalls.RemoveAt(i);
                }
            }
        }

        private bool isOutOfBounds(EnemyBall ball, double width, double height)
        {
            switch (ball.direction)
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
            if (this.ballDirection == Direction.All)
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
            switch (ball.direction)
            {
                case Direction.TopToBottom:
                    ball.X = this.random.Next(GameSettings.PlayerBallMargin,
                        (int)(this.canvasWidth - GameSettings.PlayerBallMargin));
                    ball.Y = -GameSettings.PlayerBallMargin;
                    break;

                case Direction.BottomToTop:
                    ball.X = this.random.Next(GameSettings.PlayerBallMargin,
                        (int)(this.canvasWidth - GameSettings.PlayerBallMargin));
                    ball.Y = this.canvasHeight + GameSettings.PlayerBallMargin;
                    break;

                case Direction.RightToLeft:
                    ball.X = -GameSettings.PlayerBallMargin;
                    ball.Y = this.random.Next(GameSettings.PlayerBallMargin,
                        (int)(this.canvasHeight - GameSettings.PlayerBallMargin));
                    break;

                case Direction.LeftToRight:
                    ball.X = this.canvasHeight + GameSettings.PlayerBallMargin;
                    ball.Y = this.random.Next(GameSettings.PlayerBallMargin,
                        (int)(this.canvasHeight - GameSettings.PlayerBallMargin));
                    break;

                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        #endregion
    }
}