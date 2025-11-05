using Windows.UI;

namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Game settings for the DodgeEm game.
    /// </summary>
    public static class GameSettings
    {
        #region Data members

        /// <summary>
        ///     The minimum speed for enemy balls in a wave.
        /// </summary>
        public static readonly int MinSpeed = 1;

        /// <summary>
        ///     The Max speed for enemy balls in a wave.
        /// </summary>
        public static readonly int MaxSpeed = 4;

        /// <summary>
        ///     The speed of enemy balls during the blitz phase.
        /// </summary>
        public static readonly int BlitzSpeed = 8;

        /// <summary>
        ///     The speed of the player ball in the XCord Direction.
        /// </summary>
        public static readonly int PlayerSpeedXDirection = 3;

        /// <summary>
        ///     The speed of the player ball in the YCord Direction.
        /// </summary>
        public static readonly int PlayerSpeedYDirection = 3;

        /// <summary>
        ///     The number of lives the player has.
        /// </summary>
        public static readonly int PlayerLives = 3;

        /// <summary>
        ///     The minimum number of ticks until the next ball is generated.
        /// </summary>
        public static readonly int MinTicksUntilNextBall = 20;

        /// <summary>
        ///     The maximum number of ticks until the next ball is generated (exclusive).
        /// </summary>
        public static readonly int MaxTicksUntilNextBall = 40;

        /// <summary>
        ///     The minimum number of seconds until a power-up is removed.
        /// </summary>
        public static readonly int MinSecondsUntilPowerUpRemoved = 8;

        /// <summary>
        ///     The maximum number of seconds until a power-up is removed.
        /// </summary>
        public static readonly int MaxSecondsUntilPowerUpRemoved = 12;

        /// <summary>
        ///     The minimum number of seconds until a point object is removed.
        /// </summary>
        public static readonly int MinSecondsUntilPointObjectRemoved = 15;

        /// <summary>
        ///     The maximum number of seconds until a point object is removed.
        /// </summary>
        public static readonly int MaxSecondsUntilPointObjectRemoved = 20;

        /// <summary>
        ///     The time until a power-up spawns.
        /// </summary>
        public static readonly int TimeUntilPowerUpSpawns = 5;

        /// <summary>
        ///     The delay between enemy waves.
        /// </summary>
        public static readonly int DelayBetweenWave = 5000;

        /// <summary>
        ///     The duration of the game in seconds.
        /// </summary>
        public static readonly int TickIntervalMs = 20;

        /// <summary>
        ///     The interval for the player's death animation.
        /// </summary>
        public static readonly int DeathAnimationIntervalMs = 250;

        /// <summary>
        ///     The level one duration
        /// </summary>
        public static readonly int LevelOneDuration = 25;

        /// <summary>
        ///     The level two duration
        /// </summary>
        public static readonly int LevelTwoDuration = 30;

        /// <summary>
        ///     The level three duration
        /// </summary>
        public static readonly int LevelThreeDuration = 35;

        /// <summary>
        ///     The color for North and South enemy balls.
        /// </summary>
        public static readonly Color Level1NorthAndSouthColor = Colors.Red;

        /// <summary>
        ///     The color for East and West enemy balls.
        /// </summary>
        public static readonly Color Level1EastAndWestColor = Colors.Orange;

        /// <summary>
        ///     The color for North and South enemy balls.
        /// </summary>
        public static readonly Color Level2North = Colors.White;

        /// <summary>
        ///     The color for East and West enemy balls.
        /// </summary>
        public static readonly Color Level2East = Colors.Brown;

        /// <summary>
        ///     The color for South enemy balls.
        /// </summary>
        public static readonly Color Level2South = Colors.LightGreen;

        /// <summary>
        ///     The color for West enemy balls.
        /// </summary>
        public static readonly Color Level2West = Colors.LightBlue;

        /// <summary>
        ///     The color for North enemy balls.
        /// </summary>
        public static readonly Color Level3North = Colors.Green;

        /// <summary>
        ///     The color for East enemy balls.
        /// </summary>
        public static readonly Color Level3East = Colors.Gray;

        /// <summary>
        ///     The color for South enemy balls.
        /// </summary>
        public static readonly Color Level3South = Colors.Tan;

        /// <summary>
        ///     The color for West enemy balls.
        /// </summary>
        public static readonly Color Level3West = Colors.Teal;

        /// <summary>
        ///     The final blitz color
        /// </summary>
        public static readonly Color FinalBlitzColor = Colors.Purple;

        /// <summary>
        ///     The game point color
        /// </summary>
        public static readonly Color GamePointColor = Colors.Yellow;

        #endregion
    }
}