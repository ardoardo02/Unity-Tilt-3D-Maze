using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 5f;

    public Vector3 Position { get => rb.position; }
    public bool isMoving => rb.velocity != Vector3.zero;
    public bool IsTeleporting => isTeleporting;

    Vector3 lastPosition;
    bool isTeleporting;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            lastPosition = this.transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
            rb.AddForce(Vector3.forward * Input.GetAxisRaw("Vertical") * speed);
        if (Input.GetAxisRaw("Horizontal") != 0)
            rb.AddForce(Vector3.right * Input.GetAxisRaw("Horizontal") * speed);
    }

    internal void AddForce(Vector3 force)
    {
        rb.isKinematic = false;
        lastPosition = this.transform.position;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.5f && rb.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Out" && !isTeleporting)
            StartCoroutine(DelayedTeleport());

        if (other.gameObject.tag == "Wall")
            audioManager.PlayHitWall();
    }

    IEnumerator DelayedTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(2);
        trailRenderer.enabled = false;
        rb.isKinematic = true;
        audioManager.PlayBallOut();
        this.transform.position = lastPosition;
        isTeleporting = false;
        yield return new WaitForSeconds(0.5f);
        trailRenderer.enabled = true;
    }
}
