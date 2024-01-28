
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections;

public class TimerHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent On15secondsLeft;
    [SerializeField] private TextMeshProUGUI TimeLeftDisplay;

    private int _levelTime = 60;

    private void Start()
    {
        GameManager.Instance.OnLevelStart.AddListener(StartTimer);
    }

    private float _timePassed = 0;
    private float startTime = 0;

    private void StartTimer(int levelTime)
    {
        countTimeActive = true;
        _levelTime = levelTime;
        startTime = Time.time;
        StartCoroutine(CountTime());
    }

    private bool countTimeActive = false;

    // Update is called once per frame
    private IEnumerator CountTime()
    {
        while (countTimeActive)
        {
            _timePassed = Time.time - startTime;

            TimeLeftDisplay.SetText((_levelTime - (int)_timePassed).ToString());
            Debug.Log(_timePassed.ToString());
            if (_levelTime - _timePassed <= 0)
            {
                GameManager.Instance.FinishLevel();             
                countTimeActive = false;
                this.enabled = false;
            }
            else if (_levelTime - _timePassed <= 15)
            {
                On15secondsLeft.Invoke();
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        countTimeActive = false;
    }
}
