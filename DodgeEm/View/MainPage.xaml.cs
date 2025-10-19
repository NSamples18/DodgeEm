using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using DodgeEm.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeEm.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        private readonly GameManager gameManager;

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
            this.gameManager = new GameManager(applicationHeight, applicationWidth, this.canvas);
            this.gameManager.GameOver += this.onGameOverEvent;
        }

        #endregion

        #region Methods

        private void onGameOverEvent(object sender, bool didWin)
        {
            if (didWin)
            {
                this.win.Visibility = Visibility.Visible;
            }
            else
            {
                this.lose.Visibility = Visibility.Visible;
            }
        }

        private void CoreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.PlayerManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.PlayerManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.PlayerManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.PlayerManager.MovePlayerDown();
                    break;
                case VirtualKey.Space:
                    this.gameManager.PlayerManager.SwapPlayerBallColor();
                    break;
            }
        }

        #endregion
    }
}