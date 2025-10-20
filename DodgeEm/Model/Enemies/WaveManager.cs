using System.Collections.Generic;
using System.Linq;
using Windows.UI;
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
            var waveDefinitions = getWaveDefinitions();
            this.addWaves(gameCanvas, waveDefinitions);
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
        ///     Returns the definitions for all waves (color + direction).
        /// </summary>
        private static (Color color, Direction direction)[] getWaveDefinitions()
        {
            return new[]
            {
                (GameSettings.NorthAndSouthColor, Direction.TopToBottom),
                (GameSettings.EastAndWestColor, Direction.RightToLeft),
                (GameSettings.NorthAndSouthColor, Direction.BottomToTop),
                (GameSettings.EastAndWestColor, Direction.LeftToRight),
                (GameSettings.FinalBlitzColor, Direction.All)
            };
        }

        /// <summary>
        ///     Creates and adds waves based on definitions and calculated delays.
        /// </summary>
        private void addWaves(Canvas gameCanvas, (Color color, Direction direction)[] waveDefinitions)
        {
            for (var i = 0; i < waveDefinitions.Length; i++)
            {
                var (color, direction) = waveDefinitions[i];
                var delay = GameSettings.DelayInterval * i;

                var wave = createWave(gameCanvas, color, direction, delay);
                this.waves.Add(wave);
            }
        }

        /// <summary>
        ///     Creates an individual enemy wave.
        /// </summary>
        private static EnemyWave createWave(Canvas gameCanvas, Color color, Direction direction, int delay)
        {
            return new EnemyWave(
                color,
                direction,
                delay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height);
        }

        #endregion
    }
}