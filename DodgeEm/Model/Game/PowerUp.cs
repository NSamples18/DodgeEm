using DodgeEm.Model.Game;

using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

namespace DodgeEm.Model.Game
{
    public class PowerUp : GameObject
    {
        private readonly Random random = new Random();
        private DispatcherTimer spawnTimer;
        private DispatcherTimer moveTimer;
        private DispatcherTimer removeTimer;

        private Canvas currentCanvas;
        private double canvasWidth;
        private double canvasHeight;

        private int dirX = 1;
        private int dirY = 1;

        public PowerUp(Canvas canvas, double width, double height)
        {
            this.currentCanvas = canvas;
            this.canvasWidth = width;
            this.canvasHeight = height;

            var powerUpSprite = new PowerUpSprite()
            {
                Fill = new SolidColorBrush(GameSettings.GamePointColor)
            };
            this.Sprite = powerUpSprite;

            this.canvasWidth = width;
            this.canvasHeight = height;

            StartSpawnTimer();
        }

        public void RestartPowerUp()
        {

            spawnTimer?.Stop();

            moveTimer?.Stop();
            removeTimer?.Stop();

            if (currentCanvas.Children.Contains(this.Sprite))
                currentCanvas.Children.Remove(this.Sprite);

            StartSpawnTimer();
            StartRemoveTimer();
        }

        public void RemovePowerUp()
        {
            this.currentCanvas.Children.Remove(this.Sprite);
        }

        private void StartSpawnTimer()
        {
            spawnTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            spawnTimer.Tick += SpawnTimer_Tick;
            spawnTimer.Start();
        }

        private void StartRemoveTimer()
        {
            var removeSeconds = random.Next(8, 12);
            removeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(removeSeconds) };
            removeTimer.Tick += RemoveTimer_Tick;
            removeTimer.Start();
        }

        private void SpawnTimer_Tick(object sender, object e)
        {
            spawnTimer.Stop();

            this.X = random.NextDouble() * (canvasWidth - 30);
            this.Y = random.NextDouble() * (canvasHeight - 30);

            this.Sprite.RenderAt(this.X, this.Y);
            currentCanvas.Children.Add(this.Sprite);

            this.SetSpeed(3, 3);

            moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
            this.StartRemoveTimer();
        }

        private void MoveTimer_Tick(object sender, object e)
        {
            if (dirX > 0)
                this.MoveRight();
            else
                this.MoveLeft();

            if (dirY > 0)
                this.MoveDown();
            else
                this.MoveUp();

            if (this.X <= 0)
            {
                this.X = 0;
                dirX = 1;
            }
            else if (this.X + this.Width >= canvasWidth)
            {
                this.X = canvasWidth - this.Width;
                dirX = -1;
            }

            if (this.Y <= 0)
            {
                this.Y = 0;
                dirY = 1;
            }
            else if (this.Y + this.Height >= canvasHeight)
            {
                this.Y = canvasHeight - this.Height;
                dirY = -1;
            }
        }

        private void RemoveTimer_Tick(object sender, object e)
        {
            removeTimer.Stop();
            moveTimer?.Stop();
            currentCanvas.Children.Remove(this.Sprite);
        }
    }
}
