using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    ///     Represents a level in the game.
    /// </summary>
    public class Level
    {
        #region Data members

        private readonly WaveManager waveManager;

        /// <summary>
        ///     Gets the level number.
        /// </summary>
        private readonly LevelId levelNumber;

        private readonly int elapsedMilliseconds;

        #endregion

        #region Properties

        public int stopLevel { get; }

        public int gamePoint { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level" /> class.
        /// </summary>
        /// IList
        /// <Color>
        ///     enemyColors, IList
        ///     <Object>
        ///         pointObjects,
        ///         <param name="levelNumber">The level number.</param>
        ///         <param name="startLevelTime">The start level.</param>
        ///         <param name="stopLevel">The stop level.</param>
        public Level(LevelId levelNumber, int stopLevel, int numOfGamePoint, Canvas gameCanvas)
        {
            this.waveManager = new WaveManager(gameCanvas, levelNumber);
            this.levelNumber = levelNumber;
            this.stopLevel = stopLevel;
            this.gamePoint = numOfGamePoint;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the enemy balls.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.waveManager.EnemyBalls;
        }

        /// <summary>
        ///     Gets the wave colors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Color> GetWaveColors()
        {
            return this.waveManager.GetCurrentLevelWaveColors(this.levelNumber);
        }

        /// <summary>
        ///     Gets the level identifier.
        /// </summary>
        /// <returns></returns>
        public LevelId GetLevelId()
        {
            return this.levelNumber;
        }

        /// <summary>
        ///     Nexts the level.
        /// </summary>
        public void NextLevel()
        {
            this.StopLevel();
            this.waveManager.RemoveBallsFromAllWaves();
        }

        /// <summary>
        ///     Stops the level.
        /// </summary>
        public void StopLevel()
        {
            this.waveManager.StopWave();
        }

        /// <summary>
        ///     Starts the internal timer for the wave.
        ///     Precondition: None.
        ///     Postcondition: Timer is running and ticks will occur.
        /// </summary>
        public void StartLevel()
        {
            this.waveManager.startWaveWithLevel();
        }

        /// <summary>
        ///     Resets the level.
        /// </summary>
        public void ResetLevel()
        {
            this.waveManager.RestartWavesInLevel();
        }

        #endregion
    }
}