using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
	protected float endTime;
	float timer;
	protected bool pickedUp;
	protected Transform upgradedPlayer;
	bool firstTime;
    public float speedModifier = 5f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (pickedUp) {
			if (firstTime == true) {
				Power ();
				GetComponent<MeshRenderer> ().enabled = false;
				GetComponent<BoxCollider> ().enabled = false;
				firstTime = false;
			}

			if (timer >= endTime) {
				StopPower ();
				Destroy (gameObject);
			}

			timer += Time.deltaTime;
		}
	}

	protected virtual void Power(){
		
	}

	protected virtual void StopPower(){
		
	}

	public void SetPlayer(Transform player){
		upgradedPlayer = player;
		pickedUp = true;
		firstTime = true;
	}
		
}
