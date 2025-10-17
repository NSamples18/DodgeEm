using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private readonly DispatcherTimer gameTimer;
        private DispatcherTimer winTimer;

        private readonly TextBlock winTextBlock;
        private readonly TextBlock loseTextBlock;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player manager.
        /// Precondition: None.
        /// Postcondition: Returns the PlayerManager instance.
        /// </summary>
        public PlayerManager PlayerManager { get; }

        /// <summary>
        /// Gets the wave manager.
        /// Precondition: None.
        /// Postcondition: Returns the WaveManager instance.
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
        /// <param name="winTextBlock">The TextBlock to display win messages.</param>
        /// <param name="loseTextBlock">The TextBlock to display lose messages.</param>
        public GameManager(double backgroundHeight, double backgroundWidth, TextBlock winTextBlock,
            TextBlock loseTextBlock, Canvas gameCanvas)
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

            this.winTextBlock = winTextBlock;
            this.loseTextBlock = loseTextBlock;

            this.gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs)
            };
            this.gameTimer.Tick += (s, e) => this.OnGameOver();
            this.gameTimer.Start();

            this.setUpWinTimer();
        }

        private void setUpWinTimer()
        {
            this.winTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(GameSettings.GameEnds)
            };
            this.winTimer.Tick += (s, e) => this.showWin();
            this.winTimer.Start();
        }

        #endregion

        #region Methods

        private bool OnGameOver()
        {
            if (this.ballCollision())
            {
                this.stopGame();
                this.loseTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private void showWin()
        {
            if(this.OnGameOver())
            {
                this.winTextBlock.Visibility = Visibility.Visible;
            }

            this.stopGame();
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
                if (this.PlayerManager.IsPlayerTouchingEnemyBall(enemyBall) && !this.PlayerManager.HasSameColors(enemyBall))
                {
                    return true;
                }
            }
            return false;
        }

        

        #endregion
    }
}