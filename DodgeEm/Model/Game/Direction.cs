namespace DodgeEm.Model.Game
{
    /// <summary>
    /// Represents the possible movement directions for game objects.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Indicates upward movement.
        /// </summary>
        TopToBottom,
        /// <summary>
        /// Indicates downward movement.
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Indicates movement to the left.
        /// </summary>
        LeftToRight,
        /// <summary>
        /// Indicates movement to the right.
        /// </summary>
        RightToLeft,
        /// <summary>
        /// Indicates all directions.
        /// </summary>
        All
    }
}