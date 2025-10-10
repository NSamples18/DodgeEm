using System;

namespace DodgeEm.Model
{
    /// <summary>
    /// Represents a wave where enemy balls move upward from below the canvas.
    /// </summary>
    public class SouthWave : Wave
    {
        #region Properties

        /// <summary>
        /// Gets the direction of travel for balls in this wave.
        /// Precondition: None.
        /// Postcondition: Returns Direction.Up.
        /// </summary>
        protected override Direction BallDirection => Direction.Up;

        #endregion

        #region Methods

        // Out of bounds if above the canvas
        /// <inheritdoc/>
        protected override bool IsOutOfBounds(EnemyBall ball, double width, double height)
        {
            return ball.Y < 0;
        }

        /// <summary>
        /// Sets the initial position for the specified enemy ball at a random X coordinate just below the bottom edge of the canvas.
        /// Precondition: ball != null, width > 0, height > 0.
        /// Postcondition: Ball position is set below the canvas at a random X.
        /// </summary>
        /// <param name="ball">The enemy ball to position.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        protected override void SetInitialPositions(EnemyBall ball, double width, double height)
        {
            const int margin = 30;
            var maxX = Math.Max(margin, (int)width - margin);

            ball.X = Rand.Next(margin, maxX);
            ball.Y = height + margin;
        }

        #endregion
    }
}