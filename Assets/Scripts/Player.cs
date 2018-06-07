using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    bool keyboardInput;
    public GameObject weaponObj;

    public float healthDecreasedOnEnemyContact = 100;

    public AudioClip[] taunts;
    public float tauntProbability = 0.5f;

    [HideInInspector]
    public int playerID = 1;

    [HideInInspector]
    public Weapon weapon;

	public float minTimeBetweenTaunts = 10;

    bool onEnemyContact = false;

	float lastTauntOnTime = 0;

    protected override void Update()
    {
        base.Update();

        GetInput();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(weapon != null && weapon.ammo <= 0)
        {
            Destroy(weapon.gameObject);
        }

        if (weapon != null)
        {
            bool shoot = false;
            if (weapon.continuousShooting)
            {
                shoot = shootPressed;
            }
            else
            {
                shoot = shootPressedDown;
            }

            if (shoot)
            {
                weapon.Shoot();
            }
            else
            {
                weapon.Stop();
            }
        }

        // Set anim vars
        anim.SetFloat("Speed", currentSpeed);

        // taunt
        float prob = Random.Range(0f, 100f);
        if (taunts.Length > 0 && (Time.time - lastTauntOnTime >= minTimeBetweenTaunts) && prob <= tauntProbability)
		{
            //AkSoundEngine.PostEvent ("Character_VO_Taunt", gameObject);
            int audioclipIndex = Random.Range(0, taunts.Length);
            SoundManager.PlaySound(taunts[audioclipIndex]);
			lastTauntOnTime = Time.time;
            
		}

    }

    void GetInput()
    {
        if (keyboardInput)
        {
            inputH = Input.GetAxisRaw("HorizontalP" + playerID);
            inputV = Input.GetAxisRaw("VerticalP" + playerID);

			if (inputH != 0 || inputV != 0) {

				AkSoundEngine.PostEvent ("Walking", gameObject);

			}

            inputR = Input.GetAxisRaw("LookHorizontalP" + playerID);
            inputF = Input.GetAxisRaw("LookVerticalP" + playerID);

			shootPressed = Input.GetButtonDown("JumpP" + playerID);
        }

        else
        {
            // Movement
            inputH = Input.GetAxisRaw("LeftJoystickHorizontalP" + playerID);
            inputV = -Input.GetAxisRaw("LeftJoystickVerticalP" + playerID);

			if (inputH != 0 || inputV != 0) {
			
				AkSoundEngine.PostEvent ("Walking", gameObject);

			}

            // Rotation
            inputF = Input.GetAxisRaw("RightJoystickVerticalP" + playerID);
            inputR = Input.GetAxisRaw("RightJoystickHorizontalP" + playerID);

            shootPressedDown = Input.GetAxisRaw("RightTriggerP" + playerID) > 0.1 ? true : false;// Input.GetKeyDown("joystick " + playerID + " button 7");
            shootPressed = Input.GetAxisRaw("RightTriggerP" + playerID) > 0.1 ? true : false; //Input.GetKey("joystick " + playerID + " button 7");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Get weapons
        if(other.tag == GameRepository.weaponTag)
        {
            // Destroy previous weapon
            if (weapon != null)
            {
                Destroy(weapon.gameObject);
            }

            // Set the weapon on player
            weaponObj = other.gameObject;
            weaponObj.transform.SetParent(transform, false);
            Rigidbody weaponRb = weaponObj.GetComponent<Rigidbody>();
            weaponRb.isKinematic = true;
            weaponRb.detectCollisions = false;
            weapon = weaponObj.GetComponent<Weapon>();
            weaponObj.transform.localPosition = weapon.posOnPlayer;

            weapon.SetHolder(this);

        }

        if (other.transform.tag == GameRepository.powerupTag)
        {
            other.transform.GetComponent<Powerup>().SetPlayer(transform);
        }

    }
		

	void OnCollisionEnter(Collision col)
    {
        //
		if (!onEnemyContact && col.gameObject.tag == GameRepository.enemyTag && (col.transform.GetComponent<Enemy>() == null || col.transform.GetComponent<Enemy>().isAlive))
        {
            DecreaseHealth(healthDecreasedOnEnemyContact, false);
            onEnemyContact = true;
        }
	}

    void OnCollisionExit(Collision col)
    {
        if (onEnemyContact && col.gameObject.tag == GameRepository.enemyTag && (col.transform.GetComponent<Enemy>() == null || col.transform.GetComponent<Enemy>().isAlive))
        {
            onEnemyContact = false;
        }
    }
}
