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

        private readonly LevelId levelNumber;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the stop level timer.
        /// </summary>
        public int StopLevelTimer { get; }

        /// <summary>
        ///     Gets the game points.
        /// </summary>
        public int GamePoint { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level" /> class.
        /// </summary>
        /// IList
        /// <Color>
        ///     enemyColors, IList
        /// </Color>
        /// <Object>
        ///     pointObjects,
        /// </Object>
        /// <param name="levelNumber">The level number.</param>
        /// <param name="stopLevelTimer">The stop level timer.</param>
        /// <param name="numOfGamePoint">The number of game points.</param>
        /// <param name="gameCanvas">The game canvas.</param>
        public Level(LevelId levelNumber, int stopLevelTimer, int numOfGamePoint, Canvas gameCanvas)
        {
            this.waveManager = new WaveManager(gameCanvas, levelNumber);

            this.waveManager.WaveStarted += s => this.WaveStarted?.Invoke(this);

            this.levelNumber = levelNumber;
            this.StopLevelTimer = stopLevelTimer;
            this.GamePoint = numOfGamePoint;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets all enemy balls in the level.
        /// </summary>
        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.waveManager.EnemyBalls;
        }

        /// <summary>
        ///     Gets the wave colors for the current level.
        /// </summary>
        public IEnumerable<Color> GetWaveColors()
        {
            return this.waveManager.GetCurrentLevelWaveColors();
        }

        /// <summary>
        ///     Gets the level ID.
        /// </summary>
        public LevelId GetLevelId()
        {
            return this.levelNumber;
        }

        /// <summary>
        ///     Proceeds to the next level.
        /// </summary>
        public void NextLevel()
        {
            this.StopLevel();
            this.waveManager.RemoveBallsFromAllWavesInLevel();
        }

        /// <summary>
        ///     Stops the current level.
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
            this.waveManager.StartWaveWithLevel();
        }

        /// <summary>
        ///     Resets the current level.
        /// </summary>
        public void ResetLevel()
        {
            this.waveManager.RestartWavesInLevel();
        }

        /// <summary>
        ///     Removes all enemy balls from the level.
        /// </summary>
        public void RemoveAllBalls()
        {
            this.waveManager.RemoveAllBalls();
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void WaveStartedHandler(object sender);

        /// <summary>
        /// Occurs when [wave started].
        /// </summary>
        public event WaveStartedHandler WaveStarted;

        #endregion
    }
}