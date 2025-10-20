using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    ///     Manages all enemy waves within the game.
    /// </summary>
    public class WaveManager
    {
        #region Data members

        private readonly List<EnemyWave> waves;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets all enemy balls currently active in all waves.
        ///     Precondition: None.
        ///     Postcondition: Returns a list of all active enemy balls.
        /// </summary>
        public IEnumerable<EnemyBall> EnemyBalls => this.waves.SelectMany(w => w.EnemyBalls);

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveManager" /> class.
        /// </summary>
        /// <param name="gameCanvas">The canvas on which the game is rendered.</param>
        public WaveManager(Canvas gameCanvas)
        {
            this.waves = new List<EnemyWave>();
            this.createWaves(gameCanvas);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Stops all enemy waves.
        ///     Precondition: None.
        ///     Postcondition: All waves are stopped and no new enemies will spawn.
        /// </summary>
        public void StopAllWaves()
        {
            foreach (var wave in this.waves)
            {
                wave.StopTimer();
            }
        }

        /// <summary>
        ///     Creates and initializes all enemy waves.
        /// </summary>
        /// <param name="gameCanvas">The canvas on which the game is rendered.</param>
        private void createWaves(Canvas gameCanvas)
        {
            this.waves.Add(new EnemyWave(
                GameSettings.NorthAndSouthColor,
                Direction.TopToBottom,
                0,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height));

            this.waves.Add(new EnemyWave(
                GameSettings.NorthAndSouthColor,
                Direction.BottomToTop,
                GameSettings.SouthWaveTenSecDelay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height));

            this.waves.Add(new EnemyWave(
                GameSettings.EastAndWestColor,
                Direction.LeftToRight,
                GameSettings.EastWaveFifteenSecDelay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height));

            this.waves.Add(new EnemyWave(
                GameSettings.EastAndWestColor,
                Direction.RightToLeft,
                GameSettings.WestWaveFiveSecDelay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height));

            this.waves.Add(new EnemyWave(
                GameSettings.FinalBlitzColor,
                Direction.All,
                GameSettings.FinalBlitzDelay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height));
        }

        #endregion
    }
}