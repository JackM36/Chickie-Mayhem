using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHealthUp : Powerup {
	Player playerComponent;
    public float healthIncrease = 100;

	// Use this for initialization
	void Start () {
		// dummy endTime
		endTime = 0.1f;
	}

	protected override void Power ()
	{
		playerComponent = upgradedPlayer.GetComponent<Player> ();
		playerComponent.IncreaseHealth (healthIncrease);
		Destroy (gameObject);
	}

	protected override void StopPower ()
	{
        if (playerComponent == null)
        {
            return;
        }

        base.StopPower ();
	}
}
