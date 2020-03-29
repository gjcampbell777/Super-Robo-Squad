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

    // Start is called before the first frame update
    void Start()
    {
        
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

		    		StartCoroutine(AttackParty());

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

    IEnumerator AttackParty()
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
