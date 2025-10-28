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
        private DispatcherTimer timer;
        private readonly TimeSpan tickInterval = TimeSpan.FromMilliseconds(20);

        private int elapsedMilliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary> IList<Color> enemyColors, IList<Object> pointObjects,
        /// <param name="levelNumber">The level number.</param>
        /// <param name="startLevelTime">The start level.</param>
        /// <param name="stopLevel">The stop level.</param>
        public Level(LevelId levelNumber, int stopLevel, Canvas gameCanvas)
        {
            this.waveManager = new WaveManager(gameCanvas);
            this.LevelNumber = levelNumber;
            this.stopLevel = stopLevel;
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
        }


        public void StopLevel()
        { 
            this.waveManager.StopAllWaves(this.LevelNumber); 
            this.timer.Stop();
        }

        public void StopTimer()
        {
            this.timer.Stop();
            this.timer.Tick -= this.Timer_Tick;
        }

        /// <summary>
        ///     Starts the internal timer for the wave.
        ///     Precondition: None.
        ///     Postcondition: Timer is running and ticks will occur.
        /// </summary>
        public void StartLevel()
        {
            if (this.timer != null)
            {
                this.timer.Tick += this.Timer_Tick;
                this.timer.Start();
                this.waveManager.startWaveWithLevel(this.LevelNumber);
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            this.elapsedMilliseconds += (int)this.timer.Interval.TotalMilliseconds;

            if (this.elapsedMilliseconds >= this.stopLevel)
            {
                this.waveManager.EndCurrentWaves(this.LevelNumber);
                this.StopTimer();
            }
        }

        public void ResetLevel()
        {
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
            this.waveManager.RestartWavesInLevel(this.LevelNumber);

        }
    }
}
