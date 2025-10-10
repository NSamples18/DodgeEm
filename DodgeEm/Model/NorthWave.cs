using System;

namespace DodgeEm.Model
{
    /// <summary>
    /// Represents a wave where enemy balls move downward from above the canvas.
    /// </summary>
    public class NorthWave : Wave
    {
        #region Properties

        /// <summary>
        /// Gets the direction of travel for balls in this wave.
        /// Precondition: None.
        /// Postcondition: Returns Direction.Down.
        /// </summary>
        protected override Direction BallDirection => Direction.Down;

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the specified enemy ball is out of bounds for this wave.
        /// </summary>
        protected override bool IsOutOfBounds(EnemyBall ball, double width, double height)
        {
            return ball.Y > height;
        }

        /// <summary>
        /// Sets the initial position for the specified enemy ball at a random X coordinate just above the top edge of the canvas.
        /// Precondition: ball != null, width > 0, height > 0.
        /// Postcondition: Ball position is set above the canvas at a random X.
        /// </summary>
        /// <param name="ball">The enemy ball to position.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        protected override void SetInitialPositions(EnemyBall ball, double width, double height)
        {
            const int margin = 30;
            var maxX = Math.Max(margin, (int)width - margin);

            ball.X = Rand.Next(margin, maxX);
            ball.Y = -margin;
        }

        #endregion
    }
}