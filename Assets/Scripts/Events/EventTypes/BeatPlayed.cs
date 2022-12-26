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

    public override string ToString() => $"BeatPlayedEventArgs(beat: {beat}, dspTime: {dspTime})";
}

public partial class EventManager
{
    public EventHandler OnBeatPlayed;
    public void BeatPlayed(object sender, BeatPlayedEventArgs args)
    {
#if UNITY_EDITOR
        Log($"BeatPlayed! {args}");
#endif
        OnBeatPlayed?.Invoke(sender, args);

        DelayedEvent e = new DelayedEvent();
        e.trigger = (object s, EventArgs a) => BeatPlayed_delayed(s, (BeatPlayedEventArgs)a);
        e.sender = sender;
        e.args = args;
        DelayedEvents.Enqueue(e);
    }

    public EventHandler OnBeatPlayed_delayed;
    public void BeatPlayed_delayed(object sender, BeatPlayedEventArgs args)
    {
#if UNITY_EDITOR
        Log($"BeatPlayed_delayed! {args}");
#endif
        OnBeatPlayed_delayed?.Invoke(sender, args);
    }
}
