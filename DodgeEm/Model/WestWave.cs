using System;
using Windows.UI;

namespace DodgeEm.Model
{
    /// <summary>
    /// Represents a wave where enemy balls move right (west to east) and are colored yellow.
    /// </summary>
    public class WestWave : Wave
    {
        #region Properties

        /// <summary>
        /// Gets the direction of travel for balls in this wave.
        /// Precondition: None.
        /// Postcondition: Returns Direction.Right.
        /// </summary>
        protected override Direction BallDirection => Direction.Right;

        /// <summary>
        /// Gets the color of balls in this wave.
        /// Precondition: None.
        /// Postcondition: Returns Colors.Yellow.
        /// </summary>
        protected override Color BallColor => Colors.Yellow;

        #endregion

        #region Methods

        // Balls are out of bounds if they move past the right edge of the canvas
        /// <inheritdoc/>
        protected override bool IsOutOfBounds(EnemyBall ball, double width, double height)
        {
            return ball.X > width;
        }

        /// <summary>
        /// Sets the initial position for the specified enemy ball at the left edge of the canvas and a random Y coordinate.
        /// Precondition: ball != null, width > 0, height > 0.
        /// Postcondition: Ball position is set at the left edge and random Y.
        /// </summary>
        /// <param name="ball">The enemy ball to position.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        protected override void SetInitialPositions(EnemyBall ball, double width, double height)
        {
            const int margin = 30;
            var maxY = Math.Max(margin, (int)height - margin);

            ball.X = -margin;
            ball.Y = Rand.Next(margin, maxY);
        }

        #endregion
    }
}