using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.Networking;

using System;

public class PlayerController : NetworkBehaviour
{
    // Create public variables for player speed, and for the Text UI game objects
    public float speed;
    public AudioSource pickUpSound;
    public Text winText;
    public ParticleSystem particle;
    public Text playerAndScore; 
    public GameObject player;
    public GUISkin myskin;

    //Data send to server
    [SyncVar]
    private string playerName = "myname";
    //Growth factor for the ball
    private float factor = (float) 1.1;
    private float density = 1;
    //private Stopwatch chrono;
    // Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
    private Vector3 offset;
    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;
    [SyncVar]
    private int score;

    void OnGUI()
    {   
        if (!isLocalPlayer)
        { return; }
        GUI.skin = myskin;
        UpdateNameScore();
    }

    void UpdateNameScore()
    {
        playerName = GUI.TextField(new Rect(Screen.width - Screen.width / 5, 10, Screen.width / 5, Screen.height / 10), playerName);
        playerAndScore.text = playerName + ": " + score.ToString();
    }

    // At the start of the game..
    void Start()
    {
        if (!isLocalPlayer)
        { return; }
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();
        // Set the score to zero 
        score = 0;
        // Set the position of the camera above the player
        offset = Camera.main.transform.position;
        //chrono.Start();
        // Calculate density of the sphere according to its starting mass and size
        float sphereVolume = (4 * (float)Math.PI * (float)Math.Pow(gameObject.transform.lossyScale.x, 3)) / 3;
        density = rb.mass / sphereVolume;
    }

    // Each physics step..
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        { return; }
        // Set some local float variables equal to the value of our Horizontal and Vertical Inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //If mobile device, use accelerometer instead of arrows on keyboard
        if (Application.isMobilePlatform)
        {
            moveHorizontal = Input.acceleration.x*3;
            moveVertical = Input.acceleration.y*3;
        }

        //Update the camera's position
        Camera.main.transform.position = this.player.transform.position + offset;

        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        Vector3 movement = new Vector3(moveHorizontal * factor, 0.0f, moveVertical * factor);

        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        rb.AddForce(movement * speed * rb.mass);
    }

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        { return; }
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Run the 'SetCountText()' function (see below)
            EventPickUp(other.gameObject);
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void EventPickUp(GameObject pickUp)
    {
        // Make the other game object (the pick up) inactive, to make it disappear
        pickUp.SetActive(false);
        UpdateSizeAndMass();

        PickUpAnimation();
        // Add one to the score variable 'score'
        score = score + 1;
        // Update the text field of our 'countText' variable
        offset = new Vector3(offset.x, offset.y + 1* factor, offset.z -1 * factor);
        // Check if our 'count' is equal to or exceeded 12
        if (score >= 12)
        {
            // Set the text value of our 'winText'
            winText.text = "You Win!";
            //chrono.Stop();
            //timeText.text = "Time: " + chrono.Elapsed.ToString();
        }
    }

    void PickUpAnimation ()
    {
        pickUpSound.Play();
        particle.Play();
    }

    // Function where the growth of the sphere is managed
    void UpdateSizeAndMass ()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.lossyScale.x * factor, gameObject.transform.lossyScale.y * factor, gameObject.transform.lossyScale.z * factor);
        float sphereVolume = (4 * (float)Math.PI * (float)Math.Pow(gameObject.transform.lossyScale.x, 3)) / 3;
        rb.mass = density * sphereVolume;
    }
}