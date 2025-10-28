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
        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.levels[this.currentLevelIndex].GetEnemyBalls();
        }

        public LevelManager(Canvas gameCanvas)
        {
            this.levels = new List<Level>();
            this.addLevels(gameCanvas);
            this.StartLevel();

        }

        public LevelId GetLevelId()
        {
            return this.levels[this.currentLevelIndex].GetLevelId();
        }

        public int GetLevelDuration()
        {
            if (this.currentLevelIndex < this.levels.Count)
            {
                return this.levels[this.currentLevelIndex].stopLevel;
            }
            return 0;
        }

        public void NextLevel()
        {
            this.levels[this.currentLevelIndex].NextLevel();
            this.currentLevelIndex++;
            this.StartLevel();

        }

        public void StopLevel()
        {
            this.levels[this.currentLevelIndex].StopLevel();
        }

        public void StartLevel()
        {
            this.levels[this.currentLevelIndex].StartLevel();
        }

        public void RestartCurrentLevel()
        {
            this.levels[this.currentLevelIndex].ResetLevel();

        }

        private void addLevels(Canvas gameCanvas)
        {
            this.levels.Add(new Level(LevelId.Level1, 10, gameCanvas));
            this.levels.Add(new Level(LevelId.Level2, 10, gameCanvas));
            this.levels.Add(new Level(LevelId.Level3, 10, gameCanvas));
        }

    }
}
