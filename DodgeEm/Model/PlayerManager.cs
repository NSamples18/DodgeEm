using DodgeEm.View.Sprites;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

        private Player player;

        #endregion

        #region Constructors

        /// <summary>
        ///     Manages the player in the game.
        /// </summary>
        /// <param name="backgroundHeight">The height of the game play window.</param>
        /// <param name="backgroundWidth">The width of the game play window.</param>
        public PlayerManager(double backgroundHeight, double backgroundWidth, Canvas background)
        {
            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
            this.createAndPlacePlayer(background);
        }

        #endregion

        #region Methods

        private void createAndPlacePlayer(Canvas background)
        {
            this.player = new Player();
            background.Children.Add(this.player.Sprite);

            this.placePlayerCenteredInGameArena();
        }

        private void placePlayerCenteredInGameArena()
        {
            this.player.X = this.backgroundWidth / 2 - this.player.Width / 2.0;
            this.player.Y = this.backgroundHeight / 2 - this.player.Height / 2.0;
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
            if (this.IsPlayerOnRightEdge())
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

        public void SwapPlayerBallColor()
        {
            this.player.SwapBallColor();
        }

        /// <summary>
        ///     Checks if the player is touching an enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player is touching the enemy ball, otherwise false.
        ///     Param name="enemyBall">The enemy ball to check for collision.</param>
        /// </summary>
        public bool IsPlayerTouchingEnemyBall(GameObject enemyBall)
        {
            var playerCenterX = this.player.X + this.player.Width / 2.0;
            var playerCenterY = this.player.Y + this.player.Height / 2.0;
            var enemyCenterX = enemyBall.X + enemyBall.Width / 2.0;
            var enemyCenterY = enemyBall.Y + enemyBall.Height / 2.0;

            var playerRadius = this.player.Width / 2.0;
            var enemyRadius = enemyBall.Width / 2.0;

            var dx = playerCenterX - enemyCenterX;
            var dy = playerCenterY - enemyCenterY;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance <= playerRadius + enemyRadius;
        }

       

        private bool IsPlayerOnRightEdge()
        {
            return this.player.X + this.player.Width + GameSettings.PlayerSpeedXDirection >= this.backgroundWidth;
        }

        private bool IsPlayerOnLeftEdge()
        {
            return this.player.X - GameSettings.PlayerSpeedXDirection <= 0;
        }

        private bool IsPlayerOnTopEdge()
        {
            return this.player.Y - GameSettings.PlayerSpeedYDirection <= 0;
        }

        private bool IsPlayerOnBottomEdge()
        {
            return this.player.Y + this.player.Height + GameSettings.PlayerSpeedYDirection >= this.backgroundHeight;
        }

        public bool HasSameColors(EnemyBall enemyBall)
        {
            return this.player.HasSameColors(enemyBall);
        }

        #endregion
    }
}