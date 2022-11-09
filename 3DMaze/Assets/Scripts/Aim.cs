using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Aim : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] float speed;
    float timer;
    float z;
    bool debounce = false;

    private void Update()
    {
        if (timer > 1)
            timer = 0;

        timer += Time.deltaTime * (speed * (player.IsShooting ? 10f : 1f));

        if (player.IsShooting && !debounce)
        {
            StartCoroutine(Kecilin());
        }

        if (Input.GetMouseButtonUp(0))
        {
            this.transform.localScale = Vector3.one;
            debounce = false;
        }

        z = Mathf.Lerp(0, 360, timer);
        this.transform.localRotation = Quaternion.Euler(0, 0, z);
    }

    IEnumerator Kecilin()
    {
        debounce = true;
        yield return null;
        this.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f);
    }

    // public void SpeedUp(bool val)
    // {
    //     speed = val ? speed * 2 : speed / 2;
    // }
}
