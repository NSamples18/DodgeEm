using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
{
    /// <summary>
    /// Manages all enemy waves in the game.
    /// </summary>
    public class WaveManager
    {
        #region Data members

        private const int WestWaveFiveSecDelay = 5000;
        private const int SouthWaveTenSecDelay = 10000;
        private const int EastWaveFifteenSecDelay = 15000;

        private readonly NorthWave northWave;
        private readonly WestWave westWave;
        private readonly SouthWave southWave;
        private readonly EastWave eastWave;

        #endregion

        #region Constructors
        /// <summary>
        /// manages all four enemy waves.
        /// </summary>
        public WaveManager()
        {
            this.northWave = new NorthWave();
            this.westWave = new WestWave();
            this.southWave = new SouthWave();
            this.eastWave = new EastWave();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes all enemy waves on the provided game canvas.
        /// </summary>
        /// <param name="gameCanvas">takes in the games canvas</param>
        public void InitializeWave(Canvas gameCanvas)
        {
            _ = this.northWave.StartAsync(gameCanvas, gameCanvas.Width, gameCanvas.Height);
            _ = this.westWave.StartAsync(gameCanvas, gameCanvas.Width, gameCanvas.Height, WestWaveFiveSecDelay);
            _ = this.southWave.StartAsync(gameCanvas, gameCanvas.Width, gameCanvas.Height, SouthWaveTenSecDelay);
            _ = this.eastWave.StartAsync(gameCanvas, gameCanvas.Width, gameCanvas.Height, EastWaveFifteenSecDelay);
        }

        /// <summary>
        /// Gets all enemy balls currently active in all waves.
        /// Precondition: None.
        /// Postcondition: Returns a list of all active enemy balls.
        /// </summary>
        /// <returns>A list of all active enemy balls.</returns>
        public IList<EnemyBall> GetCurrentWaveEnemies()
        {
            var allEnemies = new List<EnemyBall>();
            allEnemies.AddRange(this.northWave.GetEnemyBalls());
            allEnemies.AddRange(this.westWave.GetEnemyBalls());
            allEnemies.AddRange(this.southWave.GetEnemyBalls());
            allEnemies.AddRange(this.eastWave.GetEnemyBalls());
            return allEnemies;
        }

        /// <summary>
        /// Stops all enemy waves.
        /// Precondition: None.
        /// Postcondition: All waves are stopped and no new enemies will spawn.
        /// </summary>
        public void StopAllWaves()
        {
            this.northWave.StopTimer();
            this.westWave.StopTimer();
            this.southWave.StopTimer();
            this.eastWave.StopTimer();
        }

        #endregion
    }
}