using Windows.UI.Xaml.Media;

namespace DodgeEm.View.Sprites
{
    /// <summary>
    /// </summary>
    /// <seealso cref="DodgeEm.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class GamePointSprite
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
            get => this.pointEllipse.Fill;
            set => this.pointEllipse.Fill = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePointSprite" /> class.
        /// </summary>
        public GamePointSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}