using System;
using System.Collections.Generic;
using DodgeEm.Model.Enemies;
using DodgeEm.Model.Players;

namespace DodgeEm.Model
{
    /// <summary>
    /// Manages attack logic and collision detection between player and enemy balls.
    /// </summary>
    public class AttackManager
    {
        

        #region Methods

        /// <summary>
        /// Determines whether the player has been hit by any enemy ball.
        /// Precondition: player is not null; EnemyBalls is not null.
        /// Postcondition: Returns true if any enemy ball is touching the player, otherwise false.
        /// </summary>
        /// <param name="player">The player to check for collisions.</param>
        /// <param name="enemyBalls">A collection of enemy balls to check against the player.</param>
        /// <returns>True if the player is hit by any enemy ball; otherwise, false.</returns>
        public bool IsPlayerHit(Player player, IEnumerable<EnemyBall> enemyBalls)
        {
            foreach (var enemyBall in enemyBalls)
            {
                if (isTouching(player, enemyBall))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool isTouching(Player player, EnemyBall enemyBall)
        {
            var deltaX = player.X - enemyBall.X;
            var deltaY = player.Y - enemyBall.Y;
            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            var playerRadius = player.Width / 2.0;
            var enemyRadius = enemyBall.Width / 2.0;

            return distance < playerRadius + enemyRadius;
        }

        #endregion
    }
}