using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using DodgeEm.Model.Game;
using Windows.Storage;
using System.IO;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        private readonly GameManager gameManager;
        private readonly Leaderboard leaderboard;

        private readonly HashSet<VirtualKey> keysDown = new HashSet<VirtualKey>();

        private bool swapHandledOnCurrentSpacePress;

        private readonly DispatcherTimer moveTimer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            var applicationWidth = (double)Application.Current.Resources["CanvasWidth"];
            var applicationHeight = (double)Application.Current.Resources["CanvasHeight"];

            ApplicationView.PreferredLaunchViewSize = new Size { Width = applicationWidth, Height = applicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(applicationWidth, applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.CoreWindowOnKeyDown;
            Window.Current.CoreWindow.KeyUp += this.CoreWindowOnKeyUp;

            this.moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(GameSettings.TickIntervalMs) };
            this.moveTimer.Tick += this.MoveTimerOnTick;
            this.moveTimer.Start();

            this.gameManager = new GameManager(applicationHeight, applicationWidth, this.canvas);

            // DataContext bound to the live scoreboard for UI updates
            DataContext = this.gameManager.Scoreboard;

            // Create leaderboard stored in app local folder
            var localPath = ApplicationData.Current.LocalFolder.Path;
            var savePath = Path.Combine(localPath, "leaderboard.txt");
            this.leaderboard = new Leaderboard(savePath);

            this.gameManager.GameOver += this.onGameOverEvent;
            this.gameManager.GameTimerTick += this.updateUiGameTimer;
            this.gameManager.PlayerLivesChanged += this.updateLifeCount;
        }

        #endregion

        #region Methods

        /// <summary>
        /// GameOver handler — delegate name prompt + insertion to LeaderboardDialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="didWin">if set to <c>true</c> [did win].</param>
        private async void onGameOverEvent(object sender, bool didWin)
        {
            var finalScore = this.gameManager.Scoreboard.CurrentScore;

            try
            {
                // LeaderboardDialog handles checking IsTopTen and prompting the user.
                await LeaderboardDialog.PromptForNameAndInsertIfTopTenAsync(this.leaderboard, finalScore);
            }
            catch
            {
                
                try
                {
                    this.leaderboard.AddScore(finalScore);
                }
                catch
                {
                    Debug.Print("Failed to add score to leaderboard.");
                }
            }

            if (didWin)
            {
                this.win.Visibility = Visibility.Visible;
            }
            else
            {
                this.lose.Visibility = Visibility.Visible;
            }
        }

        private void updateUiGameTimer(object sender, TimeSpan remainingTime)
        {
            var secondsLeft = remainingTime.TotalSeconds;
            this.gameTimer.Text = $"Time: {secondsLeft:0.00}";
        }

        private void updateLifeCount(object sender, int playerLives)
        {
            this.playerLives.Text = $"Lives: {playerLives}";
        }

        private void CoreWindowOnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            this.keysDown.Remove(args.VirtualKey);

            if (args.VirtualKey == VirtualKey.Space)
            {
                this.swapHandledOnCurrentSpacePress = false;
            }
        }

        private void CoreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            this.keysDown.Add(args.VirtualKey);

            if (args.VirtualKey == VirtualKey.Space)
            {
                if (!this.swapHandledOnCurrentSpacePress)
                {
                    this.gameManager.PlayerManager.SwapPlayerBallColor();
                    this.swapHandledOnCurrentSpacePress = true;
                }
            }
        }

        private void MoveTimerOnTick(object sender, object e)
        {
            this.moveLeftOrRight();
            this.moveUpOrDown();
        }

        private void moveUpOrDown()
        {
            if (this.keysDown.Contains(VirtualKey.Up))
            {
                this.gameManager.PlayerManager.MovePlayerUp();
            }
            else if (this.keysDown.Contains(VirtualKey.Down))
            {
                this.gameManager.PlayerManager.MovePlayerDown();
            }
        }

        private void moveLeftOrRight()
        {
            if (this.keysDown.Contains(VirtualKey.Left))
            {
                this.gameManager.PlayerManager.MovePlayerLeft();
            }
            else if (this.keysDown.Contains(VirtualKey.Right))
            {
                this.gameManager.PlayerManager.MovePlayerRight();
            }
        }

        /// <summary>
        /// Show the leaderboard — keep a simple guard and delegate rendering to LeaderboardDialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void showLeaderboard(object sender, RoutedEventArgs e)
        {
            if (this.keysDown.Contains(VirtualKey.Space))
            {
                return;
            }

            try
            {
                await LeaderboardDialog.ShowLeaderboardAsync(this.leaderboard);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to show leaderboard dialog.");
            }
        }

        #endregion
    }
}