using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DodgeEm.Model.Game;

namespace DodgeEm.View
{
    /// <summary>
    /// Stores methods to show leaderboard dialogs.
    /// </summary>
    public static class LeaderboardDialog
    {

        /// <summary>
        /// Shows the leaderboard asynchronous.
        /// Shows the read-only leaderboard (used from menu)
        /// </summary>
        /// <param name="leaderboard">The leaderboard.</param>
        public static async Task ShowLeaderboardAsync(Leaderboard leaderboard)
        {
            var entries = leaderboard.TopTen
                .Where(e => e != null)
                .Select((e, idx) => formatEntry(e, idx))
                .ToArray();

            var content = entries.Length > 0 ? string.Join("\n", entries) : "No scores yet.";
            var tb = new TextBlock { Text = content, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(4) };

            var dlg = new ContentDialog { Title = "Leaderboard", Content = tb, CloseButtonText = "Close" };
            await dlg.ShowAsync();
        }



        /// <summary>
        /// Prompts for name and insert if top ten asynchronous.
        /// If `leaderboard` exposes IsTopTen and InsertScore, prompt for name and insert.
        /// </summary>
        /// <param name="leaderboard">The leaderboard.</param>
        /// <param name="score">The score.</param>
        public static async Task PromptForNameAndInsertIfTopTenAsync(Leaderboard leaderboard, int score)
        {
            if (leaderboard == null)
            {
                return;
            }

            bool qualifies = false;
            try
            {
                qualifies = leaderboard.IsTopTen(score);
            }
            catch
            {
                Debug.Print("Failed to check if score qualifies for leaderboard.");
            }

            if (!qualifies)
            {
                return;
            }

            var nameBox = new TextBox
            {
                PlaceholderText = "Name (max 20)", MaxLength = 20
            };
            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = $"You scored {score}. Enter your name:"
            });
            panel.Children.Add(nameBox);

            var dlg = new ContentDialog
            {
                Title = "New High Score!",
                Content = panel,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel"
            };

            var result = await dlg.ShowAsync();
            var name = result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(nameBox.Text)
                ? nameBox.Text.Trim()
                : "Anonymous";

            try { leaderboard.InsertScore(score, name); }
            catch {
                try
                {
                    leaderboard.AddScore(score);
                }
                catch
                {
                    Debug.Print("Failed to add score to leaderboard.");
                }
            }
        }

        private static string formatEntry(object entry, int index)
        {
            if (entry == null)
            {
                return null;
            }
            var type = entry.GetType();
            var nameProp = type.GetProperty("Name");
            var scoreProp = type.GetProperty("Score");
            if (nameProp != null && scoreProp != null)
            {
                var name = nameProp.GetValue(entry) as string ?? "Anonymous";
                var score = scoreProp.GetValue(entry)?.ToString() ?? "0";
                return $"{index + 1}. {name} - {score}";
            }

            return $"{index + 1}. {entry}";
        }
    }
}