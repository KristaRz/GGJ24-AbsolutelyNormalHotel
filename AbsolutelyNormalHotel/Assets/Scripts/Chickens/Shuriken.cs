using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Shuriken : MonoBehaviour
{
    private Vector3 lastHitPos;                 // position of last Hit
    private AudioSource[] audioData;            // audio sources on this object to play on hit
    private Rigidbody rb;                       // rigidbody of this object
    private bool justGotHit = false;            // to indicate that we recently hit an object

    public GameObject fxHitPrefab;              // prefab of a particle effect to spawn on hit
    public bool isHitDistanceEnough = true;     // checks if you swung hard enough
    public ParticleSystem sparks;               // particles to be spawned on hit
    public float minvelocity = 5;               // minimum velocity (speed) we need to trigger a hit
    public float mindistancefromlasthit = 0.5f; // minimum distance we have to be away from last hit to hit again
    public XRController rightHandController;    // access the right hand controller

    [SerializeField] private InputDeviceCharacteristics controllerToUse;

    private InputDevice _rightHand;

    [SerializeField] private float TriggerStartThreshold = 0.6f;
    [SerializeField] private float TriggerReleaseThreshold = 0.1f;

    private void Start()
    {
        audioData = GetComponents<AudioSource>();  // get the audio sources of this object (hit sounds)
        rb = GetComponent<Rigidbody>();                 // get the rigidbody of this object

        TryInitializeController();
    }

    // Gets the controller from InputDecives //
    private void TryInitializeController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerToUse, devices);
        if (devices.Count > 0)
            _rightHand = devices[0];
    }

    private bool _checkVelocity = true;

    public async void SetVelocityCheck(bool state)
    {
        if (!_checkVelocity && state)
            await CheckForVelocity();

        _checkVelocity = state;
    }


    private async Task CheckForVelocity()
    {
        while (_checkVelocity)
        {
            await Task.Yield();
        }
    }

    // [KP] Checks if your swinging distance is far enough and stores it in isHitDistanceEnough //
    void Update()
    {
        if (!isHitDistanceEnough)           // if we recently hit, this will be false
        {
            if (Vector3.Distance(lastHitPos, sparks.gameObject.transform.position) > mindistancefromlasthit)            // check if the last position is far enough from your current position then set it true again
            {
                isHitDistanceEnough = true;
            }
        }
    }

}
