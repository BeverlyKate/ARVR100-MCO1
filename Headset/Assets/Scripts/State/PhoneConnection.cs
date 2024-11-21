/// <summary>
/// Manages the connection between the VR headset and the phone.
/// </summary>
public static class PhoneConnection
{
    public static string ConnectionId { get; private set; }

    static PhoneConnection()
    {
        ConnectionId = "XXXX-YYYY-ZZZZ-WWWW";
    }

    // TODO:
    // Emit an event when the connection is established.
    // Emit an event when the connection is lost.
    // Emit an event on phone message
}
