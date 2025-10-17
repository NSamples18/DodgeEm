using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model
{
    /// <summary>
    /// Represents an enemy ball in the game.
    /// </summary>
    public class EnemyBall : GameObject
    {
        #region Data members

        public Direction direction { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyBall"/> class.
        /// Precondition: speed >= 0
        /// Postcondition: EnemyBall is created with specified color, direction, and speed.
        /// </summary>
        /// <param name="color">The color of the enemy ball.</param>
        /// <param name="direction">The direction the ball moves.</param>
        /// <param name="speed">The speed of the ball.</param>
        public EnemyBall(Color color, Direction direction, int speed)
        {
            this.direction = direction;
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
        /// Moves the enemy ball in its assigned direction.
        /// Precondition: None.
        /// Postcondition: EnemyBall position is updated according to its direction and speed.
        /// </summary>
        public void Move()
        {
            switch (this.direction)
            {
                case Direction.TopToBottom:
                    MoveDown();
                    break;
                case Direction.BottomToTop:
                    MoveUp();
                    break;
                case Direction.LeftToRight:
                    MoveLeft();
                    break;
                case Direction.RightToLeft:
                    MoveRight();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}