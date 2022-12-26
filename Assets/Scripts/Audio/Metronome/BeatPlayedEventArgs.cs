using System;

internal class BeatPlayedEventArgs : EventArgs
{
    internal int beat;
    internal double dspTime;

    internal BeatPlayedEventArgs(int beat, double dspTime)
    {
        this.beat = beat;
        this.dspTime = dspTime;
    }
}
