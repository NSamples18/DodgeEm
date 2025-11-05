using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// </summary>
    public class GamePointManager
    {
        #region Data members

        private readonly List<GamePoint> gamePoints;
        private readonly double canvasWidth;
        private readonly double canvasHeight;
        private readonly Canvas currentCanvas;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePointManager" /> class.
        /// </summary>
        /// <param name="currentCanvas">The current canvas.</param>
        /// <param name="canvasWidth">Width of the canvas.</param>
        /// <param name="canvasHeight">Height of the canvas.</param>
        public GamePointManager(Canvas currentCanvas, double canvasWidth, double canvasHeight)
        {
            this.gamePoints = new List<GamePoint>();
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            this.currentCanvas = currentCanvas;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the game points.
        /// </summary>
        /// <param name="numOfPoints">The number of points.</param>
        /// <param name="levelId">The level identifier.</param>
        public void AddGamePoints(int numOfPoints, LevelId levelId)
        {
            for (var i = 0; i < numOfPoints; i++)
            {
                this.gamePoints.Add(new GamePoint(this.currentCanvas, this.canvasWidth, this.canvasHeight, levelId));
            }
        }

        /// <summary>
        ///     Gets the game points.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GamePoint> GetGamePoints()
        {
            return this.gamePoints;
        }

        /// <summary>
        ///     Removes the game point.
        /// </summary>
        /// <param name="point">The point.</param>
        public void RemoveGamePoint(GamePoint point)
        {
            if (point == null)
            {
                return;
            }

            if (this.gamePoints.Remove(point))
            {
                point.Collect();
            }
        }

        /// <summary>
        ///     Removes all game points from the game.
        /// </summary>
        public void RemoveAllGamePoints()
        {
            foreach (var point in this.gamePoints.ToList())
            {
                point.RemoveGamePoint();
            }

            this.gamePoints.Clear();
        }

        /// <summary>
        ///     Cleanups the collected.
        /// </summary>
        public void CleanupCollected()
        {
            var removed = this.gamePoints.Where(p => p.IsCollected).ToList();
            foreach (var p in removed)
            {
                this.gamePoints.Remove(p);
            }
        }

        #endregion
    }
}