using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DodgeEm.Model.Game
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Scoreboard : INotifyPropertyChanged
    {
        #region Data members

        private int currentScore;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current score.
        /// </summary>
        /// <value>
        ///     The current score.
        /// </value>
        public int CurrentScore
        {
            get => this.currentScore;
            set
            {
                if (this.currentScore == value)
                {
                    return;
                }

                this.currentScore = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.ScoreDisplay));
            }
        }

        /// <summary>
        ///     Gets the score display.
        /// </summary>
        /// <value>
        ///     The score display.
        /// </value>
        public string ScoreDisplay => $"Score: {this.CurrentScore}";

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Adds the points.
        /// </summary>
        /// <param name="points">The points.</param>
        public void AddPoints(int points)
        {
            if (points <= 0)
            {
                return;
            }

            this.CurrentScore += points;
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}