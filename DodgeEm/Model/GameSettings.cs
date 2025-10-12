using System.Drawing;

namespace DodgeEm.Model
{
    public static class GameSettings
    {
       
        /// <summary>
        /// The minimum speed for enemy balls in a wave.
        /// </summary>
        public const int MinSpeed = 1;
        /// <summary>
        /// The Max speed for enemy balls in a wave.
        /// </summary>
        public const int MaxSpeed = 5;
        /// <summary>
        /// The minimum number of ticks until the next ball is generated.
        /// </summary>
        public const int MinTicksUntilNextBall = 6;

        /// <summary>
        /// The maximum number of ticks until the next ball is generated (exclusive).
        /// </summary>
        public const int MaxTicksUntilNextBall = 10;

        /// <summary>
        /// The color for North and South enemy balls.
        /// </summary>
        public static readonly Color NorthandSouthColor = Color.Red;
        /// <summary>
        /// The color for East and West enemy balls.
        /// </summary>
        public static readonly Color EastandWestColor = Color.Orange;
    }
}