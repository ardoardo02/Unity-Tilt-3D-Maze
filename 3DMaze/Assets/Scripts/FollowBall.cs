using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] float speed;

    public bool isMoving => this.transform.position == ball.Position;

    private void Update()
    {
        // this.transform.position = ball.Position;

        if (this.transform.position == ball.Position)
            return;

        this.transform.position = Vector3.Lerp(
            this.transform.position,
            ball.Position,
            Time.deltaTime * speed
        );

        if (ball.isMoving)
            return;

        // kalau udah deket, langsung teleport aja
        if (Vector3.Distance(this.transform.position, ball.Position) < 0.1f)
        {
            transform.position = ball.Position;
        }
    }
}
