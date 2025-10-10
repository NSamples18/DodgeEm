using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DodgeEm.View.Sprites
{
    /// <summary>
    /// Represents the visual sprite for an enemy ball in the game.
    /// Provides access to the fill color of the enemy's ellipse shape.
    /// </summary>
    public sealed partial class EnemySprite
    {
        #region Properties

        /// <summary>
        /// Gets or sets the fill brush of the enemy ellipse.
        /// Precondition: EnemyEllipse is initialized.
        /// Postcondition: The ellipse's fill color is updated.
        /// </summary>
        public Brush Fill
        {
            get => this.enemyEllipse.Fill;
            set => this.enemyEllipse.Fill = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemySprite"/> class.
        /// Precondition: None.
        /// Postcondition: The enemy sprite is initialized and ready for use.
        /// </summary>
        public EnemySprite()
        {
            this.InitializeComponent();
        }

        #endregion
    }
}