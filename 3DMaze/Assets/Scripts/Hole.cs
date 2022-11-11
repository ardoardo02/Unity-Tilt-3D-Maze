using System;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Trigger trigger;
    [SerializeField] Boolean isWin;
    public Action <Collider, Boolean> HoleEnterEvent;

    // public bool Entered { get => entered; }

    private void Start() {
        trigger.TriggerEnterEvent += OnTriggerEnterEvent;
    }

    private void OnDestroy() {
        trigger.TriggerEnterEvent -= OnTriggerEnterEvent;
    }

    private void OnTriggerEnterEvent(Collider obj)
    {
        HoleEnterEvent?.Invoke(obj, isWin);
        if(isWin) particle.Play();
    }
}
