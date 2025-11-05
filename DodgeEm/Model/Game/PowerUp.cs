using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Represents a power-up item in the game.
    /// </summary>
    public class PowerUp : GameObject
    {
        #region Data members

        private readonly Random random = new Random();
        private DispatcherTimer spawnTimer;
        private DispatcherTimer moveTimer;
        private DispatcherTimer removeTimer;

        private readonly Canvas currentCanvas;
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        private int dirX = 1;
        private int dirY = 1;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerUp" /> class.
        /// </summary>
        public PowerUp(Canvas canvas, double width, double height)
        {
            this.currentCanvas = canvas;
            this.canvasWidth = width;
            this.canvasHeight = height;

            var powerUpSprite = new PowerUpSprite
            {
                Fill = new SolidColorBrush(GameSettings.GamePointColor)
            };
            Sprite = powerUpSprite;

            this.canvasWidth = width;
            this.canvasHeight = height;
            SetSpeed(GameSettings.PlayerSpeedXDirection, GameSettings.PlayerSpeedYDirection);
            this.startSpawnTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Restarts the power up.
        /// </summary>
        public void RestartPowerUp()
        {
            this.spawnTimer?.Stop();

            this.moveTimer?.Stop();
            this.removeTimer?.Stop();

            if (this.currentCanvas.Children.Contains(Sprite))
            {
                this.currentCanvas.Children.Remove(Sprite);
            }

            this.startSpawnTimer();
            this.startRemoveTimer();
        }

        /// <summary>
        ///     Removes the power up from the game.
        /// </summary>
        public void RemovePowerUp()
        {
            this.currentCanvas.Children.Remove(Sprite);
        }

        private void startSpawnTimer()
        {
            this.spawnTimer = new DispatcherTimer
                { Interval = TimeSpan.FromSeconds(GameSettings.TimeUntilPowerUpSpawns) };
            this.spawnTimer.Tick += this.spawnTimer_Tick;
            this.spawnTimer.Start();
        }

        private void startRemoveTimer()
        {
            var removeSeconds = this.random.Next(GameSettings.MinSecondsUntilPowerUpRemoved,
                GameSettings.MaxSecondsUntilPowerUpRemoved);
            this.removeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(removeSeconds) };
            this.removeTimer.Tick += this.removeTimer_Tick;
            this.removeTimer.Start();
        }

        private void spawnTimer_Tick(object sender, object e)
        {
            this.spawnTimer.Stop();

            XCord = this.random.NextDouble() * (this.canvasWidth - Width);
            YCord = this.random.NextDouble() * (this.canvasHeight - Height);

            Sprite.RenderAt(XCord, YCord);
            this.currentCanvas.Children.Add(Sprite);

            this.moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs) };
            this.moveTimer.Tick += this.moveTimer_Tick;
            this.moveTimer.Start();
            this.startRemoveTimer();
        }

        private void moveTimer_Tick(object sender, object e)
        {
            if (this.dirX > 0)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }

            if (this.dirY > 0)
            {
                MoveDown();
            }
            else
            {
                MoveUp();
            }

            if (XCord <= 0)
            {
                XCord = 0;
                this.dirX = 1;
            }
            else if (XCord + Width >= this.canvasWidth)
            {
                XCord = this.canvasWidth - Width;
                this.dirX = -1;
            }

            if (YCord <= 0)
            {
                YCord = 0;
                this.dirY = 1;
            }
            else if (YCord + Height >= this.canvasHeight)
            {
                YCord = this.canvasHeight - Height;
                this.dirY = -1;
            }
        }

        private void removeTimer_Tick(object sender, object e)
        {
            this.removeTimer.Stop();
            this.moveTimer?.Stop();
            this.currentCanvas.Children.Remove(Sprite);
        }

        #endregion
    }
}