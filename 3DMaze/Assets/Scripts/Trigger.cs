using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Action <Collider> TriggerEnterEvent;
    bool entered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name);
        if(entered) return;

        entered = true;
        TriggerEnterEvent?.Invoke(other);
    }
}
