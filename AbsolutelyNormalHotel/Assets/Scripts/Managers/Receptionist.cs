
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptionist : MonoBehaviour
{
    [SerializeField] List<AudioClip> voiceLines = new List<AudioClip>();
    [SerializeField] float delayBetween = 0f;

    private Animator _animator;

    private void Start()
    {
        // _animator = GetComponent<Animator>();
    }

    private int _voiceIndex = 0;

    public void BellRung()
    {
        PlayNextVoiceLine();
        //Invoke("PlayNextVoiceLine", 0.5f);
    }

    public void PlayNextVoiceLine()
    {
        if (voiceLines.Count == 0) return;

        StartCoroutine(PlayOneVoiceLine(voiceLines[_voiceIndex]));
    }

    private IEnumerator PlayOneVoiceLine(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySound(audioClip);
        //_animator.SetTrigger("Talk");

        _voiceIndex++;
        yield return new WaitForSeconds(audioClip.length);

        if(_voiceIndex <= voiceLines.Count) PlayNextVoiceLine();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
