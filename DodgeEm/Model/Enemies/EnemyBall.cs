using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Core;
using DodgeEm.Model.Game;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Enemies
{
    /// <summary>
    ///     Represents an enemy ball in the game.
    /// </summary>
    public class EnemyBall : GameObject
    {
        #region Properties

        /// <summary>
        ///     Gets the Direction the enemy ball is moving.
        /// </summary>
        public Direction Direction { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyBall" /> class.
        ///     Precondition: speed >= 0
        ///     Postcondition: EnemyBall is created with specified color, Direction, and speed.
        /// </summary>
        /// <param name="color">The color of the enemy ball.</param>
        /// <param name="direction">The Direction the ball moves.</param>
        /// <param name="speed">The speed of the ball.</param>
        public EnemyBall(Color color, Direction direction, int speed)
        {
            this.Direction = direction;
            var enemySprite = new EnemySprite
            {
                Fill = new SolidColorBrush(color)
            };
            Sprite = enemySprite;
            SetSpeed(speed, speed);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the enemy ball in its assigned Direction.
        ///     Precondition: None.
        ///     Postcondition: EnemyBall position is updated according to its Direction and speed.
        /// </summary>
        public void Move()
        {
            switch (this.Direction)
            {
                case Direction.TopToBottom:
                    MoveDown();
                    break;
                case Direction.BottomToTop:
                    MoveUp();
                    break;
                case Direction.LeftToRight:
                    MoveRight();
                    break;
                case Direction.RightToLeft:
                    MoveLeft();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}