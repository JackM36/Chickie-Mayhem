using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    protected void Awake()
    {
    }

	// Use this for initialization
	protected virtual void Start () {
	}

	protected virtual void Update () {
	}

	protected void FixedUpdate (){
		Move ();
	}

	protected virtual void Move(){
	}
}

