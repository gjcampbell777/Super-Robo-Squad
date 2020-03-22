using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{

	public GameObject[] PartyMembers;
	private bool noRepeat = true;
	private static int partySize = 4;
	private string[] PartyMemberOrder =  new string[partySize];

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
		    	CheckParty(hit.collider.gameObject);

		    	if(noRepeat)
		    	{
		    		AddMember(hit.collider.gameObject);
		    	} else {
		    		RemoveMember(hit.collider.gameObject);
		    	}

		    	OutputParty();

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
					} else {
						PartyMemberOrder[j] = null;
					}
				}

				i = partySize;
			}
		}
    }

	void OutputParty(){

    	for(int i = 0; i < partySize; i++)
		{
			if(PartyMemberOrder[i] != null)
			{
				print(PartyMemberOrder[i]);
			} else {
				print("Empty.");
			}
		}

    }

}
