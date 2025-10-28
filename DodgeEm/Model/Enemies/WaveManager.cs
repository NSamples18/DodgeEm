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
        private LevelId level;

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
        public WaveManager(Canvas gameCanvas, LevelId level)
        {
            this.waves = new List<EnemyWave>();
            var waveDefinitions = getWaveDefinitions();
            this.addWaves(gameCanvas, waveDefinitions);
            this.level = level;
        }

        #endregion

        #region Methods

        public void RemoveBallsFromAllWaves()
        {
            foreach (var wave in this.waves)
            {
                if (wave.levelId == level)
                {
                    wave.StopTimer();
                    wave.RemoveAllBalls();
                }
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
                if (wave.levelId == this.level)
                {
                    wave.StopTimer();
                }
            }
        }

        public void RestartWavesInLevel()
        {
            foreach (var wave in this.waves)
            {
                if (wave.levelId == level)
                {
                    wave.resetWaveTimer();
                    wave.RemoveAllBalls();
                }
            }
        }

        public void startWaveWithLevel()
        {
            foreach (var enemyWave in this.waves)
            {
                if(enemyWave.levelId == level)
                {
                    enemyWave.StartWave();
                }
            }
        }

        /// <summary>
        ///     Returns the definitions for all waves (color + direction).
        /// </summary>
        private static (LevelId levelId, Color color, Direction direction)[] getWaveDefinitions()
        {
            return new[]
            {
                (LevelId.Level1, GameSettings.NorthAndSouthColor, Direction.TopToBottom),
                (LevelId.Level1, GameSettings.EastAndWestColor, Direction.LeftToRight),
                (LevelId.Level2, GameSettings.NorthAndSouthColor, Direction.BottomToTop),
                (LevelId.Level3, GameSettings.EastAndWestColor, Direction.RightToLeft),
                (LevelId.Level3, GameSettings.FinalBlitzColor, Direction.VerticalMixed)
            };
        }

        /// <summary>
        ///     Creates and adds waves based on definitions and calculated delays.
        /// </summary>
        private void addWaves(Canvas gameCanvas, (LevelId levelId, Color color, Direction direction)[] waveDefinitions)
        {
            for (var i = 0; i < waveDefinitions.Length; i++)
            {
                var (levelId, color, direction) = waveDefinitions[i];
              //  var delay = GameSettings.DelayInterval * i;

                var wave = createWave(levelId, gameCanvas, color, direction, 0);
                this.waves.Add(wave);
            }
        } 

        /// <summary>
        ///     Creates an individual enemy wave.
        /// </summary>
        private static EnemyWave createWave(LevelId level, Canvas gameCanvas, Color color, Direction direction, int delay)
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
    }
}