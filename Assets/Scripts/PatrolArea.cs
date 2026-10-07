using UnityEngine;
using UnityEngine.Events;

public class PatrolArea : MonoBehaviour
{
    public static UnityEvent<Player> IntruderDetectedEvent;
    public static UnityEvent<Player> IntruderLeftEvent;

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
        {
            return;
        }

        IntruderDetectedEvent.Invoke(player);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
        {
            return;
        }

        IntruderLeftEvent.Invoke(player);
    }
}
