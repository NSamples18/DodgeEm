using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DodgeEm.Model.Game
{
    public class GamePointManager
    {
        private readonly List<GamePoint> gamePoints;
        private int currentPointIndex = 0;
        private double canvasWidth;
        private double canvasHeight;
        private Canvas currentCanvas;
        public GamePointManager(Canvas currentCanvas, double canvasWidth, double canvasHeight)
        {
            this.gamePoints = new List<GamePoint>();
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            this.currentCanvas = currentCanvas;
        }
        public void AddGamePoints(int numOfPoints, LevelId levelId)
        {
            for (int i = 0; i < numOfPoints; i++)
            {
                this.gamePoints.Add(new GamePoint(this.currentCanvas, this.canvasWidth, this.canvasHeight, levelId));
            }
        }
    }
}
