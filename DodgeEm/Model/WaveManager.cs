using System.Collections.Generic;
using Windows.UI;
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

        private EnemyWave northWave;
        private EnemyWave southWave;
        private EnemyWave eastWave;
        private EnemyWave westWave;

        public WaveManager(Canvas gameCanvas)
        {
            this.createWaves(gameCanvas);
        }

        private void createWaves(Canvas gameCanvas)
        {
             this.northWave = new EnemyWave(Colors.Red, Direction.TopToBottom, 0, gameCanvas, gameCanvas.Width, gameCanvas.Height);
             this.southWave = new EnemyWave(Colors.Red, Direction.BottomToTop, SouthWaveTenSecDelay, gameCanvas, gameCanvas.Width, gameCanvas.Height);
             this.eastWave = new EnemyWave(Colors.Orange, Direction.LeftToRight, EastWaveFifteenSecDelay, gameCanvas, gameCanvas.Width, gameCanvas.Height);
             this.westWave = new EnemyWave(Colors.Orange, Direction.RightToLeft, WestWaveFiveSecDelay, gameCanvas, gameCanvas.Width, gameCanvas.Height);
        }

        #endregion


        #region Methods
        /// <summary>
        /// Gets all enemy balls currently active in all waves.
        /// Precondition: None.
        /// Postcondition: Returns a list of all active enemy balls.
        /// </summary>
        /// <returns>A list of all active enemy balls.</returns>
            public IEnumerable<EnemyBall> EnemyBalls
        {
            get
            {
                var allEnemies = new List<EnemyBall>();
                allEnemies.AddRange(this.northWave.EnemyBalls);
                allEnemies.AddRange(this.westWave.EnemyBalls);
                allEnemies.AddRange(this.southWave.EnemyBalls);
                allEnemies.AddRange(this.eastWave.EnemyBalls);
                return allEnemies;
            }
        }

        /// <summary>
        /// Stops all enemy waves.
        /// Precondition: None.
        /// Postcondition: All waves are stopped and no new enemies will spawn.
        /// </summary>
        public void StopAllWaves()
        {
            this.northWave.StopTimer();
            this.southWave.StopTimer();
            this.eastWave.StopTimer();
            this.westWave.StopTimer();
        }

        #endregion
    }
}