using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// </summary>
    /// <seealso cref="DodgeEm.Model.Core.GameObject" />
    public class GamePoint : GameObject
    {
        #region Data members

        private DispatcherTimer removalTimer;
        private readonly Random random = new Random();
        private readonly Canvas currentCanvas;
        private readonly LevelId levelId;

        #endregion

        #region Properties

        /// <summary>
        ///     True once the point has been collected/removed.
        /// </summary>
        public bool IsCollected { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePoint" /> class.
        /// </summary>
        public GamePoint(Canvas currentCanvas, double width, double height, LevelId levelId)
        {
            var gamePointSprite = new GamePointSprite
            {
                Fill = new SolidColorBrush(GameSettings.GamePointColor)
            };
            Sprite = gamePointSprite;
            this.levelId = levelId;
            this.randomRemoveGamePoint();
            this.randomSpawn(width, height);
            this.currentCanvas = currentCanvas;
            currentCanvas.Children.Add(Sprite);
        }

        #endregion

        #region Methods

        private void randomRemoveGamePoint()
        {
            var seconds = GameSettings.LevelOneDuration;
            if (this.levelId == LevelId.Level2)
            {
                seconds = this.random.Next(15, 20);
            }

            if (this.levelId == LevelId.Level3)
            {
                seconds = this.random.Next(10, 15);
            }

            this.removalTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
            this.removalTimer.Tick += this.removalTimer_Tick;
            this.removalTimer.Start();
        }

        private void removalTimer_Tick(object sender, object e)
        {
            this.Collect();
        }

        private void randomSpawn(double canvasWidth, double canvasHeight)
        {
            var maxX = canvasWidth - Width;
            var maxY = canvasHeight - Height;

            XCord = this.random.NextDouble() * maxX;
            YCord = this.random.NextDouble() * maxY;

            Sprite?.RenderAt(XCord, YCord);
        }

        /// <summary>
        ///     Removes the game point from the canvas.
        /// </summary>
        public void RemoveGamePoint()
        {
            this.currentCanvas.Children.Remove(Sprite);
        }

        /// <summary>
        ///     Remove this game point from the canvas and stop internal timers.
        ///     Safe to call multiple times.
        /// </summary>
        public void Collect()
        {
            if (this.IsCollected)
            {
                return;
            }

            this.IsCollected = true;

            this.removalTimer?.Stop();
            this.RemoveGamePoint();
        }

        #endregion
    }
}