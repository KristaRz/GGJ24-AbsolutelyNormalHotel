

using System.Collections;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager Instance { get; private set; }

    [SerializeField] private float LoadNewLevelDelay = 0.5f;

    void Awake()
    {
        Instance = this;
        
        LeftDoorOpen = LeftDoor.transform.position;
        LeftDoorClosed = LeftDoorTarget.position;
        RightDoorOpen = RightDoor.transform.position;
        RightDoorClosed = RightDoorTarget.position;

        _buttons = GetComponentsInChildren<FloorButton>();
        if (!startStateOfDoors)
            SlideDoorInstant(false);
    }

    public void StartThisFloor()
    {
        GameManager.Instance.LevelStart((int)SlideDelay);
    }

    #region Floors

    private FloorButton[] _buttons;

    private int _selectedFloor = 0;


    public void SelectFloor(FloorButton floor)
    {
        _selectedFloor = floor.Index;
        GameManager.Instance.SetLevel(_selectedFloor);

        foreach(FloorButton floorButton in _buttons)
        {
            if (floorButton != floor)
                floorButton.SelectFloor(false);
        }
        StartCoroutine(WaitUntilSceneLoad());
    }


    private IEnumerator WaitUntilSceneLoad()
    {
        if(_doorStateCurrent) SetDoorState(false);

        yield return new WaitUntil(() => _doorSlideRoutine == null);
        yield return new WaitForSeconds(LoadNewLevelDelay);
        SceneHandler.Instance.LoadSceneNumber(_selectedFloor);
    }

    #endregion

    #region Doors
    [Tooltip("False = closed, True = Open")]
    [SerializeField] private bool startStateOfDoors = false;
    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private Transform LeftDoorTarget;
    private Vector3 LeftDoorClosed;
    private Vector3 LeftDoorOpen;

    [SerializeField] private GameObject RightDoor;
    [SerializeField] private Transform RightDoorTarget;
    private Vector3 RightDoorClosed;
    private Vector3 RightDoorOpen;

    [SerializeField] private float SlideTime = 1;
    [SerializeField] private float SlideDelay = 0;

    private bool _doorStateCurrent = true;
    private float startTime;
    private Coroutine _doorSlideRoutine;

    public void SetDoorState(bool state)
    {

        if (LeftDoorOpen == null || RightDoor == null)
            return;

        if (_doorSlideRoutine != null)
            return;

        startTime = Time.time;
        if (state)
        {
            _doorSlideRoutine = StartCoroutine(SlideDoor(LeftDoorOpen, RightDoorOpen, SlideDelay));
            _doorStateCurrent = true;
        }
        else
        {
            _doorSlideRoutine = StartCoroutine(SlideDoor(LeftDoorClosed, RightDoorClosed, SlideDelay));
            _doorStateCurrent = false;
        }
    }

    public void SetDoorDelay(float delay) => SlideDelay = delay;

    private float _timePassed;

    private Vector3 _leftStart;
    private Vector3 _rightStart;
    private float _lDistance;
    private float _rDistance;

    private IEnumerator SlideDoor(Vector3 leftTarget, Vector3 rightTarget, float delay)
    {
        yield return new WaitForSeconds(delay);

        _leftStart = LeftDoor.transform.position;
        _rightStart = RightDoor.transform.position;

        _lDistance = Vector3.Distance(_leftStart, leftTarget);
        _rDistance = Vector3.Distance(_rightStart, rightTarget);
 
        
        _timePassed = 0;

        while (_timePassed <= SlideTime-0.8)
        {
            // Distance moved equals elapsed time times speed..
            _timePassed = (Time.time - startTime) / SlideTime;

            // Fraction of journey completed equals current distance divided by total distance.
            float lFractionOfJourney = _timePassed / _lDistance;
            float rFractionOfJourney = _timePassed / _rDistance;

            // Set our position as a fraction of the distance between the markers.
            LeftDoor.transform.position = Vector3.Lerp(_leftStart, leftTarget, lFractionOfJourney);
            RightDoor.transform.position = Vector3.Lerp(_rightStart, rightTarget, rFractionOfJourney);
            yield return null;
        }
        LeftDoor.transform.position = leftTarget;
        RightDoor.transform.position = rightTarget;

        _doorSlideRoutine = null;
    }

    private void SlideDoorInstant(bool state)
    {
        if (state)
        {
            LeftDoor.transform.position = LeftDoorOpen;
            RightDoor.transform.position = RightDoorOpen;
            _doorStateCurrent = true;
        }
        else
        {
            LeftDoor.transform.position = LeftDoorClosed;
            RightDoor.transform.position = RightDoorClosed;
            _doorStateCurrent = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion
}
