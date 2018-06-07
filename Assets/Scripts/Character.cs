using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName = "Character";
    public Sprite characterImg;
    [HideInInspector] public bool isAlive = true;
    public int characterID = 0;
	[SerializeField] public float speed = 20;
    [SerializeField]
    float maxHealth = 100;
    public float maxSpeed;

    public float destroyDelay = 0f;

    public float Health { get; protected set; }

    // Input vars
	// Movement
    protected float inputH = 0;
    protected float inputV = 0;

	// Rotation
	protected float inputF = 0;
	protected float inputR = 0;
    protected bool shootPressedDown = false;
    protected bool shootPressed = false;

    protected Rigidbody rb;
    protected Animator anim;
    protected float currentSpeed;

    Vector3 relativeForward;
	Vector3 relativeRight;
    Vector3 lastPos;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Health = maxHealth;

        lastPos = transform.position;
    }

	protected void Start ()
    {
	}

	protected virtual void Update ()
    {
        
    }

	protected virtual void FixedUpdate ()
    {
        // Set forward and right vectors relative to camera
        relativeForward = Camera.main.transform.forward;
        relativeForward.y = 0;
        relativeForward = Vector3.Normalize(relativeForward);
        relativeRight = Quaternion.Euler(new Vector3(0, 90, 0)) * relativeForward;

        //currentSpeed = Mathf.Clamp01(Mathf.Sqrt((inputV * inputV) + (inputH * inputH)));

        if (isAlive)
        {
            Move();
            Look();
        }

        // Calculate speed
        currentSpeed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

	protected virtual void Move()
    {
        // Calculate direction based on input
        Vector3 direction = new Vector3(inputH, 0, inputV);

        if (direction.magnitude > 0.1f)
        {
            // Calculate heading vector (camera dependent)
            Vector3 rightMovement = relativeRight * Time.deltaTime * inputH;
            Vector3 upMovement = relativeForward * Time.deltaTime * inputV;
            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            // Move
            transform.position += heading * speed * Time.deltaTime;
        }
	}

	void Look()
	{
		// Calculate direction based on input
		Vector3 direction = new Vector3(inputR, 0, inputF);

		if (direction.magnitude > 0.1f)
		{
			// Calculate heading vector (camera dependent)
			Vector3 rightMovement = relativeRight * Time.deltaTime * inputR;
			Vector3 upMovement = relativeForward * Time.deltaTime * inputF;
			Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

			// Rotate towards heading
			Quaternion turnRotation = Quaternion.LookRotation(heading, Vector3.up);
			transform.rotation = turnRotation;
		}
    }

    public void IncreaseHealth(float value)
    {
        Health = Mathf.Clamp(Health + value, 0, maxHealth);
    }

    public void DecreaseHealth(float value, bool burn)
    {
        Health = Mathf.Clamp(Health - value, 0, maxHealth);
        if(Health == 0)
        {
            if(burn)
            {
                StartCoroutine(BurnDie());
            }
            else
            {
                StartCoroutine(Die());
            }
        }
    }

    protected IEnumerator BurnDie()
    {
        isAlive = false;

        yield return new WaitForSeconds(destroyDelay);

        Transform[] bodyParts = GetComponentsInChildren<Transform>();
        foreach (Transform part in bodyParts)
        {
            part.SetParent(null);
            part.gameObject.AddComponent<Rigidbody>();
        }

        rb.detectCollisions = false;
        rb.isKinematic = true;

        AkSoundEngine.PostEvent("Character_Die", gameObject);

        foreach (Transform part in bodyParts)
        {
            Destroy(part.gameObject);
        }
    }

    protected IEnumerator Die()
    {
        isAlive = false;

        Transform[] bodyParts = GetComponentsInChildren<Transform>();
        foreach (Transform part in bodyParts)
        {
            part.SetParent(null);
            part.gameObject.AddComponent<Rigidbody>();
        }

        yield return new WaitForSeconds(destroyDelay);

        rb.detectCollisions = false;
        rb.isKinematic = true;

		AkSoundEngine.PostEvent ("Character_Die", gameObject);

        foreach (Transform part in bodyParts)
        {
            Destroy(part.gameObject);
        }
    }
}

