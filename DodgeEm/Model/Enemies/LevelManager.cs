using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Enemies
{
    public class LevelManager
    {
        #region Data members

        private readonly List<Level> levels;
        private int currentLevelIndex;

        #endregion

        #region Constructors

        public LevelManager(Canvas gameCanvas)
        {
            this.levels = new List<Level>();
            this.addLevels(gameCanvas);
            this.StartLevel();
        }

        #endregion

        #region Methods

        public IEnumerable<EnemyBall> GetEnemyBalls()
        {
            return this.levels[this.currentLevelIndex].GetEnemyBalls();
        }

        public IEnumerable<Color> GetCurrentLevelWaveColors()
        {
            return this.levels[this.currentLevelIndex].GetWaveColors();
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

        public int GetCurrentLevelGamePoints()
        {
            return this.levels[this.currentLevelIndex].gamePoint;
        }

        public void NextLevel()
        {
            this.levels[this.currentLevelIndex].NextLevel();
            this.currentLevelIndex++;
            this.levels[this.currentLevelIndex].StartLevel();
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
            this.levels.Add(new Level(LevelId.Level1, 25, 1, gameCanvas));
            this.levels.Add(new Level(LevelId.Level2, 30, 2, gameCanvas));
            this.levels.Add(new Level(LevelId.Level3, 35, 3, gameCanvas));
        }

        #endregion
    }
}