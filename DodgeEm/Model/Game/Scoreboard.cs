using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DodgeEm.Model.Game
{
    public class Scoreboard : INotifyPropertyChanged
    {
        #region Data members

        private int currentScore;

        #endregion

        #region Properties

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

        public string ScoreDisplay => $"Score: {this.CurrentScore}";

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddPoints(int points)
        {
            if (points <= 0)
            {
                return;
            }

            this.CurrentScore += points;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}