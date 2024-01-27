
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public int Index;
    [SerializeField] private float PressDepth;
    private Transform UnpressedPosition;
    private Vector3 PressedPosition;

    private void Start()
    {
        UnpressedPosition = transform;
        PressedPosition = new Vector3(UnpressedPosition.position.x, UnpressedPosition.position.y, UnpressedPosition.position.z - PressDepth);
    }

    public void SelectFloor(bool state)
    {
        if (state)
        {
            transform.position = PressedPosition;
        }
        else
            transform.Translate(new Vector3(0,0, PressDepth), Space.Self);
    }
}
