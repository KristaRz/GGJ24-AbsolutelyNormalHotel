
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public int Index;
    [SerializeField] GameObject ButtonMesh;
    [SerializeField] private float PressDepth;


    private Transform UnpressedPosition;
    private Vector3 PressedPosition;
    private bool pressedState = false;

    private void Start()
    {
        UnpressedPosition = ButtonMesh.transform;
        //PressedPosition = new Vector3(UnpressedPosition.position.x, UnpressedPosition.position.y, UnpressedPosition.position.z - PressDepth);
    }

    public void ToggleButton()
    {
        pressedState = !pressedState;
        if (pressedState)
        {
            SelectFloor(true);
            ElevatorManager.Instance.SelectFloor(this);
        }
    }

    public void SelectFloor(bool state)
    {
        if (state)
            ButtonMesh.transform.Translate(new Vector3(0, 0, PressDepth), Space.Self);
        else
            ButtonMesh.transform.position = UnpressedPosition.position;
        

        pressedState = state;
    }
}
