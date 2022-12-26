using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Attempting to create an EventManager, but one already exists");
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private struct DelayedEvent
    {
        public Action<object, EventArgs> trigger;
        public object sender;
        public EventArgs args;
    }

    private Queue<DelayedEvent> DelayedEvents = new Queue<DelayedEvent>();

    private void Update()
    {
        while (DelayedEvents.Count > 0)
        {
            DelayedEvent e = DelayedEvents.Dequeue();
            e.trigger.Invoke(e.sender, e.args);
        }
    }

    public EventHandler OnBeatPlayed;
    public void BeatPlayed(object sender, BeatPlayedEventArgs args)
    {
        //Debug.Log($"BeatPlayed: beat {args.beat} at {args.dspTime}");
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
        //Debug.Log($"BeatPlayed_delayed: beat {args.beat} at {args.dspTime}");
        OnBeatPlayed_delayed?.Invoke(sender, args);
    }
}
