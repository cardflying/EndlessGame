using System;

public static class GameEventManager
{
    // Define the event blueprint (can accept string, int, or custom classes)
    public static event Action<string> OnMessageBroadcasted;

    // A simple method anyone can call to trigger the event
    public static void RaiseMessage(string message)
    {
        OnMessageBroadcasted?.Invoke(message);
    }
}