// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI.Xaml.Media;

namespace DodgeEm.View.Sprites
{
    /// <summary>
    ///     Draws the player.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class PlayerSprite
    {
        public Brush InnerFill
        {
            get => this.InnerEllipse.Fill;
            set => this.InnerEllipse.Fill = value;
        }

        public Brush OuterFill
        {
            get => this.OuterEllipse.Fill;
            set => this.OuterEllipse.Fill = value;
        }

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        ///     Precondition: none
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}