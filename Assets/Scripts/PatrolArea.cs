using UnityEngine;
using UnityEngine.Events;

public class PatrolArea : MonoBehaviour
{
    public static bool isPlayerInside => _isPlayerInside;
    static bool _isPlayerInside;

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
        {
            return;
        }

        _isPlayerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
        {
            return;
        }

        _isPlayerInside = false;
    }
}
