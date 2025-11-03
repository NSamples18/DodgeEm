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
        #region Properties

        /// <summary>
        ///     Gets or sets the fill brush of the inner ellipse.
        ///     Precondition: innerEllipse is initialized.
        ///     Postcondition: The inner ellipse's fill color is updated.
        /// </summary>
        public Brush InnerFill
        {
            get => this.innerEllipse.Fill;
            set => this.innerEllipse.Fill = value;
        }

        /// <summary>
        ///     Gets or sets the fill brush of the outer ellipse.
        ///     Precondition: outerEllipse is initialized.
        ///     Postcondition: The outer ellipse's fill color is updated.
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
        ///     Precondition: none
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
        }


        #endregion
    }
}