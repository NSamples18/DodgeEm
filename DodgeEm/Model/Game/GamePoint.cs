using System;
using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DodgeEm.Model.Core.GameObject" />
    public class GamePoint : GameObject
    {
        private DispatcherTimer removalTimer;
        private Random random = new Random();
        private Canvas currentCanvas;
        private LevelId levelId;




        /// <summary>
        /// Initializes a new instance of the <see cref="GamePoint"/> class.
        /// </summary>
        public GamePoint(Canvas currentCanvas, double width, double height, LevelId levelId)
        {
            var gamePointSprite = new GamePointSprite()
            {
                Fill = new SolidColorBrush(GameSettings.GamePointColor)
            };
            Sprite = gamePointSprite;
            this.levelId = levelId;
            this.randomRemoveGamePoint();
            this.randomSpawn(width, height);
            this.currentCanvas = currentCanvas;
            currentCanvas.Children.Add(this.Sprite);
        }

        private void randomRemoveGamePoint()
        {

            var seconds = random.Next(15, 20);

            if (this.levelId == LevelId.Level3)
            {
                seconds = random.Next(10, 15);
            }

            removalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
            removalTimer.Tick += removalTimer_Tick;
            removalTimer.Start();
            
        }

        private void removalTimer_Tick(object sender, object e)
        {
            this.currentCanvas.Children.Remove(this.Sprite);
        }

        private void randomSpawn(double canvasWidth, double canvasHeight)
        {
            var maxX = canvasWidth - this.Width;
            var maxY = canvasHeight - this.Height;

            this.X = random.NextDouble() * maxX;
            this.Y = random.NextDouble() * maxY;

            Sprite?.RenderAt(this.X, this.Y);
        }
    }
}