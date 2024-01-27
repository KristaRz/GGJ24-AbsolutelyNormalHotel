

using System.Collections;

using UnityEngine;

public class ElevatorManager : MonoBehaviour
{

    public static ElevatorManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        
        LeftDoorOpen = LeftDoor.transform;
        RightDoorOpen = RightDoor.transform;

        _buttons = GetComponentsInChildren<FloorButton>();
    }


    #region Floors

    private FloorButton[] _buttons;

    private int _selectedFloor = 0;

    public void SelectFloor(FloorButton floor)
    {
        _selectedFloor = floor.Index;
        foreach(FloorButton floorButton in _buttons)
        {
            if (floorButton != floor)
                floorButton.SelectFloor(false);
        }
        // coroutine for a few seconds until it starts (and door close?)
    }



    #endregion

    #region Doors

    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private Transform LeftDoorClosed;
    private Transform LeftDoorOpen;

    [SerializeField] private GameObject RightDoor;
    [SerializeField] private Transform RightDoorClosed;
    private Transform RightDoorOpen;

    [SerializeField] private float SlideTime = 1;
    [SerializeField] private float SlideDelay = 0;


    private float startTime;
    private Coroutine _doorSlideRoutine;

    public void SetDoorState(bool state)
    {
        if (LeftDoorOpen == null || RightDoor == null)
            return;

        if(_doorSlideRoutine != null)
            StopCoroutine(_doorSlideRoutine);

        startTime = Time.time;
        
        if (state)
            _doorSlideRoutine = StartCoroutine(SlideDoor(LeftDoorOpen, RightDoorOpen, SlideDelay));
        else
            _doorSlideRoutine = StartCoroutine(SlideDoor(LeftDoorClosed, RightDoorClosed, SlideDelay));
    }

    public void SetDoorDelay(float delay) => SlideDelay = delay;

    private float _timePassed;

    private Vector3 _leftStart;
    private Vector3 _rightStart;
    private float _lDistance;
    private float _rDistance;

    private IEnumerator SlideDoor(Transform leftTarget, Transform rightTarget, float delay)
    {
        yield return new WaitForSeconds(delay);

        _leftStart = LeftDoor.transform.position;
        _rightStart = RightDoor.transform.position;

        _lDistance = Vector3.Distance(_leftStart, leftTarget.position);
        _rDistance = Vector3.Distance(_rightStart, rightTarget.position);

        
        _timePassed = 0;

        while (_timePassed <= SlideTime)
        {
            // Distance moved equals elapsed time times speed..
            _timePassed = (Time.time - startTime) / SlideTime;

            // Fraction of journey completed equals current distance divided by total distance.
            float lFractionOfJourney = _timePassed / _lDistance;
            float rFractionOfJourney = _timePassed / _rDistance;

            // Set our position as a fraction of the distance between the markers.
            LeftDoor.transform.position = Vector3.Lerp(_leftStart, leftTarget.position, lFractionOfJourney);
            RightDoor.transform.position = Vector3.Lerp(_rightStart, rightTarget.position, rFractionOfJourney);
            yield return null;
        }

        LeftDoor.transform.position = leftTarget.position;
        RightDoor.transform.position = rightTarget.position;

        _doorSlideRoutine = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion
}
