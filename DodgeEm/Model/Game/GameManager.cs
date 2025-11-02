using DodgeEm.Model.Enemies;
using DodgeEm.Model.Players;
using System;
using Windows.Media.PlayTo;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Types and Delegates

        /// <summary>
        ///     Delegate for the GameOver event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="didWin">Indicates if the player won.</param>
        public delegate void GameOverHandler(object sender, bool didWin);

        /// <summary>
        ///     Delegate for the GameTimerTick event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="remainingTime">The remaining time.</param>
        public delegate void GameTimerTickHandler(object sender, TimeSpan remainingTime);
        /// <summary>
        ///     Delegate for the PlayerLivesChanged event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="playerLives">The player lives.</param>
        public delegate void PlayerLivesChangedHandler(object sender, int playerLives);

        #endregion

        #region Data members

        private readonly DispatcherTimer mainTimer;
        private DateTime gameEndTimeUtc;

        private bool gameOverTriggered;

        private int level = 1;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player manager.
        ///     Precondition: None.
        ///     Postcondition: Returns the PlayerManager instance.
        /// </summary>
        public PlayerManager PlayerManager { get; }

        /// <summary>
        ///     Gets the wave manager.
        ///     Precondition: None.
        ///     Postcondition: Returns the WaveManager instance.
        /// </summary>
        private WaveManager WaveManager { get; }
        private LevelManager LevelManager { get; }
        private GamePointManager GamePointManager { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="backgroundHeight">The backgroundHeight of the game play window.</param>
        /// <param name="backgroundWidth">The backgroundWidth of the game play window.</param>
        /// <param name="gameCanvas">The Canvas element for the game.</param>
        public GameManager(double backgroundHeight, double backgroundWidth, Canvas gameCanvas)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.PlayerManager = new PlayerManager(backgroundHeight, backgroundWidth, gameCanvas);
            this.LevelManager = new LevelManager(gameCanvas);
            this.GamePointManager = new GamePointManager(gameCanvas, backgroundWidth, backgroundHeight);

            this.gameEndTimeUtc = DateTime.UtcNow.AddSeconds(this.LevelManager.GetLevelDuration());

            this.mainTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs)
            };
            OnLevelChanged();
            this.mainTimer.Tick += (s, e) => this.onMainTick();
            this.mainTimer.Start();
            this.spawnGamePoint();

        }

        #endregion

        #region Methods

        public void OnLevelChanged()
        {
            var colors = this.LevelManager.GetCurrentLevelWaveColors();
            this.PlayerManager.UpdatePlayerColors(colors);
        }

        /// <summary>
        ///     Event raised when the game is over.
        /// </summary>
        public event GameOverHandler GameOver;

        /// <summary>
        ///     Event raised on each game timer tick.
        /// </summary>
        public event GameTimerTickHandler GameTimerTick;
        /// <summary>
        ///     Event raised when the player lives change.
        /// </summary>
        public event PlayerLivesChangedHandler PlayerLivesChanged;

        private void onMainTick()
        {
            if (this.gameOverTriggered)
            {
                return;
            }

            this.updateTimerUi();
            this.handleGameEndConditions();
        }

        private void updateTimerUi()
        {
            var remaining = this.getRemainingTime();
            this.GameTimerTick?.Invoke(this, remaining);
        }

        private void handleGameEndConditions()
        {
            if (this.hasBallCollision() && !this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() == 0)
            {
                this.endGame(false);
                return;
            }

            if (!this.hasBallCollision() && this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() > 0 && this.LevelManager.GetLevelId() < LevelId.Level3)
            { 

                this.LevelManager.NextLevel();
                this.restartGameTimer();
                this.OnLevelChanged();
                this.spawnGamePoint();
            }

            if (!this.hasBallCollision() && this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() > 0 && this.LevelManager.GetLevelId() == LevelId.Level3)
            {
                this.mainTimer.Stop();
                this.LevelManager.StopLevel();
                this.endGame(true);
            }
        }

        private void restartGameTimer()
        {
            this.mainTimer.Stop();
            this.gameEndTimeUtc = DateTime.UtcNow.AddSeconds(this.LevelManager.GetLevelDuration());
            this.mainTimer.Start();
        }

        private bool hasTimeExpired()
        {
            return this.getRemainingTime() <= TimeSpan.Zero;
        }

        private TimeSpan getRemainingTime()
        {
            var remaining = this.gameEndTimeUtc - DateTime.UtcNow;
            return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
        }

        private void endGame(bool didWin)
        {
            if (this.gameOverTriggered)
            {
                return;
            }

            this.gameOverTriggered = true;
            this.stopGame();
            this.GameOver?.Invoke(this, didWin);
        }

        private void stopGame()
        {
            this.LevelManager.StopLevel();
            this.mainTimer.Stop();
        }

        private bool hasBallCollision()
        {
            foreach (var enemyBall in this.LevelManager.GetEnemyBalls())
            {
                if (this.PlayerManager.IsPlayerTouchingEnemyBall(enemyBall) && !this.PlayerManager.HasSameColors(enemyBall))
                {
                    this.LevelManager.RestartCurrentLevel();
                    this.restartGameTimer();
                    updatePlayerLives();
                    return true;
                }
            }
            return false;
        }

        private void updatePlayerLives()
        {
            this.PlayerManager.PlayerLosesLife();
            this.onPlayerLivesChanged(this.PlayerManager.GetPlayerLives());
        }

        private void onPlayerLivesChanged(int playerLives)
        {
            this.PlayerLivesChanged?.Invoke(this, playerLives);
        }

        private void spawnGamePoint()
        {
            
            this.GamePointManager.AddGamePoints(this.LevelManager.GetCurrentLevelGamePoints(), this.LevelManager.GetLevelId());
        }

        

        #endregion
    }
}