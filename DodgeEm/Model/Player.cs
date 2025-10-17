using DodgeEm.View.Sprites;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace DodgeEm.Model
{
    /// <summary>
    ///     Manages the player.
    /// </summary>
    /// <seealso cref="GameObject" />
    public class Player : GameObject
    {
        private bool swapColor = true;
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(GameSettings.PlayerSpeedXDirection, GameSettings.PlayerSpeedYDirection);
        }

        public void SwapBallColor()
        {
            if (Sprite is PlayerSprite playerSprite)
            {
                if (swapColor)
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

        #endregion
    }
}