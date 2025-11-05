namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Represents the possible movement directions for game objects.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        ///     Indicates upward movement.
        /// </summary>
        TopToBottom,

        /// <summary>
        ///     Indicates downward movement.
        /// </summary>
        BottomToTop,

        /// <summary>
        ///     Indicates movement to the left.
        /// </summary>
        RightToLeft,

        /// <summary>
        ///     Indicates movement to the right.
        /// </summary>
        LeftToRight,

        /// <summary>
        ///     Indicates movement to the northeast.
        /// </summary>
        BottomLeft,

        /// <summary>
        ///     Indicates movement to the northwest.
        /// </summary>
        BottomRight,

        /// <summary>
        ///     Indicates movement to the southeast.
        /// </summary>
        TopLeft,

        /// <summary>
        ///     Indicates movement to the southwest.
        /// </summary>
        TopRight,

        /// <summary>
        ///     Indicates mixed diagonal movement.
        /// </summary>
        DiagonalMixed,

        /// <summary>
        ///     Indicates mixed vertical movement.
        /// </summary>
        VerticalMixed
    }
}