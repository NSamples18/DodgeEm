using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
{
    /// <summary>
    /// Abstract base class for enemy ball waves. Handles spawning, movement, and removal of balls.
    /// </summary>
    public abstract class Wave
    {
        #region Fields      
        /// <summary>
        /// The minimum speed for enemy balls in a wave.
        /// </summary>
        protected const int MinSpeed = 1;
        /// <summary>
        /// The Max speed for enemy balls in a wave.
        /// </summary>
        protected const int MaxSpeed = 5;
        /// <summary>
        /// The minimum number of ticks until the next ball is generated.
        /// </summary>
        protected const int MinTicksUntilNextBall = 6;

        /// <summary>
        /// The maximum number of ticks until the next ball is generated (exclusive).
        /// </summary>
        protected const int MaxTicksUntilNextBall = 10;


        /// <summary>
        /// The collection of enemy balls currently managed by this wave.
        /// </summary>
        protected readonly IList<EnemyBall> EnemyBalls = new List<EnemyBall>();

        /// <summary>
        /// The random number generator used for ball placement and timing.
        /// </summary>
        protected readonly Random Rand = new Random();

        /// <summary>
        /// The canvas on which enemy balls are drawn and updated.
        /// </summary>
        protected Canvas CurrentCanvas;

        /// <summary>
        /// The width of the current game canvas.
        /// </summary>
        protected double CanvasWidth;

        /// <summary>
        /// The height of the current game canvas.
        /// </summary>
        protected double CanvasHeight;

        private DispatcherTimer timer;
        private int tickCount;
        private int ticksUntilNextBall = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interval between timer ticks for this wave.
        /// Precondition: None.
        /// Postcondition: Timer interval is set.
        /// </summary>
        public TimeSpan TickInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Gets the direction of travel for balls in this wave.
        /// </summary>
        protected abstract Direction BallDirection { get; }

        /// <summary>
        /// Gets the speed range (inclusive lower, exclusive upper) used on ball creation.
        /// </summary>
        protected virtual (int min, int max) SpeedRange => (MinSpeed, MaxSpeed);

        /// <summary>
        /// Gets the color of balls in this wave.
        /// </summary>
        protected virtual Color BallColor => Colors.Red;

        #endregion

        #region Methods

        /// <summary>
        /// Starts the wave, places balls, and begins ticking.
        /// Precondition: gameCanvas != null, width > 0, height > 0.
        /// Postcondition: Wave is started and enemy balls begin spawning.
        /// </summary>
        /// <param name="gameCanvas">The canvas to use for the wave.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        /// <param name="initialDelayMs">Optional initial delay in milliseconds.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task StartAsync(Canvas gameCanvas, double width, double height, int initialDelayMs = 0)
        {
            this.CurrentCanvas = gameCanvas;
            this.CanvasWidth = width;
            this.CanvasHeight = height;

            if (initialDelayMs > 0)
            {
                await Task.Delay(initialDelayMs);
            }

            this.StartTimer();
        }

        /// <summary>
        /// Starts the internal timer for the wave.
        /// Precondition: None.
        /// Postcondition: Timer is running and ticks will occur.
        /// </summary>
        public void StartTimer()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Tick -= this.Timer_Tick;
            }

            this.timer = new DispatcherTimer { Interval = this.TickInterval };
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
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Tick -= this.Timer_Tick;
            }
        }

        /// <summary>
        /// Gets the enemy balls currently managed by this wave.
        /// Precondition: None.
        /// Postcondition: Returns a collection of enemy balls.
        /// </summary>
        /// <returns>An IEnumerable of EnemyBall.</returns>
        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.EnemyBalls;
        }

        private void Timer_Tick(object sender, object e)
        {
            this.OnTick();
        }

        /// <summary>
        /// Called on each timer tick to update the wave state.
        /// Precondition: None.
        /// Postcondition: Enemy balls are added, moved, and out-of-bounds balls are removed.
        /// </summary>
        protected void OnTick()
        {
            this.addRandomBallsToCanvas();
            this.MoveEnemyBalls();

            if (this.CurrentCanvas != null)
            {
                this.RemoveOutOfBoundsBalls();
            }
        }

        /// <summary>
        /// Moves all enemy balls according to their direction and speed.
        /// Precondition: enemyBalls contains valid EnemyBall instances.
        /// Postcondition: All enemy balls are moved.
        /// </summary>
        protected void MoveEnemyBalls()
        {
            foreach (var ball in this.EnemyBalls)
            {
                ball.Move();
            }
        }

        /// <summary>
        /// Generates a new enemy ball and adds it to the canvas.
        /// Precondition: currentCanvas is not null; canvasWidth and canvasHeight are valid.
        /// Postcondition: A new enemy ball is created, positioned, and added to the canvas.
        /// </summary>
        protected void GenerateEnemyBall()
        {
            var (minSpeed, maxSpeed) = this.SpeedRange;

            var speed = this.Rand.Next(minSpeed, maxSpeed);
            var ball = new EnemyBall(this.BallColor, this.BallDirection, speed);
            this.EnemyBalls.Add(ball);
            this.SetInitialPositions(ball, this.CanvasWidth, this.CanvasHeight);
            this.CurrentCanvas.Children.Add(ball.Sprite);
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

        private void getRandomTick(Random random)
        {
            this.ticksUntilNextBall = random.Next(MinTicksUntilNextBall, MaxTicksUntilNextBall);
        }

        /// <summary>
        /// Removes enemy balls that are out of bounds from the canvas and the list.
        /// Precondition: enemyBalls contains valid EnemyBall instances; currentCanvas is not null.
        /// Postcondition: Out-of-bounds balls are removed from both the canvas and the list.
        /// </summary>
        protected void RemoveOutOfBoundsBalls()
        {
            for (var i = this.EnemyBalls.Count - 1; i >= 0; i--)
            {
                var enemyBall = this.EnemyBalls[i];
                if (this.IsOutOfBounds(enemyBall, this.CanvasWidth, this.CanvasHeight))
                {
                    this.CurrentCanvas.Children.Remove(enemyBall.Sprite);
                    this.EnemyBalls.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Sets initial X/Y for each ball given canvas bounds.
        /// Precondition: ball != null, width > 0, height > 0.
        /// Postcondition: Ball position is set.
        /// </summary>
        /// <param name="ball">The enemy ball to position.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        protected abstract void SetInitialPositions(EnemyBall ball, double width, double height);

        /// <summary>
        /// Returns true when the ball is off-screen for this wave’s direction.
        /// Precondition: ball != null, width > 0, height > 0.
        /// Postcondition: Returns true if ball is out of bounds, otherwise false.
        /// </summary>
        /// <param name="ball">The enemy ball to check.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        /// <returns>True if the ball is out of bounds, otherwise false.</returns>
        protected abstract bool IsOutOfBounds(EnemyBall ball, double width, double height);

        #endregion
    }
}