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

        private bool swapColor = true;
        private int playerLives = 3;
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

        public void SetAvailableColors(IEnumerable<Color> colors)
        {
            this.availableColors = colors.ToList();
            this.availableColors.RemoveAt(this.availableColors.Count - 1);
            this.currentColorIndex = 0;
            this.SetPlayerColor(this.availableColors[this.currentColorIndex]);
        }

        public void SwapToNextColor()
        {
            if (this.availableColors.Count == 0)
            {
                return;
            }

            this.currentColorIndex = (this.currentColorIndex + 1) % this.availableColors.Count;
            this.SetPlayerColor(this.availableColors[this.currentColorIndex]);
        }

        private void SetPlayerColor(Color color)
        {
            if (Sprite is PlayerSprite playerSprite)
            {
                playerSprite.InnerFill = new SolidColorBrush(Colors.Blue);
                playerSprite.OuterFill = new SolidColorBrush(color);
            }
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

        public void PlayerLosesLife()
        {
            this.playerLives--;
        }

        public int GetPlayerLives()
        {
            return this.playerLives;
        }

        #endregion
    }
}