using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{

	public GameObject[] PartyMembers;
	public Sprite[] OrderSprites;
	private bool noRepeat = true;
	private static int partySize = 4;
	private string[] PartyMemberOrder = new string[partySize];
	private int[] OrderNumbers = new int[partySize];
	private int[] PartyColours = new int[partySize];

	enum Colours {
		Red, Yellow, Blue, Grey,
		Orange, Green, Purple, 
		Black, White,
		Pink, Brown, Lime
	}

	Colours robotColour;

    // Start is called before the first frame update
    void Start()
    {
     
    	robotColour = Colours.Red;

    	PartyColours[0] = Random.Range(0, 12);
    	PartyColours[1] = Random.Range(0, 12);
    	PartyColours[2] = Random.Range(0, 12);
    	PartyColours[3] = Random.Range(0, 12);

    	for(int i = 0; i < partySize; i++)
		{

			//SETTING AND OUTPUT OF ROBOT COLOUR
			robotColour = (Colours)PartyColours[i];
			print(robotColour);

			switch(robotColour)
			{
			case Colours.Red:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 0, 0, 1);
				break;
			case Colours.Yellow:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 1, 0, 1);
				break;
			case Colours.Blue:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0, 0, 1, 1);
				break;
			case Colours.Grey:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0.5f, 0.5f, 0.5f, 1);
				break;
			case Colours.Orange:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 0.5f, 0, 1);
				break;
			case Colours.Green:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0, 1, 0, 1);
				break;
			case Colours.Purple:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0.5f, 0, 1, 1);
				break;
			case Colours.Black:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0, 0, 0, 1);
				break;
			case Colours.White:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 1, 1, 1);
				break;
			case Colours.Pink:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 0, 0.5f, 1);
				break;
			case Colours.Brown:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0.4f, 0.2f, 0, 1);
				break;
			case Colours.Lime:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(0.2f, 1, 0.2f, 1);
				break;
			default:
				PartyMembers[i].transform.GetComponent<SpriteRenderer>().color
				= new Color(1, 1, 1, 1);
				break;
			}

		}

    }

    // Update is called once per frame
    void Update()
    {

    	noRepeat = true;

        if (Input.GetMouseButtonDown(0))
        {
        	Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

		    if (hit.collider != null) {


		    	if(hit.collider.gameObject.tag == "Party")
		    	{
		    		CheckParty(hit.collider.gameObject);

			    	if(noRepeat)
			    	{
			    		AddMember(hit.collider.gameObject);
			    	} else {
			    		RemoveMember(hit.collider.gameObject);
			    	}

			    	OutputParty();
		    	}

		    	if(hit.collider.gameObject.tag == "Attack")
		    	{

		    		StartCoroutine(AttackParty(robotColour));

		    	}

			}
        }
    }

    void CheckParty(GameObject member)
    {
    	for(int i = 0; i < partySize; i++)
		{
			if(PartyMemberOrder[i] == member.name)
			{
				noRepeat = false;
			}
		}
    }

    void AddMember(GameObject member)
    {
    	
		for(int i = 0; i < partySize; i++)
		{
			if(PartyMemberOrder[i] == null)
			{
				PartyMemberOrder[i] = member.name;

				string[] nameParse = PartyMemberOrder[i].Split(' ');
				int partyNumber = int.Parse(nameParse[2]);
				OrderNumbers[i] = partyNumber;

				i = partySize;
			}
		}

	}

	void RemoveMember(GameObject member)
    {
    	for(int i = 0; i < partySize; i++)
		{
			if(PartyMemberOrder[i] == member.name)
			{
				for(int j = i; j < partySize; j++)
				{
					if(j < partySize-1){
						PartyMemberOrder[j] = PartyMemberOrder[j+1];
						OrderNumbers[j] = OrderNumbers[j+1];
					} else {
						PartyMemberOrder[j] = null;
						OrderNumbers[j] = 0;
					}
				}

				i = partySize;
			}
		}
    }

	void OutputParty(){

		//RESET ORDER DISPLAY
    	for(int i = 0; i < partySize; i++)
		{

			PartyMembers[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

			//FOR DEBUGGING
			if(OrderNumbers[i] != 0)
			{

				//print(OrderNumbers[i]);

			}

		}

		for(int i = 0; i < partySize; i++)
		{

			for(int j = 1; j <= partySize; j++)
			{

				//DISPLAYS ORDER ABOVE IF NUMBER IS PRESENT (1-4)
				//ADD OUTPUT FOR DEBUGGING IF NECESSARY
				if(OrderNumbers[i] == j)
				{

					PartyMembers[OrderNumbers[i]-1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = OrderSprites[i];

				}

			}

		}

    }

    IEnumerator AttackParty(Colours robotColour)
    {

    	// RUNS ATTACK ANIMATION BASED ON PARTY ORDER
    	for(int i = 0; i < partySize; i++)
		{

			if(OrderNumbers[i] != 0)
			{

				PartyMembers[OrderNumbers[i]-1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
				Animator attack = PartyMembers[OrderNumbers[i]-1].transform.GetComponent<Animator>();
				attack.SetTrigger("AttackTrigger");

				yield return new WaitForSeconds(
					attack.GetCurrentAnimatorStateInfo(0).length
					+attack.GetCurrentAnimatorStateInfo(0).normalizedTime);

				//SETTING AND OUTPUT OF ROBOT COLOUR
				robotColour = (Colours)PartyColours[OrderNumbers[i]-1];
				//print(robotColour);
			}

			// CLEAN UP IN CASE ANYTHING IS MISSED
			OrderNumbers[i] = 0;
			PartyMemberOrder[i] = null;

		}

		// CLEAN UP IN CASE ANYTHING IS MISSED
		for(int i = 0; i < partySize; i++)
		{

			PartyMembers[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

		}

    }

}
