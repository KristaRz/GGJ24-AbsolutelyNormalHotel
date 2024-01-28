using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float scoreCountSpeed = 1.0f;

    [SerializeField] private UnityEvent OnFinishCountedScore;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnFinishLevelWithScore.AddListener(ShowScore);
    }

    private float _timePassed = 0;
    private float startTime = 0;

    private void ShowScore(int score)
    {
        startTime = Time.time;
        StartCoroutine(CountUpScore(score));
    }


    private IEnumerator CountUpScore(int score)
    {
        _timePassed = 0;

        while (_timePassed <= scoreCountSpeed)
        {
            _timePassed = (Time.time - startTime) / scoreCountSpeed;

            int scoreFraction = (int)Mathf.Lerp(score, 0, _timePassed / score);
            scoreText.SetText(scoreFraction.ToString());

            yield return null;
        }
        scoreText.SetText(score.ToString());
        OnFinishCountedScore.Invoke();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnFinishLevelWithScore.RemoveListener(ShowScore);
    }
}
