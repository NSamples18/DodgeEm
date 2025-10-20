using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
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
        public delegate void GameOverHandler(object sender, bool didWin);

        /// <summary>
        ///     Delegate for the GameTimerTick event.
        /// </summary>
        public delegate void GameTimerTickHandler(object sender, TimeSpan remainingTime);

        #endregion

        #region Data members

        private readonly DispatcherTimer mainTimer;
        private readonly DateTime gameEndTimeUtc;

        private bool gameOverTriggered;

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
            this.WaveManager = new WaveManager(gameCanvas);

            this.gameEndTimeUtc = DateTime.UtcNow.AddSeconds(GameSettings.GameEnds);

            this.mainTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs)
            };
            this.mainTimer.Tick += (s, e) => this.onMainTick();
            this.mainTimer.Start();
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
            if (this.hasBallCollision() && !this.hasTimeExpired())
            {
                this.endGame(false);
                return;
            }

            if (!this.hasBallCollision() && this.hasTimeExpired())
            {
                this.endGame(true);
            }
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
            this.WaveManager.StopAllWaves();
            this.mainTimer.Stop();
        }

        private bool hasBallCollision()
        {
            foreach (var enemyBall in this.WaveManager.EnemyBalls)
            {
                if (this.PlayerManager.IsPlayerTouchingEnemyBall(enemyBall) &&
                    !this.PlayerManager.HasSameColors(enemyBall))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}