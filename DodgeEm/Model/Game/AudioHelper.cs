using System;
using System.Runtime.InteropServices;

public static class AudioHelper
{
    [DllImport("winmm.dll")]
    private static extern long mciSendString(string command, IntPtr buffer, int bufferSize, IntPtr hwndCallback);

    public static void Play(string filePath)
    {
        // Close any previous sound
        mciSendString("close MediaFile", IntPtr.Zero, 0, IntPtr.Zero);

        // Open and play the new sound
        mciSendString($"open \"{filePath}\" type mpegvideo alias MediaFile", IntPtr.Zero, 0, IntPtr.Zero);
        mciSendString("play MediaFile", IntPtr.Zero, 0, IntPtr.Zero);
    }
}