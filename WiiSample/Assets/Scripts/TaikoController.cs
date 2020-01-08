using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoController : SingletonBehaviour<TaikoController>
{
    [SerializeField] GameObject taikoObj;
    // 移動量がこれ以上ならば太鼓の音がならない
    [SerializeField] float playSEDistanceThreasold = 1f;
    [SerializeField] float playSESpanSecond = 0.2f;
    [SerializeField] int audioSourceLayerCount = 5;

    private Animator taikoAnimator;
    private Rigidbody taikoRigidbody;
    private Vector3 prevPosition;
    private Camera mainCamera;
    private float timeCounter = 0f;
    private bool isPlayLoop = false;
    private List<AudioSource> seAudioList = new List<AudioSource>();
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Start()
    {
        defaultPosition = taikoObj.transform.position;
        defaultRotation = taikoObj.transform.rotation;
        mainCamera = Camera.main;
        taikoAnimator = taikoObj.GetComponent<Animator>();
        taikoRigidbody = taikoObj.GetComponent<Rigidbody>();
        prevPosition = taikoObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = taikoObj.transform.position - prevPosition;
        mainCamera.transform.position = mainCamera.transform.position + diff;
        mainCamera.transform.LookAt(taikoObj.transform.position);
        if (diff.magnitude > playSEDistanceThreasold)
        {
            if (!isPlayLoop)
            {
                isPlayLoop = true;
                StopCoroutine(LoopPlaySE());
                StartCoroutine(LoopPlaySE());
            }
        }
        else
        {
            isPlayLoop = false;
        }
        prevPosition = taikoObj.transform.position;
    }

    private IEnumerator LoopPlaySE()
    {
        while (isPlayLoop)
        {
            if (seAudioList.Count >= audioSourceLayerCount)
            {
                AudioSource popAudio = seAudioList[0];
                if (popAudio != null && popAudio.isPlaying)
                {
                    popAudio.Stop();
                    StopCoroutine(SoundManager.Instance.SEAudioPlayCorutine(popAudio));
                    Destroy(popAudio.gameObject);
                    seAudioList.RemoveAt(0);
                }
            }
            AudioSource seAudioSource = SoundManager.Instance.PlaySE("Drum_level3");
            if (seAudioSource != null)
            {
                seAudioList.Add(seAudioSource);
            }
            yield return new WaitForSeconds(playSESpanSecond);
        }
    }

    public void GoForward(float progress)
    {
        taikoAnimator.SetTrigger("Forward");
        taikoRigidbody.velocity = taikoObj.transform.forward * progress * 5;
    }

    public void MoveBack(float progress)
    {
        taikoAnimator.SetTrigger("Back");
        taikoRigidbody.velocity = -(taikoObj.transform.forward * progress * 5);
    }

    public void StopTaiko()
    {
        taikoAnimator.SetTrigger("Idle");
    }

    public void ResetTaiko()
    {
        taikoObj.transform.position = defaultPosition;
        taikoObj.transform.rotation = defaultRotation;
    }
}
