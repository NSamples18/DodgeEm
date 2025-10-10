using System;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model
{
    /**
     * Manages the player in the game.
     */
    public class PlayerManager
    {
        #region Data members

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player instance.
        /// Precondition: None.
        /// Postcondition: Returns the Player instance.
        /// </summary>
        public Player Player { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Manages the player in the game.
        /// </summary>
        /// <param name="backgroundHeight">The height of the game play window.</param>
        /// <param name="backgroundWidth">The width of the game play window.</param>
        public PlayerManager(double backgroundHeight, double backgroundWidth)
        {
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the game placing player in the game
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }

            this.createAndPlacePlayer(background);
        }

        private void createAndPlacePlayer(Canvas background)
        {
            this.Player = new Player();
            background.Children.Add(this.Player.Sprite);

            this.placePlayerCenteredInGameArena();
        }

        private void placePlayerCenteredInGameArena()
        {
            this.Player.X = this.backgroundWidth / 2 - this.Player.Width / 2.0;
            this.Player.Y = this.backgroundHeight / 2 - this.Player.Height / 2.0;
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: The player has moved left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.IsPlayerOnLeftEdge())
            {
                return;
            }

            this.Player.MoveLeft();
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: The player has moved right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.IsPlayerOnRightEdge())
            {
                return;
            }

            this.Player.MoveRight();
        }

        /// <summary>
        ///     Moves the player up
        ///     Precondition: none
        ///     Postcondition: The player has moved up.
        /// </summary>
        public void MovePlayerUp()
        {
            if (this.IsPlayerOnTopEdge())
            {
                return;
            }

            this.Player.MoveUp();
        }

        /// <summary>
        ///     Moves the player down
        ///     Precondition: none
        ///     Postcondition: The player has moved down.
        /// </summary>
        public void MovePlayerDown()
        {
            if (this.IsPlayerOnBottomEdge())
            {
                return;
            }

            this.Player.MoveDown();
        }

        private bool IsPlayerOnRightEdge()
        {
            return this.Player.X + this.Player.Width >= this.backgroundWidth;
        }

        private bool IsPlayerOnLeftEdge()
        {
            return this.Player.X <= 0;
        }

        private bool IsPlayerOnTopEdge()
        {
            return this.Player.Y <= 0;
        }

        private bool IsPlayerOnBottomEdge()
        {
            return this.Player.Y + this.Player.Height >= this.backgroundHeight;
        }

        #endregion
    }
}