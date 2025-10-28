using DodgeEm.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Enemies
{
    public class LevelManager
    {
        private readonly List<Level> levels;
        private int currentLevelIndex = 0;

        private DispatcherTimer timer;
        private readonly TimeSpan tickInterval = TimeSpan.FromMilliseconds(20);
        private int elapsedMilliseconds = 0;

        public LevelManager(Canvas gameCanvas)
        {
            this.levels = new List<Level>();
            this.addLevels(gameCanvas);
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
            this.StartAllLevels();

        }

        public void StopLevel()
        {
            this.timer.Tick -= this.Timer_Tick;
            this.levels[this.currentLevelIndex].StopLevel();
            this.timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            this.elapsedMilliseconds += (int)this.tickInterval.TotalMilliseconds;
            if (this.currentLevelIndex < this.levels.Count)
            {
                var currentLevel = this.levels[this.currentLevelIndex];
                if (this.elapsedMilliseconds >= currentLevel.stopLevel)
                {
                    this.resartTimer();

                    currentLevel.StopLevel();
                    this.currentLevelIndex++;
                }
            }
        }

        private void resartTimer()
        {
            this.timer = new DispatcherTimer { Interval = this.tickInterval };
            this.elapsedMilliseconds = 0;
        }

        public void StartAllLevels()
        {
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
            this.levels[this.currentLevelIndex].StartLevel();

        }

        public void RestartCurrentLevel()
        {
            this.levels[this.currentLevelIndex].ResetLevel();
            this.resartTimer();

        }

        private void addLevels(Canvas gameCanvas)
        {
            this.levels.Add(new Level(LevelId.Level1, 5000, gameCanvas));
            this.levels.Add(new Level(LevelId.Level2, 30000, gameCanvas));
            this.levels.Add(new Level(LevelId.Level3, 35000, gameCanvas));
        }

    }
}
