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

        private readonly ICollection<EnemyWave> waves;
        private readonly LevelId level;

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
        /// <param name="level">The level.</param>
        public WaveManager(Canvas gameCanvas, LevelId level)
        {
            this.waves = new List<EnemyWave>();
            var waveDefinitions = getWaveDefinitions();
            this.addWaves(gameCanvas, waveDefinitions);
            this.level = level;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the colors of the enemy balls in the current level.
        /// </summary>
        public IEnumerable<Color> GetCurrentLevelWaveColors()
        {
            return this.waves
                .Where(w => w.LevelId == this.level)
                .Select(w => w.BallColor)
                .Distinct();
        }

        /// <summary>
        ///     Removes all enemy balls from all waves in the current level.
        /// </summary>
        public void RemoveBallsFromAllWavesInLevel()
        {
            foreach (var wave in this.waves)
            {
                if (wave.LevelId == this.level)
                {
                    wave.RemoveAllBalls();
                }
            }
        }

        /// <summary>
        ///     Removes all enemy balls from all waves.
        /// </summary>
        public void RemoveAllBalls()
        {
            foreach (var wave in this.waves)
            {
                wave.RemoveAllBalls();
            }
        }

        /// <summary>
        ///     Stops all enemy waves.
        ///     Precondition: None.
        ///     Postcondition: VerticalMixed waves are stopped and no new enemies will spawn.
        /// </summary>
        public void StopWave()
        {
            foreach (var wave in this.waves)
            {
                if (wave.LevelId == this.level)
                {
                    wave.StopTimer();
                }
            }
        }

        /// <summary>
        ///     Restarts all waves in the current level.
        /// </summary>
        public void RestartWavesInLevel()
        {
            foreach (var wave in this.waves)
            {
                if (wave.LevelId == this.level)
                {
                    wave.ResetWave();
                    wave.RemoveAllBalls();
                }
            }
        }

        /// <summary>
        ///     Starts all waves in the current level.
        /// </summary>
        public void StartWaveWithLevel()
        {
            foreach (var wave in this.waves)
            {
                if (wave.LevelId == this.level)
                {
                    wave.StartWave();
                }
            }
        }

        private static (LevelId levelId, Color color, Direction direction)[] getWaveDefinitions()
        {
            return new[]
            {
                (LevelId.Level1, GameSettings.Level1NorthAndSouthColor, Direction.TopToBottom),
                (LevelId.Level1, GameSettings.Level1EastAndWestColor, Direction.LeftToRight),
                (LevelId.Level1, GameSettings.Level1NorthAndSouthColor, Direction.BottomToTop),
                (LevelId.Level1, GameSettings.Level1EastAndWestColor, Direction.RightToLeft),
                (LevelId.Level1, GameSettings.FinalBlitzColor, Direction.VerticalMixed),

                (LevelId.Level2, GameSettings.Level2North, Direction.TopToBottom),
                (LevelId.Level2, GameSettings.Level2East, Direction.LeftToRight),
                (LevelId.Level2, GameSettings.Level2South, Direction.BottomToTop),
                (LevelId.Level2, GameSettings.Level2West, Direction.RightToLeft),
                (LevelId.Level2, GameSettings.FinalBlitzColor, Direction.VerticalMixed),

                (LevelId.Level3, GameSettings.Level3North, Direction.TopToBottom),
                (LevelId.Level3, GameSettings.Level3East, Direction.LeftToRight),
                (LevelId.Level3, GameSettings.Level3South, Direction.BottomToTop),
                (LevelId.Level3, GameSettings.Level3West, Direction.RightToLeft),
                (LevelId.Level3, GameSettings.FinalBlitzColor, Direction.DiagonalMixed)
            };
        }

        private void addWaves(Canvas gameCanvas, (LevelId levelId, Color color, Direction direction)[] waveDefinitions)
        {
            var groupedByLevel = waveDefinitions.GroupBy(w => w.levelId);

            foreach (var levelGroup in groupedByLevel)
            {
                var index = 0;

                foreach (var (levelId, color, direction) in levelGroup)
                {
                    var delay = 3000 * index;
                    var wave = createWave(levelId, gameCanvas, color, direction, delay);

                    wave.WaveStarted += s => this.WaveStarted?.Invoke(this);

                    this.waves.Add(wave);
                    index++;
                }
            }
        }

        private static EnemyWave createWave(LevelId level, Canvas gameCanvas, Color color, Direction direction,
            int delay)
        {
            return new EnemyWave(
                level,
                color,
                direction,
                delay,
                gameCanvas,
                gameCanvas.Width,
                gameCanvas.Height);
        }

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        public delegate void WaveStartedHandler(object sender);

        /// <summary>
        ///     Occurs when [wave started].
        /// </summary>
        public event WaveStartedHandler WaveStarted;

        #endregion
    }
}