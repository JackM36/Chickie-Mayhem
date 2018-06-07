using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

    // character variables=
    List<GameObject> characterList;
    int index;
    public int playerID;

    // canvas elements
    public Text characterName;
    public Image joystickButton;
    public Text indicatorText;
    public Image leftArrows;
    public Image rightArrows;

    // variables for joystick input
    protected bool leftDpadPress, rightDpadPress;
    protected bool selected, deselected;
    bool pressed;
    bool active = false;

    UIManager manager;

	// Use this for initialization
	void Start()
    {
        pressed = false;
        selected = false;
        deselected = true;
        manager = GameObject.Find("UIManager").GetComponent<UIManager>();
        characterList = new List<GameObject>();
        foreach (Player c in manager.playerCharacters)
        {
            // spawn character on main screen
            GameObject character = Instantiate(c.gameObject);
            character.GetComponent<Character>().enabled = false;
            character.transform.SetParent(this.transform);
            character.transform.position = this.transform.localPosition;
            character.GetComponent<Rigidbody>().useGravity = false;

            characterList.Add(character);
            character.SetActive(false);
            //characterList[index].SetActive(true);
            characterName.text = characterList[index].GetComponent<Character>().characterName;
        }
	}

    void Update()
    {
        GetInput();

        if (active)
        {
            if (!pressed)
            {
                if (leftDpadPress)
                {
                    Previous();
                    pressed = true;
                }
                else if (rightDpadPress)
                {
                    Next();
                    pressed = true;
                }
            }

            if (selected)
            {
                indicatorText.text = "Ready";
                joystickButton.enabled = false;
                if (playerID == 1)
                {
                    manager.selectedP1 = true;
                }
                else if (playerID == 2)
                {
                    manager.selectedP2 = true;
                }
            }

            if (deselected)
            {
                indicatorText.text = "Press      to Select";
                joystickButton.enabled = true;
                if (playerID == 1)
                {
                    manager.selectedP1 = false;
                }
                else if (playerID == 2)
                {
                    manager.selectedP2 = false;
                }
            }

            // rotate object around its y-axis
            this.gameObject.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
        }
        else
        {
            if (selected)
            {
                indicatorText.text = "Press      to Select";
                joystickButton.enabled = true;
                leftArrows.enabled = true;
                rightArrows.enabled = true;
                SetActive(true);
                if (playerID == 1)
                {
                    manager.selectedP1 = false;
                    manager.characterIDP1 = characterList[index].GetComponent<Player>().characterID;
                }
                else if (playerID == 2)
                {
                    manager.selectedP2 = false;
                    manager.characterIDP2 = characterList[index].GetComponent<Player>().characterID;
                }
                manager.playersJoined = true;
            }
        }
    }

    void SetActive(bool active)
    {
        this.active = active;
        characterList[index].SetActive(active);

    }

    void GetInput()
    {
        leftDpadPress = Input.GetAxisRaw("DPadXP" + playerID) == -1 ? true : false;
        rightDpadPress = Input.GetAxisRaw("DPadXP" + playerID) == 1 ? true : false;
        selected = Input.GetKeyDown("joystick " + playerID + " button 0");
        deselected = Input.GetKeyDown("joystick " + playerID + " button 1");

        if (!leftDpadPress && !rightDpadPress)
        {
            pressed = false;
        }
    }

    void Next()
    {
        characterList[index].SetActive(false);
        if (index == characterList.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        characterList[index].SetActive(true);
        if (playerID == 1)
        {
            manager.characterIDP1 = characterList[index].GetComponent<Player>().characterID;
        }
        else if (playerID == 2)
        {
            manager.characterIDP2 = characterList[index].GetComponent<Player>().characterID;
        }
        characterName.text = characterList[index].GetComponent<Character>().characterName;
    }

    void Previous()
    {
        characterList[index].SetActive(false);
        if (index == 0)
        {
            index = characterList.Count - 1;
        }
        else
        {
            index--;
        }
        characterList[index].SetActive(true);
        if (playerID == 1)
        {
            manager.characterIDP1 = characterList[index].GetComponent<Player>().characterID;
        }
        else if (playerID == 2)
        {
            manager.characterIDP2 = characterList[index].GetComponent<Player>().characterID;
        }
        characterName.text = characterList[index].GetComponent<Character>().characterName;
    }

}
