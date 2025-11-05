using Windows.UI.Xaml.Media;

namespace DodgeEm.View.Sprites
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class PowerUpSprite
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the fill.
        /// </summary>
        /// <value>
        ///     The fill.
        /// </value>
        public Brush Fill
        {
            get => this.powerUpEllipse.Fill;
            set => this.powerUpEllipse.Fill = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerUpSprite" /> class.
        /// </summary>
        public PowerUpSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}