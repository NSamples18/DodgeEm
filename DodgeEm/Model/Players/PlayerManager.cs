using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Core;
using DodgeEm.Model.Game;

namespace DodgeEm.Model.Players
{
    /**
     * Manages the player in the game.
     */
    public class PlayerManager
    {
        #region Data members

        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private Player player;

        #endregion

        #region Constructors

        /// <summary>
        ///     Manages the player in the game.
        /// </summary>
        /// <param name="backgroundHeight">The height of the game play window.</param>
        /// <param name="backgroundWidth">The width of the game play window.</param>
        /// <param name="background">The canvas representing the game background.</param>
        public PlayerManager(double backgroundHeight, double backgroundWidth, Canvas background)
        {
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
            this.createAndPlacePlayer(background);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates the player's available colors.
        /// </summary>
        public void UpdatePlayerColors(IEnumerable<Color> colors)
        {
            this.player.SetAvailableColors(colors);
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

            this.player.MoveLeft();
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: The player has moved right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.isPlayerOnRightEdge())
            {
                return;
            }

            this.player.MoveRight();
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

            this.player.MoveUp();
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

            this.player.MoveDown();
        }

        /// <summary>
        ///     Swaps the color of the player's ball.
        ///     Precondition: None.
        ///     Postcondition: The player's ball color is swapped.
        /// </summary>
        public void SwapPlayerBallColor()
        {
            this.player.SwapToNextColor();
        }

        /// <summary>
        ///     Checks if the player is touching an enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player is touching the enemy ball, otherwise false.
        /// </summary>
        public bool HasPlayerCollidedWithBall(GameObject enemyBall)
        {
            return this.player.IsTouchingEnemyBall(enemyBall);
        }

        /// <summary>
        ///     Checks if the player has the same color as the enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player has the same color as the enemy ball, otherwise false.
        /// </summary>
        public bool HasSameColors(GameObject enemyBall)
        {
            return this.player.IsSameColor(enemyBall);
        }

        /// <summary>
        ///     Called when the player loses a life.
        /// </summary>
        public void PlayerLosesLife()
        {
            this.placePlayerCenteredInGameArena();
            this.player.PlayerLosesLife();
        }

        /// <summary>
        ///     Gets the number of lives the player has left.
        /// </summary>
        public int GetPlayerLives()
        {
            return this.player.GetPlayerLives();
        }

        private bool isPlayerOnRightEdge()
        {
            return this.player.XCord + this.player.Width + GameSettings.PlayerSpeedXDirection >= this.backgroundWidth;
        }

        private bool IsPlayerOnLeftEdge()
        {
            return this.player.XCord - GameSettings.PlayerSpeedXDirection <= 0;
        }

        private bool IsPlayerOnTopEdge()
        {
            return this.player.YCord - GameSettings.PlayerSpeedYDirection <= 0;
        }

        private bool IsPlayerOnBottomEdge()
        {
            return this.player.YCord + this.player.Height + GameSettings.PlayerSpeedYDirection >= this.backgroundHeight;
        }

        private void createAndPlacePlayer(Canvas background)
        {
            this.player = new Player();
            background.Children.Add(this.player.Sprite);

            this.placePlayerCenteredInGameArena();
        }

        private void placePlayerCenteredInGameArena()
        {
            this.player.XCord = this.backgroundWidth / 2 - this.player.Width / 2.0;
            this.player.YCord = this.backgroundHeight / 2 - this.player.Height / 2.0;
        }

        #endregion
    }
}