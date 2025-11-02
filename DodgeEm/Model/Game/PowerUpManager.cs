using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    public class PowerUpManager
    {
        private readonly IList<PowerUp> powerUps;
        private Canvas currentCanvas;
        private double canvasWidth;
        private double canvasHeight;

        public IList<PowerUp> PowerUps
        {
            get { return this.powerUps; }
        }

        public PowerUpManager(Canvas canvas, double width, double height)
        {
            this.powerUps = new List<PowerUp>();
            this.currentCanvas = canvas;
            this.canvasWidth = width;
            this.canvasHeight = height;
        }

        public void SpawnPowerUp()
        {
            var powerUp = new PowerUp(this.currentCanvas, this.canvasWidth, this.canvasHeight);
            this.powerUps.Add(powerUp);
        }

        public void RestartPowerUp()
        {
            foreach (var powerUp in this.powerUps)
            {
                powerUp.RestartPowerUp();
            }
        }

        public void RemovePowerUp(PowerUp powerUp)
        {
            this.powerUps.Remove(powerUp);
            powerUp.RemovePowerUp();
        }
    }
}