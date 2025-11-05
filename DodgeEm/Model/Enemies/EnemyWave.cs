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

        public readonly LevelId levelId;

        private readonly Direction ballDirection;

        private readonly Canvas currentCanvas;
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        private DispatcherTimer timer;
        private int tickCount;
        private int ticksUntilNextBall = 1;
        private readonly int delayMilliseconds;
        private int currentDelay;
        private DateTime lastTickTime;
        private readonly TimeSpan tickInterval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs);

        private readonly Random random = new Random();

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the level identifier.
        /// </summary>
        /// <value>
        ///     The level identifier.
        /// </value>
        public LevelId LevelId { get; private set; }

        /// <summary>
        ///     Gets the list of enemy balls in the wave.
        /// </summary>
        public IList<EnemyBall> EnemyBalls { get; } = new List<EnemyBall>();

        /// <summary>
        ///     Gets the color of the enemy balls.
        /// </summary>
        public Color BallColor { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyWave" /> class.
        /// </summary>
        /// <param name="levelId">The level identifier.</param>
        /// <param name="color">The color of the enemy balls.</param>
        /// <param name="direction">The Direction of the enemy balls.</param>
        /// <param name="startWave">The starting wave number.</param>
        /// <param name="gameCanvas">The canvas to draw the enemy balls on.</param>
        /// <param name="width">The width of the game area.</param>
        /// <param name="height">The height of the game area.</param>
        public EnemyWave(LevelId levelId, Color color, Direction direction, int startWave, Canvas gameCanvas,
            double width,
            double height)
        {
            this.timer = null;

            this.BallColor = color;
            this.ballDirection = direction;
            this.LevelId = levelId;

            this.currentCanvas = gameCanvas;
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.delayMilliseconds = startWave;
            this.currentDelay = startWave;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes all enemy balls from the wave and the canvas.
        /// </summary>
        public void RemoveAllBalls()
        {
            foreach (var enemyBall in this.EnemyBalls)
            {
                this.currentCanvas.Children.Remove(enemyBall.Sprite);
            }

            this.EnemyBalls.Clear();
        }

        /// <summary>
        ///     Resets the wave to its initial state.
        /// </summary>
        public void ResetWave()
        {
            this.StopTimer();
            this.RestartWaveTimer();
            this.StartWave();
        }

        /// <summary>
        ///     Restarts the wave timer.
        /// </summary>
        public void RestartWaveTimer()
        {
            this.currentDelay = this.delayMilliseconds;
            this.ticksUntilNextBall = 1;
            this.lastTickTime = DateTime.Now;
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
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
            if (this.timer == null)
            {
                this.RestartWaveTimer();
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
                    return ball.YCord > height;

                case Direction.BottomToTop:
                    return ball.YCord + ball.Height < 0;

                case Direction.LeftToRight:
                    return ball.XCord > width;

                case Direction.RightToLeft:
                    return ball.XCord + ball.Width < 0;
                case Direction.BottomLeft:
                    return ball.YCord + ball.Height < 0 || ball.XCord > width;
                case Direction.BottomRight:
                    return ball.YCord + ball.Height < 0 || ball.XCord + ball.Width < 0;
                case Direction.TopLeft:
                    return ball.YCord > height || ball.XCord > width;
                case Direction.TopRight:
                    return ball.YCord > height || ball.XCord + ball.Width < 0;

                case Direction.VerticalMixed:
                case Direction.DiagonalMixed:
                    throw new InvalidOperationException(
                        $"{ball.Direction} should be replaced with a specific direction before calling Move().");

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
            var direction = this.ballDirection;
            var speed = this.random.Next(GameSettings.MinSpeed, GameSettings.MaxSpeed);
            if (this.ballDirection == Direction.VerticalMixed)
            {
                direction = this.randomBlitzDirection();
                speed = this.random.Next(GameSettings.MinSpeed, GameSettings.BlitzSpeed);
            }
            else if (this.ballDirection == Direction.DiagonalMixed)
            {
                direction = this.randomDiagonalDirection();
                speed = this.random.Next(GameSettings.MinSpeed, GameSettings.BlitzSpeed);
            }

            var ball = new EnemyBall(this.BallColor, direction, speed);

            this.EnemyBalls.Add(ball);
            this.setInitialPositions(ball);
            this.currentCanvas.Children.Add(ball.Sprite);
        }

        private Direction randomBlitzDirection()
        {
            Direction[] blitzDirections = { Direction.TopToBottom, Direction.BottomToTop };

            return blitzDirections[this.random.Next(blitzDirections.Length)];
        }

        private Direction randomDiagonalDirection()
        {
            Direction[] diagonalDirections =
            {
                Direction.BottomLeft,
                Direction.BottomRight,
                Direction.TopLeft,
                Direction.TopRight
            };
            return diagonalDirections[this.random.Next(diagonalDirections.Length)];
        }

        private void setInitialPositions(EnemyBall ball)
        {
            var marginX = this.canvasWidth - ball.Width;
            var marginY = this.canvasHeight - ball.Height;

            switch (ball.Direction)
            {
                case Direction.TopToBottom:
                    this.setTopSpawn(ball, marginX);
                    break;
                case Direction.BottomToTop:
                    this.setBottomSpawn(ball, marginX);
                    break;
                case Direction.RightToLeft:
                    this.setRightSpawn(ball, marginY);
                    break;
                case Direction.LeftToRight:
                    this.setLeftSpawn(ball, marginY);
                    break;
                case Direction.BottomLeft:
                    this.setBottomLeftCornerSpawn(ball);
                    break;
                case Direction.BottomRight:
                    this.setBottomRightCornerSpawn(ball);
                    break;
                case Direction.TopLeft:
                    this.setTopLeftCornerSpawn(ball);
                    break;
                case Direction.TopRight:
                    this.setTopRightCornerSpawn(ball);
                    break;
                case Direction.VerticalMixed:
                case Direction.DiagonalMixed:
                    throw new InvalidOperationException(
                        $"{ball.Direction} should be replaced with a specific direction before calling Move().");
                default:
                    throw new InvalidOperationException("Unknown Direction");
            }
        }

        private void setTopSpawn(EnemyBall ball, double marginX)
        {
            ball.XCord = this.random.Next((int)ball.Width, (int)marginX);
            ball.YCord = -ball.Height;
        }

        private void setBottomSpawn(EnemyBall ball, double marginX)
        {
            ball.XCord = this.random.Next((int)ball.Width, (int)marginX);
            ball.YCord = this.canvasHeight + ball.Height;
        }

        private void setRightSpawn(EnemyBall ball, double marginY)
        {
            ball.XCord = this.canvasWidth + ball.Width;
            ball.YCord = this.random.Next((int)ball.Height, (int)marginY);
        }

        private void setLeftSpawn(EnemyBall ball, double marginY)
        {
            ball.XCord = -ball.Width;
            ball.YCord = this.random.Next((int)ball.Height, (int)marginY);
        }

        private void setBottomLeftCornerSpawn(EnemyBall ball)
        {
            if (this.shouldSpawnFromPrimarySide())
            {
                this.setLeftSideBottomHalfSpawn(ball);
            }
            else
            {
                this.setBottomSideLeftHalfSpawn(ball);
            }
        }

        private void setBottomRightCornerSpawn(EnemyBall ball)
        {
            if (this.shouldSpawnFromPrimarySide())
            {
                this.setRightSideBottomHalfSpawn(ball);
            }
            else
            {
                this.setBottomSideRightHalfSpawn(ball);
            }
        }

        private void setTopLeftCornerSpawn(EnemyBall ball)
        {
            if (this.shouldSpawnFromPrimarySide())
            {
                this.setLeftSideTopHalfSpawn(ball);
            }
            else
            {
                this.setTopSideLeftHalfSpawn(ball);
            }
        }

        private void setTopRightCornerSpawn(EnemyBall ball)
        {
            if (this.shouldSpawnFromPrimarySide())
            {
                this.setRightSideTopHalfSpawn(ball);
            }
            else
            {
                this.setTopSideRightHalfSpawn(ball);
            }
        }

        private bool shouldSpawnFromPrimarySide()
        {
            return this.random.Next(0, 2) == 0;
        }

        private void setLeftSideBottomHalfSpawn(EnemyBall ball)
        {
            ball.XCord = -ball.Width;
            ball.YCord = this.random.Next((int)(this.canvasHeight / 2), (int)this.canvasHeight);
        }

        private void setBottomSideLeftHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.random.Next(0, (int)(this.canvasWidth / 2));
            ball.YCord = this.canvasHeight + ball.Height;
        }

        private void setRightSideBottomHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.canvasWidth + ball.Width;
            ball.YCord = this.random.Next((int)(this.canvasHeight / 2), (int)this.canvasHeight);
        }

        private void setBottomSideRightHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.random.Next((int)(this.canvasWidth / 2), (int)this.canvasWidth);
            ball.YCord = this.canvasHeight + ball.Height;
        }

        private void setLeftSideTopHalfSpawn(EnemyBall ball)
        {
            ball.XCord = -ball.Width;
            ball.YCord = this.random.Next(0, (int)(this.canvasHeight / 2));
        }

        private void setTopSideLeftHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.random.Next(0, (int)(this.canvasWidth / 2));
            ball.YCord = -ball.Height;
        }

        private void setRightSideTopHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.canvasWidth + ball.Width;
            ball.YCord = this.random.Next(0, (int)(this.canvasHeight / 2));
        }

        private void setTopSideRightHalfSpawn(EnemyBall ball)
        {
            ball.XCord = this.random.Next((int)(this.canvasWidth / 2), (int)this.canvasWidth);
            ball.YCord = -ball.Height;
        }

        #endregion
    }
}