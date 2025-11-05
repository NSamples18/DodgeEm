using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using DodgeEm.Model.Game;

namespace DodgeEm.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        private GameManager gameManager;
        private readonly Leaderboard leaderboard;

        private readonly HashSet<VirtualKey> keysDown = new HashSet<VirtualKey>();

        private bool swapHandledOnCurrentSpacePress;
        private bool gameStarted;

        private readonly DispatcherTimer moveTimer;
        private readonly DispatcherTimer powerUpTextTimer;

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

            var localPath = ApplicationData.Current.LocalFolder.Path;
            var savePath = Path.Combine(localPath, "leaderboard.txt");
            this.leaderboard = new Leaderboard(savePath);

            this.powerUpTextTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            this.powerUpTextTimer.Tick += this.PowerUpTextTimer_Tick;
        }

        #endregion

        #region Methods

        private async void updatePlayerPowerUp(object sender, bool isHit)
        {
            if (isHit)
            {
                this.powerUpText.Visibility = Visibility.Visible;
                this.powerUpTextTimer.Stop();
                this.powerUpTextTimer.Start();

                await AudioHelper.PlayAsync("mixkit-arcade-bonus-alert-767.wav");
            }
        }

        private void PowerUpTextTimer_Tick(object sender, object e)
        {
            this.powerUpTextTimer.Stop();
            this.powerUpText.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     GameOver handler — delegate name prompt + insertion to LeaderboardDialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="didWin">if set to <c>true</c> [did win].</param>
        private async void onGameOverEvent(object sender, bool didWin)
        {
            var finalScore = this.gameManager.Scoreboard.CurrentScore;

            try
            {
                await LeaderboardDialog.PromptForNameAndInsertIfTopTenAsync(this.leaderboard, finalScore);
            }

            catch
            {
                Debug.Print("Failed to add score to leaderboard.");
            }

            if (didWin)
            {
                this.win.Visibility = Visibility.Visible;
                await AudioHelper.PlayAsync("roblox-old-winning-sound-effect.mp3");
            }
            else
            {
                this.lose.Visibility = Visibility.Visible;
                await AudioHelper.PlayAsync("vine-boom-sound-effect(chosic.com).mp3");
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
            if (this.gameStarted)
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
        ///     Show the leaderboard — keep a simple guard and delegate rendering to LeaderboardDialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
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
                Debug.WriteLine("Failed to show leaderboard dialog.");
            }
        }

        private void resetLeaderboard(object sender, RoutedEventArgs e)
        {
            if (this.keysDown.Contains(VirtualKey.Space))
            {
                return;
            }

            try
            {
                this.leaderboard.Reset();
            }
            catch
            {
                Debug.WriteLine("Failed to reset leaderboard.");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var applicationWidth = (double)Application.Current.Resources["CanvasWidth"];
            var applicationHeight = (double)Application.Current.Resources["CanvasHeight"];
            this.gameManager = new GameManager(applicationHeight, applicationWidth, this.canvas);
            DataContext = this.gameManager.Scoreboard;

            this.gameManager.GameOver += this.onGameOverEvent;
            this.gameManager.GameTimerTick += this.updateUiGameTimer;
            this.gameManager.PlayerLivesChanged += this.updateLifeCount;
            this.gameManager.PlayerPowerUp += this.updatePlayerPowerUp;

            this.gameManager.LevelStarted += this.onLevelStarted;
            this.gameManager.WaveStarted += this.onWaveStarted;

            this.startButton.Visibility = Visibility.Collapsed;
            this.leaderboardButton.Visibility = Visibility.Collapsed;

            this.gameStarted = true;

            _ = AudioHelper.PlayAsync("mixkit-retro-game-notification-212.wav");
        }

        private async void onLevelStarted(object sender, LevelId levelId)
        {
            try
            {
                await AudioHelper.PlayAsync("mixkit-retro-game-notification-212.wav");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to play level start sound: {ex}");
            }
        }

        private async void onWaveStarted(object sender)
        {
            try
            {
                await AudioHelper.PlaySoundEffectAsync("mixkit-arcade-game-jump-coin-216.wav");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to play wave-start SFX: {ex}");
            }
        }

        #endregion
    }
}