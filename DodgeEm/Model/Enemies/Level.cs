using DodgeEm.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    public class Level
    {
        private readonly WaveManager waveManager;

        /// <summary>
        /// Gets the level number.
        /// </summary>
        private readonly LevelId LevelNumber;

        public int stopLevel { get; }

        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.waveManager.EnemyBalls;
        }

        public IEnumerable<Color> GetWaveColors()
        {
            return this.waveManager.GetCurrentLevelWaveColors(this.LevelNumber);
        }

        private int elapsedMilliseconds;

        public int gamePoint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary> IList<Color> enemyColors, IList<Object> pointObjects,
        /// <param name="levelNumber">The level number.</param>
        /// <param name="startLevelTime">The start level.</param>
        /// <param name="stopLevel">The stop level.</param>
        public Level(LevelId levelNumber, int stopLevel, int numOfGamePoint, Canvas gameCanvas)
        {
            this.waveManager = new WaveManager(gameCanvas, levelNumber);
            this.LevelNumber = levelNumber;
            this.stopLevel = stopLevel;
            this.gamePoint = numOfGamePoint;
        }

        public LevelId GetLevelId()
        {
            return this.LevelNumber;
        }

        public void NextLevel()
        {
            this.StopLevel();
            this.waveManager.RemoveBallsFromAllWavesInLevel();
        }


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

        public void ResetLevel()
        {
            this.waveManager.RestartWavesInLevel();
        }

        public void RemoveAllBalls()
        {
            this.waveManager.RemoveAllBalls();
        }
    }
}
