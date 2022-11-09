using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // [Header("BGM")]
    // [SerializeField] AudioSource bgm_template;
    [Header("SFX")]
    [SerializeField] AudioSource sfx_GolfSwing;
    [SerializeField] AudioSource sfx_GolfCupDrop;
    [SerializeField] AudioSource sfx_Success;
    [SerializeField] AudioSource sfx_WallHit;
    [SerializeField] AudioSource sfx_BallOut;

    public void PlayGolfSwingSFX()
    {
        sfx_GolfSwing.Play();
    }

    public void PlayWinSFX()
    {
        sfx_GolfCupDrop.Play();
        sfx_Success.PlayDelayed(0.2f);
    }

    public void PlayHitWall()
    {
        sfx_WallHit.Play();
    }

    public void PlayBallOut()
    {
        sfx_BallOut.Play();
    }
}
