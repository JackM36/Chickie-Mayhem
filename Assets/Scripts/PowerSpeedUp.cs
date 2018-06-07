using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpeedUp : Powerup {
	Player playerComponent;

	// Use this for initialization
	void Start () {
		endTime = 10;

		AkSoundEngine.PostEvent ("PowerSpeedUp", gameObject);

	}

	protected override void Power ()
	{
		playerComponent = upgradedPlayer.GetComponent<Player> ();
        playerComponent.speed = Mathf.Clamp(playerComponent.speed * speedModifier, 0, upgradedPlayer.GetComponent<Player>().maxSpeed);

        base.Power ();
	}

	protected override void StopPower ()
	{
        if (playerComponent == null)
        {
            return;
        }

        playerComponent.speed = Mathf.Clamp(playerComponent.speed / speedModifier, 0, upgradedPlayer.GetComponent<Player>().maxSpeed);
        base.StopPower ();
	}
}
