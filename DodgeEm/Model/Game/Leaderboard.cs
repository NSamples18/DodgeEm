using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DodgeEm.Model.Game
{
    /// <summary>
    ///     Simple name+score entry for text-file leaderboards (no JSON).
    ///     Saved format: one entry per line as "Name|Score".
    ///     Loading is robust and accepts legacy lines like "Name - Score" or just "Score".
    /// </summary>
    public class LeaderboardEntry
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Converts to string.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Name} | {this.Score}";
        }

        #endregion
    }

    /// <summary>
    ///     Manages a simple top-ten leaderboard stored in a text file.
    /// </summary>
    public class Leaderboard
    {
        #region Data members

        private readonly LeaderboardEntry[] topten;
        private readonly string saveFilePath;

        #endregion

        #region Properties

        /// <summary>
        ///     Expose entries (copy) so callers cannot mutate internal array directly.
        ///     Gets the top ten.
        /// </summary>
        /// <value>
        ///     The top ten.
        /// </value>
        public LeaderboardEntry[] TopTen => this.topten.ToArray();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Leaderboard" /> class.
        /// </summary>
        /// <param name="saveFilePath">The save file path.</param>
        public Leaderboard(string saveFilePath = null)
        {
            this.topten = new LeaderboardEntry[10];
            this.saveFilePath = saveFilePath;

            if (!string.IsNullOrWhiteSpace(this.saveFilePath))
            {
                try
                {
                    this.LoadFromFile(this.saveFilePath);
                }
                catch
                {
                    Debug.Print("Failed to load leaderboard.");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns true if score would be in the top ten (or there is space).
        ///     Determines whether [is top ten] [the specified score].
        /// </summary>
        /// <param name="score">The score.</param>
        /// <returns>
        ///     <c>true</c> if [is top ten] [the specified score]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTopTen(int score)
        {
            if (score < 0)
            {
                return false;
            }

            var existing = this.topten.Where(e => e != null).Select(e => e.Score).ToList();
            if (existing.Count < this.topten.Length)
            {
                return true;
            }

            return score > existing.Min();
        }

        /// <summary>
        ///     Insert name+score if it qualifies; returns the 0-based index where inserted or null if not inserted.
        ///     Inserts the score.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int? InsertScore(int score, string name)
        {
            if (score < 0)
            {
                return null;
            }

            var entry = new LeaderboardEntry
            {
                Name = string.IsNullOrWhiteSpace(name) ? "Anonymous" : name.Trim().Replace("|", " "),
                Score = score
            };

            var list = this.topten.Where(e => e != null).ToList();
            list.Add(entry);

            var top = list
                .OrderByDescending(e => e.Score)
                .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                .Take(this.topten.Length)
                .ToList();

            if (!top.Contains(entry))
            {
                return null;
            }

            for (var i = 0; i < this.topten.Length; i++)
            {
                this.topten[i] = i < top.Count ? top[i] : null;
            }

            if (!string.IsNullOrWhiteSpace(this.saveFilePath))
            {
                try
                {
                    this.SaveToFile(this.saveFilePath);
                }
                catch
                {
                    Debug.Print("Failed to save leaderboard.");
                }
            }

            return Array.FindIndex(this.topten, e => e == entry);
        }

        /// <summary>
        ///     Load entries from file. Accepts "Name|Score", "Name - Score", or just "Score".
        ///     Loads from file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void LoadFromFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return;
            }

            var lines = File.ReadAllLines(path);
            for (var i = 0; i < this.topten.Length; i++)
            {
                if (i >= lines.Length)
                {
                    this.topten[i] = null;
                    continue;
                }

                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    this.topten[i] = null;
                    continue;
                }

                var pipeIndex = line.IndexOf('|');
                if (pipeIndex >= 0)
                {
                    var namePart = line.Substring(0, pipeIndex).Trim();
                    var scorePart = line.Substring(pipeIndex + 1).Trim();
                    if (int.TryParse(scorePart, out var sc))
                    {
                        this.topten[i] = new LeaderboardEntry { Name = namePart, Score = sc };
                        continue;
                    }
                }

                var dashIndex = line.LastIndexOf('-');
                if (dashIndex > 0)
                {
                    var namePart = line.Substring(0, dashIndex).Trim();
                    var scorePart = line.Substring(dashIndex + 1).Trim();
                    if (int.TryParse(scorePart, out var sc2))
                    {
                        this.topten[i] = new LeaderboardEntry { Name = namePart, Score = sc2 };
                        continue;
                    }
                }

                if (int.TryParse(line, out var sc3))
                {
                    this.topten[i] = new LeaderboardEntry { Name = "Anonymous", Score = sc3 };
                }
                else
                {
                    this.topten[i] = null;
                }
            }
        }

        /// <summary>
        ///     Save in the simple pipe-delimited format "Name|Score" — one entry per line.
        ///     Uses atomic write (temp file + replace).
        ///     Saves to file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.InvalidOperationException">No save path specified.</exception>
        public void SaveToFile(string path = null)
        {
            var target = path ?? this.saveFilePath;
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new InvalidOperationException("No save path specified.");
            }

            var dir = Path.GetDirectoryName(target);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var temp = target + ".tmp";
            var lines = this.topten.Select(e => e == null ? string.Empty : $"{e.Name}|{e.Score}").ToArray();
            File.WriteAllLines(temp, lines);

            if (File.Exists(target))
            {
                File.Replace(temp, target, null);
            }
            else
            {
                File.Move(temp, target);
            }
        }

        internal void AddScore(int finalScore)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}