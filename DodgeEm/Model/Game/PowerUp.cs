using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// Represents a power-up item in the game.
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
        /// Initializes a new instance of the <see cref="PowerUp"/> class.
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

            this.startSpawnTimer();
        }

        #endregion

        #region Methods        
        /// <summary>
        /// Restarts the power up.
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
        /// Removes the power up from the game.
        /// </summary>
        public void RemovePowerUp()
        {
            this.currentCanvas.Children.Remove(Sprite);
        }

        private void startSpawnTimer()
        {
            this.spawnTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            this.spawnTimer.Tick += this.spawnTimer_Tick;
            this.spawnTimer.Start();
        }

        private void startRemoveTimer()
        {
            var removeSeconds = this.random.Next(8, 12);
            this.removeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(removeSeconds) };
            this.removeTimer.Tick += this.removeTimer_Tick;
            this.removeTimer.Start();
        }

        private void spawnTimer_Tick(object sender, object e)
        {
            this.spawnTimer.Stop();

            X = this.random.NextDouble() * (this.canvasWidth - 30);
            Y = this.random.NextDouble() * (this.canvasHeight - 30);

            Sprite.RenderAt(X, Y);
            this.currentCanvas.Children.Add(Sprite);

            SetSpeed(3, 3);

            this.moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
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

            if (X <= 0)
            {
                X = 0;
                this.dirX = 1;
            }
            else if (X + Width >= this.canvasWidth)
            {
                X = this.canvasWidth - Width;
                this.dirX = -1;
            }

            if (Y <= 0)
            {
                Y = 0;
                this.dirY = 1;
            }
            else if (Y + Height >= this.canvasHeight)
            {
                Y = this.canvasHeight - Height;
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