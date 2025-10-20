using Windows.UI;
using Windows.UI.Xaml.Media;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model
{
    /// <summary>
    ///     Manages the player.
    /// </summary>
    /// <seealso cref="GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private bool swapColor = true;

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
        ///     Swaps the color of the player's ball.
        ///     Precondition: None.
        ///     Postcondition: The player's ball color is swapped.
        /// </summary>
        public void SwapBallColor()
        {
            if (Sprite is PlayerSprite playerSprite)
            {
                if (this.swapColor)
                {
                    playerSprite.InnerFill = new SolidColorBrush(Colors.Red);
                    playerSprite.OuterFill = new SolidColorBrush(Colors.Orange);
                    this.swapColor = false;
                }
                else
                {
                    playerSprite.InnerFill = new SolidColorBrush(Colors.Orange);
                    playerSprite.OuterFill = new SolidColorBrush(Colors.Red);
                    this.swapColor = true;
                }
            }
        }
        /// <summary>
        ///     Checks if the player has the same color as the enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player has the same color as the enemy ball, otherwise false.
        /// </summary>
        public bool HasSameColors(GameObject otherBall)
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


        #endregion
    }
}