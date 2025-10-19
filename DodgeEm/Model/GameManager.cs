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

        #endregion

        #region Data members

        private readonly DispatcherTimer gameTimer;
        private DispatcherTimer winTimer;

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

            this.gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs)
            };
            this.gameTimer.Tick += (s, e) => this.checkGameOverShowLose();
            this.gameTimer.Start();

            this.setUpWinTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Event raised when the game is over.
        /// </summary>
        public event GameOverHandler GameOver;

        private void setUpWinTimer()
        {
            this.winTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(GameSettings.GameEnds)
            };
            this.winTimer.Tick += (s, e) => this.showWin();
            this.winTimer.Start();
        }

        private void triggerGameOver(bool didWin)
        {
            if (this.gameOverTriggered)
            {
                return;
            }

            this.stopGame();
            this.gameOverTriggered = true;
            this.GameOver?.Invoke(this, didWin);
        }

        private void checkGameOverShowLose()
        {
            if (this.ballCollision())
            {
                this.triggerGameOver(false);
            }
        }

        private void showWin()
        {
            if (!this.ballCollision())
            {
                this.triggerGameOver(true);
            }
        }

        private void stopGame()
        {
            this.WaveManager.StopAllWaves();
            this.gameTimer.Stop();
            this.winTimer.Stop();
        }

        private bool ballCollision()
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