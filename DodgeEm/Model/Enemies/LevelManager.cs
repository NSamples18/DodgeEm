using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    ///     Manages the levels in the game.
    /// </summary>
    public class LevelManager
    {
        #region Data members

        private readonly List<Level> levels;
        private int currentLevelIndex;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelManager" /> class.
        /// </summary>
        public LevelManager(Canvas gameCanvas)
        {
            this.levels = new List<Level>();
            this.addLevels(gameCanvas);
            this.StartLevel();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the enemy balls in the current level.
        /// </summary>
        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.levels[this.currentLevelIndex].GetEnemyBalls();
        }

        /// <summary>
        ///     Gets the wave colors in the current level.
        /// </summary>
        public IEnumerable<Color> GetCurrentLevelWaveColors()
        {
            return this.levels[this.currentLevelIndex].GetWaveColors();
        }

        /// <summary>
        ///     Gets the level ID of the current level.
        /// </summary>
        public LevelId GetLevelId()
        {
            return this.levels[this.currentLevelIndex].GetLevelId();
        }

        /// <summary>
        ///     Gets the duration of the current level.
        /// </summary>
        public int GetLevelDuration()
        {
            if (this.currentLevelIndex < this.levels.Count)
            {
                return this.levels[this.currentLevelIndex].StopLevelTimer;
            }

            return 0;
        }

        /// <summary>
        ///     Gets the game points of the current level.
        /// </summary>
        public int GetCurrentLevelGamePoints()
        {
            return this.levels[this.currentLevelIndex].GamePoint;
        }

        /// <summary>
        ///     Advances to the next level.
        /// </summary>
        public void NextLevel()
        {
            this.levels[this.currentLevelIndex].NextLevel();
            this.currentLevelIndex++;
            this.levels[this.currentLevelIndex].StartLevel();
        }

        /// <summary>
        ///     Stops the current level.
        /// </summary>
        public void StopLevel()
        {
            this.levels[this.currentLevelIndex].StopLevel();
        }

        /// <summary>
        ///     Starts the current level.
        /// </summary>
        public void StartLevel()
        {
            this.levels[this.currentLevelIndex].StartLevel();
        }

        /// <summary>
        ///     Restarts the current level.
        /// </summary>
        public void RestartCurrentLevel()
        {
            this.levels[this.currentLevelIndex].ResetLevel();
        }

        /// <summary>
        ///     Removes all enemy balls from the current level.
        /// </summary>
        public void RemoveAllBalls()
        {
            this.levels[this.currentLevelIndex].RemoveAllBalls();
        }

        private void addLevels(Canvas gameCanvas)
        {
            var lvl1 = new Level(LevelId.Level1, 25, 1, gameCanvas);
            lvl1.WaveStarted += s => this.WaveStarted?.Invoke(this);
            this.levels.Add(lvl1);

            var lvl2 = new Level(LevelId.Level2, 30, 2, gameCanvas);
            lvl2.WaveStarted += s => this.WaveStarted?.Invoke(this);
            this.levels.Add(lvl2);

            var lvl3 = new Level(LevelId.Level3, 35, 3, gameCanvas);
            lvl3.WaveStarted += s => this.WaveStarted?.Invoke(this);
            this.levels.Add(lvl3);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Raised whenever a wave inside the current level starts.
        /// </summary>
        public delegate void WaveStartedHandler(object sender);

        /// <summary>
        ///     Occurs when [wave started].
        /// </summary>
        public event WaveStartedHandler WaveStarted;

        #endregion
    }
}