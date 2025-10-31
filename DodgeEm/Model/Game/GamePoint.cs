using System.Drawing;
using DodgeEm.Model.Core;
using DodgeEm.View.Sprites;
using Windows.UI.Xaml.Media;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DodgeEm.Model.Core.GameObject" />
    public class GamePoint : GameObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePoint"/> class.
        /// </summary>
        public GamePoint()
        {
            var gamePointSprite = new GamePointSprite()
            {
                Fill = new SolidColorBrush(GameSettings.GamePointColor)
            };
            Sprite = gamePointSprite;
        }
    }
}