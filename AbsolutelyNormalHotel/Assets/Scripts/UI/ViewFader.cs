using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewFader : MonoBehaviour
{

    public UnityEvent OnScreenOff;

    [SerializeField] private Color FullBlack;
    [SerializeField] private Color FadedBlack;

    [SerializeField] private float FadeSpeed;

    private Material _blackMat;
    private void Start()
    {
        _blackMat = GetComponent<Renderer>().material;
    }

    private bool blinkScreen = false;
    public void BlinkScreen()
    {
        blinkScreen = true;
        FadeScreenState(false);
    }

    private void FadeScreenState(bool state)
    {
        if (_viewFadeRoutine != null)
            return;

        if(!state) _viewFadeRoutine = StartCoroutine(FadeScreen(false, 0, 1f));
        else _viewFadeRoutine = StartCoroutine(FadeScreen(true, 0, 0f));
    }

    private Coroutine _viewFadeRoutine;
   
    private IEnumerator FadeScreen(bool targetState, float startDelay, float endDelay)
    {
        yield return new WaitForSeconds(startDelay);

        float timePassed = 0;

        while (timePassed < FadeSpeed)
        {
            if (targetState)
                _blackMat.color = new Color(_blackMat.color.r, _blackMat.color.g, _blackMat.color.b,
                Mathf.Lerp(0, 1f, timePassed / FadeSpeed));
            else
                _blackMat.color = new Color(_blackMat.color.r, _blackMat.color.g, _blackMat.color.b,
                Mathf.Lerp(1, 0f, timePassed / FadeSpeed));

            timePassed += Time.deltaTime;

            yield return null;
        }

        if (!targetState)
        {
            _blackMat.color = FullBlack;
            OnScreenOff.Invoke();
        }
        else
            _blackMat.color = FadedBlack;

        _viewFadeRoutine = null;

        if (blinkScreen)
        {
            yield return new WaitForSeconds(endDelay);

            blinkScreen = false;
            FadeScreenState(true);
        }
    }
}
