using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Manages the spawning and removal of power-ups in the game.
    /// </summary>
    public class PowerUpManager
    {
        #region Data members

        private readonly Canvas currentCanvas;
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the list of active power-ups.
        /// </summary>
        public IList<PowerUp> PowerUps { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerUpManager" /> class.
        /// </summary>
        /// <param name="canvas">The canvas on which power-ups are drawn.</param>
        /// <param name="width">The width of the canvas.</param>
        /// <param name="height">The height of the canvas.</param>
        public PowerUpManager(Canvas canvas, double width, double height)
        {
            this.PowerUps = new List<PowerUp>();
            this.currentCanvas = canvas;
            this.canvasWidth = width;
            this.canvasHeight = height;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Spawns a new power-up.
        /// </summary>
        public void SpawnPowerUp()
        {
            var powerUp = new PowerUp(this.currentCanvas, this.canvasWidth, this.canvasHeight);
            this.PowerUps.Add(powerUp);
        }

        /// <summary>
        ///     Restarts all active power-ups.
        /// </summary>
        public void RestartPowerUp()
        {
            foreach (var powerUp in this.PowerUps)
            {
                powerUp.RestartPowerUp();
            }
        }

        /// <summary>
        ///     Removes a power-up from the game.
        /// </summary>
        public void RemovePowerUp(PowerUp powerUp)
        {
            this.PowerUps.Remove(powerUp);
            powerUp.RemovePowerUp();
        }
        /// <summary>
        ///     Removes all power-ups from the game.
        /// </summary>
        public void RemoveAllPowerUps()
        {
            foreach (var powerUp in this.PowerUps)
            {
                powerUp.RemovePowerUp();
            }

            this.PowerUps.Clear();
        }

        #endregion
    }
}