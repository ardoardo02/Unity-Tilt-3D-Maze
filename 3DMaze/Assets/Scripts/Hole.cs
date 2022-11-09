using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    bool entered = false;

    public bool Entered { get => entered; }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        entered = true;
        particle.Play();
    }
}
