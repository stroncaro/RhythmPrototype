using System;
using System.Collections.Generic;

public partial class EventManager : SingletonMonoBehaviour<EventManager>
{
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
}
