using System;

public class BeatPlayedEventArgs : EventArgs
{
    public int beat;
    public double dspTime;

    public BeatPlayedEventArgs(int beat, double dspTime)
    {
        this.beat = beat;
        this.dspTime = dspTime;
    }
}
