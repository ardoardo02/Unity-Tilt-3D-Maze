using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text countText;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] FollowBall camPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;
    [SerializeField] AudioManager audioManager;

    Vector3 lastMouseposition;
    float ballDistance;
    bool isShooting;
    Vector3 forceDir;
    float forceFactor;
    Renderer[] arrowRends;
    int shootCount;
    RectTransform aimRect;

    public int ShootCount { get => shootCount; }
    public bool IsShooting { get => isShooting; }

    private void Start()
    {
        aimRect = aim.GetComponent<RectTransform>();
        ballDistance = Vector3.Distance(cam.transform.position, ball.Position) + 2f;

        arrowRends = GetComponentsInChildren<Renderer>();
        arrow.SetActive(false);
        line.enabled = false;
    }

    void Update()
    {
        if (ball.isMoving && aim.enabled)
            aim.enabled = false;

        if (ball.isMoving || ball.IsTeleporting || ball.Position.y < 0)
            return;


        if (aimRect.anchoredPosition.x != cam.WorldToScreenPoint(ball.Position).x)
        {
            aimRect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
            aim.enabled = true;
        }

        if (this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;

            StartCoroutine(UpdateCursorUI());
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, ballDistance, ballLayer))
            {
                isShooting = true;
                arrow.SetActive(true);
                line.enabled = true;
            }
        }

        // shooting
        if (Input.GetMouseButton(0) && isShooting)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);

                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 10);
                forceFactor = forceMagnitude / 10;
            }

            // arrow
            arrow.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3(1 + 0.5f * forceFactor, 1 + 0.5f * forceFactor, 1 + 2 * forceFactor);

            foreach (var rend in arrowRends)
            {
                rend.material.color = Color.Lerp(Color.Lerp(Color.green, Color.yellow, forceFactor * 2), Color.red, forceFactor);
            }

            // aim
            aimRect = aim.GetComponent<RectTransform>();
            aimRect.anchoredPosition = Input.mousePosition;

            //line
            var ballScrPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[] { ballScrPos, Input.mousePosition });
        }

        // camera
        if (Input.GetMouseButton(0) && !isShooting)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMouseposition);

            var delta = current - last;

            camPivot.transform.RotateAround(ball.Position, Vector3.up, delta.x * camSensitivity.x);
            camPivot.transform.RotateAround(ball.Position, cam.transform.right, -delta.y * camSensitivity.y);

            var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

            // kalau melewati batas putar balik
            if (angle < 2)
            {
                camPivot.transform.RotateAround(ball.Position, cam.transform.right, 2 - angle);
            }
            else if (angle > 65)
            {
                camPivot.transform.RotateAround(ball.Position, cam.transform.right, 65 - angle);
            }
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            ball.AddForce(forceDir * shootForce * forceFactor);
            audioManager.PlayGolfSwingSFX();
            shootCount++;
            countText.text = "Stroke: " + ShootCount;
            forceFactor = 0;
            forceDir = Vector3.zero;

            isShooting = false;

            arrow.SetActive(false);
            aim.enabled = false;
            line.enabled = false;
        }

        lastMouseposition = Input.mousePosition;
    }

    IEnumerator UpdateCursorUI()
    {
        yield return new WaitForSeconds(1);

        aimRect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        aim.enabled = true;
    }
}
