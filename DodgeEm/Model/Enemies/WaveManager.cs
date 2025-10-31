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

        public IEnumerable<Color> GetCurrentLevelWaveColors(LevelId levelId)
        {
            return this.waves
                .Where(w => w.levelId == this.level)
                .Select(w => w.BallColor)
                .Distinct();
        }
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
                if (wave.levelId == level)
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
                    wave.resetWave();
                    wave.RemoveAllBalls();
                }
            }
        }

        public void startWaveWithLevel()
        {
            foreach (var wave in this.waves)
            {
                if (wave.levelId == level)
                {
                    wave.StartWave();
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
                (LevelId.Level1, GameSettings.NorthAndSouthColor, Direction.BottomToTop),
                (LevelId.Level1, GameSettings.EastAndWestColor, Direction.RightToLeft),
                (LevelId.Level1, GameSettings.FinalBlitzColor, Direction.VerticalMixed),

                (LevelId.Level2, Colors.Pink, Direction.TopToBottom),
                (LevelId.Level2, Colors.Brown, Direction.LeftToRight),
                (LevelId.Level2, Colors.CadetBlue, Direction.BottomToTop),
                (LevelId.Level2, Colors.Bisque, Direction.RightToLeft),
                (LevelId.Level2, Colors.Aqua, Direction.VerticalMixed),

                (LevelId.Level3, Colors.Blue, Direction.TopToBottom),
                (LevelId.Level3, Colors.Green, Direction.LeftToRight),
                (LevelId.Level3, Colors.Gray, Direction.BottomToTop),
                (LevelId.Level3, GameSettings.EastAndWestColor, Direction.RightToLeft),
                (LevelId.Level3, GameSettings.FinalBlitzColor, Direction.DiagonalMixed)
            };
        }

        /// <summary>
        ///     Creates and adds waves based on definitions and calculated delays.
        /// </summary>
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

                    this.waves.Add(wave);
                    index++;
                }
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