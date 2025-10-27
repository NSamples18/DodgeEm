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
                    currentLevel.StopAllLevels();
                    this.currentLevelIndex++;
                    currentLevel.StartLevel();
                }
            }
        }

        public void StartAllLevels()
        {
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
        }

        private void addLevels(Canvas gameCanvas)
        {
            this.levels.Add(new Level(LevelId.Level1, 0, 60000, gameCanvas));
            this.levels.Add(new Level(LevelId.Level2, 60001, 120000, gameCanvas));
            this.levels.Add(new Level(LevelId.Level3, 120001, 180000, gameCanvas));
        }

    }
}
