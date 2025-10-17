
using Windows.UI;

namespace DodgeEm.Model
{
    /// <summary>
    /// Game settings for the DodgeEm game.
    /// </summary>
    public static class GameSettings
    {
       
        /// <summary>
        /// The minimum speed for enemy balls in a wave.
        /// </summary>
        public static readonly int MinSpeed = 2;
        /// <summary>
        /// The Max speed for enemy balls in a wave.
        /// </summary>
        public static readonly int MaxSpeed = 5;

        public static readonly int BlitzSpeed = 10;
        /// <summary>
        /// The speed of the player ball in the X direction.
        /// </summary>
        public static readonly int PlayerSpeedXDirection = 3;
        /// <summary>
        /// The speed of the player ball in the Y direction.
        /// </summary>
        public static readonly int PlayerSpeedYDirection = 3;
        /// <summary>
        /// The margin around the player ball.
        /// </summary>
        public static readonly int PlayerBallMargin = 30;
        /// <summary>
        /// The minimum number of ticks until the next ball is generated.
        /// </summary>
        public static readonly int MinTicksUntilNextBall = 6;

        /// <summary>
        /// The maximum number of ticks until the next ball is generated (exclusive).
        /// </summary>
        public static readonly int MaxTicksUntilNextBall = 10;
        /// <summary>
        /// The number of ticks until the game ends.
        /// </summary>
        public static readonly int GameEnds = 25;
        /// <summary>
        /// The duration of the game in seconds.
        /// </summary>
        public static readonly int TickIntervalMs = 20;
        /// <summary>
        /// The delay for the West wave (5 seconds).
        /// </summary>
        public static readonly int WestWaveFiveSecDelay = 5000;
        /// <summary>
        /// The delay for the South wave (10 seconds).
        /// </summary>
        public static readonly int SouthWaveTenSecDelay = 10000;
        /// <summary>
        /// The delay for the East wave (15 seconds).
        /// </summary>
        public static readonly int EastWaveFifteenSecDelay = 15000;
        /// <summary>
        /// The delay for the Final Blitz wave (20 seconds).
        /// </summary>
        public static readonly int FinalBlitzDelay = 20000;

        /// <summary>
        /// The color for North and South enemy balls.
        /// </summary>
        public static readonly Color NorthAndSouthColor = Colors.Red;
        /// <summary>
        /// The color for East and West enemy balls.
        /// </summary>
        public static readonly Color EastAndWestColor = Colors.Orange;
        /// <summary>
        /// The final blitz color
        /// </summary>
        public static readonly Color FinalBlitzColor = Colors.Purple;
    }
}