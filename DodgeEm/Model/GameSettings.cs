
using Windows.UI;

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

        public const int PlayerSpeedXDirection = 3;

        public const int PlayerSpeedYDirection = 3;

        public const int PlayerBallMargin = 30;
        /// <summary>
        /// The minimum number of ticks until the next ball is generated.
        /// </summary>
        public const int MinTicksUntilNextBall = 6;

        /// <summary>
        /// The maximum number of ticks until the next ball is generated (exclusive).
        /// </summary>
        public const int MaxTicksUntilNextBall = 10;

        public const int GameEnds = 25;

        public const int TickIntervalMs = 20;

        public const int WestWaveFiveSecDelay = 5000;

        public const int SouthWaveTenSecDelay = 10000;

        public const int EastWaveFifteenSecDelay = 15000;

        /// <summary>
        /// The color for North and South enemy balls.
        /// </summary>
        public static readonly Color NorthandSouthColor = Colors.Red;
        /// <summary>
        /// The color for East and West enemy balls.
        /// </summary>
        public static readonly Color EastandWestColor = Colors.Orange;
    }
}