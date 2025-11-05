using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace DodgeEm.Model.Game
{


    /// <summary>
    ///     Provides audio playback functionality.
    /// </summary>
    public static class AudioHelper
    {
        #region Data members

        private static MediaPlayer backgroundPlayer;

        #endregion

        #region Methods

        /// <summary>
        /// Plays the asynchronous.
        /// </summary>
        /// <param name="assetFileName">Name of the asset file.</param>
        public static async Task PlayAsync(string assetFileName)
        {
            try
            {
                if (backgroundPlayer == null)
                {
                    backgroundPlayer = new MediaPlayer { AutoPlay = true, Volume = 1.0 };
                }

                var uri = new Uri($"ms-appx:///Assets/{assetFileName}");
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var source = MediaSource.CreateFromStorageFile(file);

                backgroundPlayer.Source = source;
                backgroundPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AudioHelper.PlayAsync failed: {ex}");
            }
        }

        /// <summary>
        /// Plays the sound effect asynchronous.
        /// </summary>
        /// <param name="assetFileName">Name of the asset file.</param>
        public static async Task PlaySoundEffectAsync(string assetFileName)
        {
            Debug.WriteLine($"[AudioHelper] PlaySoundEffectAsync requested: {assetFileName}");
            try
            {
                var uri = new Uri($"ms-appx:///Assets/{assetFileName}");
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var source = MediaSource.CreateFromStorageFile(file);

                var sfxPlayer = new MediaPlayer { AutoPlay = true, Volume = 1.0 };
                sfxPlayer.Source = source;

                sfxPlayer.MediaEnded += (s, e) =>
                {
                    Debug.WriteLine($"[AudioHelper] SFX MediaEnded: {assetFileName}");
                    try
                    {
                        sfxPlayer.Source = null;
                        sfxPlayer.Dispose();
                    }
                    catch
                    {
                        Debug.WriteLine($"[AudioHelper] SFX MediaEnded: {assetFileName}");
                    }
                };
                sfxPlayer.MediaFailed += (s, e) =>
                {
                    Debug.WriteLine($"[AudioHelper] SFX MediaFailed: {assetFileName}");
                    try
                    {
                        sfxPlayer.Dispose();
                    }
                    catch
                    {
                        Debug.WriteLine($"[AudioHelper] SFX MediaFailed: {assetFileName}");
                    }
                };

                Debug.WriteLine($"[AudioHelper] SFX starting: {assetFileName}");
                sfxPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AudioHelper.PlaySoundEffectAsync failed: {ex}");
            }
        }

        #endregion
    }
}