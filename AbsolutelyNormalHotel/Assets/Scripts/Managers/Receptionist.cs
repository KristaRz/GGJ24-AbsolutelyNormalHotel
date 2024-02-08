
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptionist : MonoBehaviour
{
    [SerializeField] List<AudioClip> voiceLines = new List<AudioClip>();
    [SerializeField] float delayBetween = 0f;
    public Transform[] Eyes;
    private Transform _playerFollow;
    private Animator _animator;

    private void Start()
    {
        // _animator = GetComponent<Animator>();
       // _playerFollow = Camera.main.transform;
    }
    /*
    private void Update()
    {
        if (_playerFollow == null) return;

        foreach(var eye in Eyes)
        {
            eye.LookAt(_playerFollow.position, transform.up);
            //eye.transform.localRotation.SetLookRotation(_playerFollow.position);
        }
    }*/

    private int _voiceIndex = 0;

    public void BellRung()
    {
        PlayNextVoiceLine();
        //Invoke("PlayNextVoiceLine", 0.5f);
    }

    public void PlayNextVoiceLine()
    {
        if (voiceLines.Count == 0) return;
        if (_voiceIndex >= voiceLines.Count) return;

        StartCoroutine(PlayOneVoiceLine(voiceLines[_voiceIndex]));
    }

    private IEnumerator PlayOneVoiceLine(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySound(audioClip);
        //_animator.SetTrigger("Talk");

        _voiceIndex++;
        yield return new WaitForSeconds(audioClip.length);

        if(_voiceIndex < voiceLines.Count) PlayNextVoiceLine();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
