using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGravity : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioManager audioManager;

    [SerializeField] float gravityMagnitude;

    bool useGyro;
    Vector3 gravityDir;

    private void Start() {
        if(SystemInfo.supportsGyroscope)
        {
            useGyro = true;
            Input.gyro.enabled = true;
        }
    }

    private void Update() {
        var inputDir = useGyro ? Input.gyro.gravity : Input.acceleration;

        gravityDir = new Vector3(
            inputDir.y,
            inputDir.z,
            -inputDir.x
        );
    }

    private void FixedUpdate() {
        rb.AddForce(gravityDir * gravityMagnitude, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall")
            audioManager.PlayHitWall();
    }
}
