using System;
using Windows.Foundation;
using DodgeEm.View.Sprites;

namespace DodgeEm.Model.Core
{
    /// <summary>
    ///     Defines basics of every game object.
    /// </summary>
    public abstract class GameObject
    {
        #region Data members

        private Point location;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x location of the game object.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.location.X;
            set
            {
                this.location.X = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets or sets the y location of the game object.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.location.Y;
            set
            {
                this.location.Y = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets the x speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed x.
        /// </value>
        public int SpeedX { get; private set; }

        /// <summary>
        ///     Gets the y speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed y.
        /// </value>
        public int SpeedY { get; private set; }

        /// <summary>
        ///     Gets the width of the game object.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height of the game object.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets the sprite associated with the game object.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the game object right.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveRight()
        {
            this.moveX(this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object left.
        ///     Precondition: None
        ///     Postcondition: X == X@prev + SpeedX
        /// </summary>
        public void MoveLeft()
        {
            this.moveX(-this.SpeedX);
        }

        /// <summary>
        ///     Moves the game object up.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev - SpeedY
        /// </summary>
        public void MoveUp()
        {
            this.moveY(-this.SpeedY);
        }

        /// <summary>
        ///     Moves the game object down.
        ///     Precondition: None
        ///     Postcondition: Y == Y@prev + SpeedY
        /// </summary>
        public void MoveDown()
        {
            this.moveY(this.SpeedY);
        }

        /// <summary>
        ///     Sets the speed of the game object.
        ///     Precondition: speedX >= 0 AND speedY >=0
        ///     Postcondition: SpeedX == speedX AND SpeedY == speedY
        /// </summary>
        /// <param name="speedX">The speed x.</param>
        /// <param name="speedY">The speed y.</param>
        public void SetSpeed(int speedX, int speedY)
        {
            if (speedX < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedX));
            }

            if (speedY < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speedY));
            }

            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        /// <summary>
        ///     Checks if the player is touching an enemy ball.
        ///     Precondition: enemyBall is not null.
        ///     Postcondition: Returns true if the player is touching the enemy ball, otherwise false.
        ///     <param name="enemyBall">The enemy ball to check for collision.</param>
        /// </summary>
        public virtual bool IsTouchingEnemyBall(GameObject enemyBall)
        {
            var playerCenterX = this.X + this.Width / 2.0;
            var playerCenterY = this.Y + this.Height / 2.0;
            var enemyCenterX = enemyBall.X + enemyBall.Width / 2.0;
            var enemyCenterY = enemyBall.Y + enemyBall.Height / 2.0;

            var playerRadius = this.Width / 2.0;
            var enemyRadius = enemyBall.Width / 2.0;

            var dx = playerCenterX - enemyCenterX;
            var dy = playerCenterY - enemyCenterY;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance <= playerRadius + enemyRadius;
        }

        private void moveX(int x)
        {
            this.X += x;
        }

        private void moveY(int y)
        {
            this.Y += y;
        }

        private void render()
        {
            ISpriteRenderer render = this.Sprite;

            render?.RenderAt(this.X, this.Y);
        }

        #endregion
    }
}