using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Enemies;
using DodgeEm.Model.Players;

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
        /// <summary>
        ///     Delegate for the PlayerPowerUp event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="isHit">Indicates if the player was hit by a power-up.</param>
        public delegate void PlayerPowerUpHandler(object sender, bool isHit);

        #endregion

        #region Data members

        private readonly DispatcherTimer mainTimer;
        private DateTime gameEndTimeUtc;

        private bool gameOverTriggered;

        private readonly int level = 1;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player manager.
        ///     Precondition: None.
        ///     Postcondition: Returns the PlayerManager instance.
        /// </summary>
        public PlayerManager PlayerManager { get; }


        private LevelManager LevelManager { get; }
        private GamePointManager GamePointManager { get; }
        private PowerUpManager PowerUpManager { get; }

        /// <summary>
        ///     Gets the scoreboard.
        /// </summary>
        /// <value>
        ///     The scoreboard.
        /// </value>
        public Scoreboard Scoreboard { get; }

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
            this.PowerUpManager = new PowerUpManager(gameCanvas, backgroundWidth, backgroundHeight);

            this.Scoreboard = new Scoreboard();

            this.gameEndTimeUtc = DateTime.UtcNow.AddSeconds(this.LevelManager.GetLevelDuration());

            this.mainTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs)
            };
            this.OnLevelChanged();
            this.mainTimer.Tick += (s, e) => this.onMainTick();
            this.mainTimer.Start();
            this.spawnGamePoint();
        }

        #endregion

        #region Methods

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

        /// <summary>
        ///     Event raised when the player power-up state changes.
        /// </summary>
        public event PlayerPowerUpHandler PlayerPowerUp;

        private void OnLevelChanged()
        {
            var colors = this.LevelManager.GetCurrentLevelWaveColors();
            this.PlayerManager.UpdatePlayerColors(colors);
        }

        private void onMainTick()
        {
            if (this.gameOverTriggered)
            {
                return;
            }

            this.handleGamePointCollisions();

            this.updateTimerUi();
            this.handleGameEndConditions();
            this.handlePowerUp();
            this.restartLevel();
        }

        private void handlePowerUp()
        {
            if (this.playerCollisionWithPowerUp())
            {
                this.PlayerPowerUp?.Invoke(this, true);
            }
        }

        private void handleGamePointCollisions()
        {
            var points = this.GamePointManager.GetGamePoints().ToList();
            foreach (var gp in points)
            {
                if (this.PlayerManager.IsPlayerTouchingGamePoint(gp))
                {
                    this.Scoreboard.AddPoints(1);

                    this.GamePointManager.RemoveGamePoint(gp);
                }
            }

            this.GamePointManager.CleanupCollected();
        }

        private void updateTimerUi()
        {
            var remaining = this.getRemainingTime();
            this.GameTimerTick?.Invoke(this, remaining);
        }

        private void handleGameEndConditions()
        {
            if (this.hasBallCollisionWithEnemy() && !this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() == 0)
            {
                this.endGame(false);
                return;
            }

            if (!this.hasBallCollisionWithEnemy() && this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() > 0 &&
                this.LevelManager.GetLevelId() == LevelId.Level3)
            {
                this.PowerUpManager.RemoveAllPowerUps();
                this.mainTimer.Stop();
                this.LevelManager.StopLevel();
                this.endGame(true);
            }
        }

        private void restartLevel()
        {
            if (!this.hasBallCollisionWithEnemy() && this.hasTimeExpired() && this.PlayerManager.GetPlayerLives() > 0 &&
                this.LevelManager.GetLevelId() < LevelId.Level3)
            {
                this.LevelManager.NextLevel();
                this.restartGameTimer();
                this.OnLevelChanged();
                this.spawnGamePoint();
                this.PowerUpManager.SpawnPowerUp();
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

        private bool hasBallCollisionWithEnemy()
        {
            foreach (var enemyBall in this.LevelManager.GetEnemyBalls())
            {
                if (this.PlayerManager.IsPlayerTouchingEnemyBall(enemyBall) &&
                    !this.PlayerManager.HasSameColors(enemyBall))
                {
                    this.LevelManager.RestartCurrentLevel();
                    this.restartGameTimer();
                    this.updatePlayerLives();
                    this.PowerUpManager.RestartPowerUp();
                    return true;
                }
            }

            return false;
        }

        private bool playerCollisionWithPowerUp()
        {
            foreach (var powerUp in this.PowerUpManager.PowerUps)
            {
                if (this.PlayerManager.IsPlayerTouchingEnemyBall(powerUp))
                {
                    this.PowerUpManager.RemovePowerUp(powerUp);
                    this.LevelManager.RemoveAllBalls();
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
            this.GamePointManager.AddGamePoints(this.LevelManager.GetCurrentLevelGamePoints(),
                this.LevelManager.GetLevelId());
        }

        #endregion
    }
}