using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DodgeEm.Model.Game;

namespace DodgeEm.View.Sprites
{
    /// <summary>
    ///     Handles player sprite visuals and animations.
    /// </summary>
    public sealed partial class PlayerSprite
    {
        #region Data members

        private DispatcherTimer deathTimer;
        private int tickCount;
        private Brush originalInnerFill;
        private Brush originalOuterFill;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the fill brush of the inner ellipse.
        /// </summary>
        public Brush InnerFill
        {
            get => this.innerEllipse.Fill;
            set => this.innerEllipse.Fill = value;
        }

        /// <summary>
        ///     Gets or sets the fill brush of the outer ellipse.
        /// </summary>
        public Brush OuterFill
        {
            get => this.outerEllipse.Fill;
            set => this.outerEllipse.Fill = value;
        }

        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the color of the player sprite.
        /// <param name="color">The color to set.</param>
        /// </summary>
        public void setPlayerColor(Color color)
        {
            this.InnerFill = new SolidColorBrush(Colors.Blue);
            this.OuterFill = new SolidColorBrush(color);
        }

        /// <summary>
        ///     Plays a simple death animation that flashes
        ///     red  yellow  red restores original colors.
        /// </summary>
        public void PlayDeathAnimation()
        {
            this.originalInnerFill = this.InnerFill;
            this.originalOuterFill = this.OuterFill;
            this.tickCount = 0;

            this.deathTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameSettings.DeathAnimationIntervalMs)
            };
            this.deathTimer.Tick += this.onDeathAnimationTick;
            this.deathTimer.Start();
        }


        private void onDeathAnimationTick(object sender, object e)
        {
            this.tickCount++;

            switch (this.tickCount)
            {
                case 1:
                    this.setColor(Colors.Red);
                    break;
                case 2:
                    this.setColor(Colors.Yellow);
                    break;
                case 3:
                    this.setColor(Colors.Red);
                    break;
                default:
                    this.InnerFill = this.originalInnerFill;
                    this.OuterFill = this.originalOuterFill;

                    this.deathTimer.Stop();
                    this.deathTimer.Tick -= this.onDeathAnimationTick;
                    this.deathTimer = null;
                    break;
            }
        }


        private void setColor(Color color)
        {
            this.InnerFill = new SolidColorBrush(color);
            this.OuterFill = new SolidColorBrush(color);
        }

        #endregion
    }
}