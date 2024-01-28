
using UnityEngine;

public class Katana : MonoBehaviour
{
    private Vector3 lastHitPos;                 // position of last Hit
    private AudioSource[] audioData;            // audio sources on this object to play on hit
    private Rigidbody rb;                       // rigidbody of this object
    private bool justGotHit = false;            // to indicate that we recently hit an object

    ///public GameObject fxHitPrefab;              // prefab of a particle effect to spawn on hit
    public bool isHitDistanceEnough = true;     // checks if you swung hard enough
    public Vector3 lastHit;               // particles to be spawned on hit
    public float minvelocity = 5;               // minimum velocity (speed) we need to trigger a hit
    public float mindistancefromlasthit = 0.5f; // minimum distance we have to be away from last hit to hit again
    //public XRController rightHandController;    // access the right hand controller


    private void Start()
    {
        audioData = GetComponents<AudioSource>();  // get the audio sources of this object (hit sounds)
        rb = GetComponent<Rigidbody>();                 // get the rigidbody of this object
    }

    // [KP] Checks if your swinging distance is far enough and stores it in isHitDistanceEnough //
    void Update()
    {
        if (!isHitDistanceEnough)           // if we recently hit, this will be false
        {
            if (Vector3.Distance(lastHitPos, lastHit) > mindistancefromlasthit)            // check if the last position is far enough from your current position then set it true again
            {
                Debug.Log("Distance enough");
                isHitDistanceEnough = true;
            }
        }
    }

    // [KP, SK] Checks if the axe has collided with a stone and if it was strong and fast enough initiates reaction //
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)                                       // 'Destroyable by pickaxe' layer
        {

            if (isHitDistanceEnough && rb.velocity.magnitude >= minvelocity)      // only if we have enough distance we register the collision
            {
                Debug.Log("Trying to hit");
                if (!justGotHit)        // only execture if you didn't just get hit
                {
                    Debug.Log("Hit");
                    justGotHit = true;
                    Invoke("delayjustGotHitFalse", 0.5f);       // sets the justGotHit boolean false again after a bit
                    hit();                                      // hit reactions
                    lastHit = other.transform.position;
                    other.gameObject.GetComponent<ChickenGame>().DestroySelf();                       // hit reactions in the stone
                }
            }
            lastHitPos = lastHit;  // get the position of the hit for the particles
            isHitDistanceEnough = false;                        // reset the checking for the distance
        }
    }

    // [KP] Responsible for playing particle, sound and vibration. //
    public void hit()
    {
        /*
        if (fxHitPrefab && audioData != null)       // if the particles and audio aren't active yet
        {
            sparks.Play();                          // start the particle play

            int randomNumber = Random.Range(0, 1);      // using a random number to mix it up between 2 sounds
            audioData[randomNumber].Play(0);            // play the sound that is attached to this object

            if (rightHandController.inputDevice.TryGetHapticCapabilities(out HapticCapabilities capabilities))      // check if the controller can do vibration
            {
                if (capabilities.supportsImpulse)       // if it does, send a vibration
                {
                    uint channel = 0;
                    float amplitude = 0.3f;
                    float duration = 0.1f;
                    rightHandController.inputDevice.SendHapticImpulse(channel, amplitude, duration);    // sending the vibration impulse using the before variables
                }
            }

        }
        */
    }

    // [KP] Sets the boolean false after a delay to avoid errors. //
    private void delayjustGotHitFalse()
    {
        justGotHit = false;
    }
}
