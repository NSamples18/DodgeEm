using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Core;
using DodgeEm.Model.Game;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Players
{
    /// <summary>
    ///     Manages the player.
    /// </summary>
    /// <seealso cref="GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private int playerLives = GameSettings.PlayerLives;
        private List<Color> availableColors = new List<Color>();
        private int currentColorIndex;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(GameSettings.PlayerSpeedXDirection, GameSettings.PlayerSpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the available colors for the player.
        /// </summary>
        public void SetAvailableColors(IEnumerable<Color> colors)
        {
            this.availableColors = colors.ToList();
            this.availableColors.RemoveAt(this.availableColors.Count - 1);
            this.currentColorIndex = 0;
            this.setPlayerColor();
        }

        private void setPlayerColor()
        {
            if (Sprite is PlayerSprite playerSprite)
            {
                playerSprite.setPlayerColor(this.availableColors[this.currentColorIndex]);
            }
        }

        /// <summary>
        ///     Swaps the player's color to the next available color.
        /// </summary>
        public void SwapToNextColor()
        {
            if (this.availableColors.Count == 0)
            {
                return;
            }

            this.currentColorIndex = (this.currentColorIndex + 1) % this.availableColors.Count;
            this.setPlayerColor();
        }

        /// <summary>
        ///     Checks if the player is touching an enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player is touching the enemy ball, otherwise false.
        ///     <param name="enemyBall">The enemy ball to check for collision.</param>
        /// </summary>
        public virtual bool IsTouchingEnemyBall(GameObject enemyBall)
        {
            var playerCenterX = XCord + Width / 2.0;
            var playerCenterY = YCord + Height / 2.0;
            var enemyCenterX = enemyBall.XCord + enemyBall.Width / 2.0;
            var enemyCenterY = enemyBall.YCord + enemyBall.Height / 2.0;

            var playerRadius = Width / 2.0;
            var enemyRadius = enemyBall.Width / 2.0;

            var dx = playerCenterX - enemyCenterX;
            var dy = playerCenterY - enemyCenterY;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance <= playerRadius + enemyRadius;
        }

        /// <summary>
        ///     Checks if the player has the same color as the enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player has the same color as the enemy ball, otherwise false.
        /// </summary>
        public bool IsSameColor(GameObject otherBall)
        {
            if (Sprite is PlayerSprite playerSprite)
            {
                var playerOuterColor = ((SolidColorBrush)playerSprite.OuterFill).Color;

                if (otherBall.Sprite is EnemySprite enemySprite)
                {
                    var enemyColor = ((SolidColorBrush)enemySprite.Fill).Color;
                    return playerOuterColor == enemyColor;
                }
            }

            return false;
        }

        /// <summary>
        ///     Called when the player loses a life.
        /// </summary>
        public void PlayerLosesLife()
        {
            this.playerLives--;
            if (Sprite is PlayerSprite playerSprite)
            {
                playerSprite.PlayDeathAnimation();
            }
        }

        /// <summary>
        ///     Gets the number of lives the player has left.
        /// </summary>
        public int GetPlayerLives()
        {
            return this.playerLives;
        }

        #endregion
    }
}