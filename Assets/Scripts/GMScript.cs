using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMScript : MonoBehaviour
{

	public GameObject[] PartyMembers;
	public Sprite[] OrderSprites;
	public GameObject Hit;
	public GameObject EnemyHit;
	public GameObject Sheild;

	public Text EnemyDamageText;
	public Text PartyDamageText;
	public Text EnemyHealthText;
	public Text PartyHealthText;

	public GameObject[] EnemyParts;

	public Sprite[] EnemyHead;
	public Sprite[] EnemyAntenna;
	public Sprite[] EnemyBody;
	public Sprite[] EnemyArm;
	public Sprite[] EnemyChest;
	public Sprite[] EnemyWeakness;

	private bool noRepeat = true;
	private bool sheild = false;
	private bool gameover = false;
	private bool victory = false;
	private int partyHealth = 16;
	private int enemyHealth = 16;
	private int partyAttack = 1;
	private int enemyAttack = 4;
	private static int partySize = 4;
	private string[] PartyMemberOrder = new string[partySize];
	private int[] OrderNumbers = new int[partySize];
	private int[] PartyColours = new int[partySize];

	enum Colours {
		Red, Yellow, Blue, Grey,
		Orange, Green, Purple, 
		Black, White,
		Pink, Brown, Lime,
		Cyan, PurpleBlue, OrangeYellow
	}

	Colours robotColour;

    // Start is called before the first frame update
    void Start()
    {

    	EnemyBuilder();

    	EnemyHealthText.text = enemyHealth.ToString();
    	PartyHealthText.text = partyHealth.ToString();
    	EnemyDamageText.text = null;
    	PartyDamageText.text = null;
     
    	robotColour = Colours.Red;

    	PartyColours[0] = Random.Range(0, 15);
    	PartyColours[1] = Random.Range(0, 15);
    	PartyColours[2] = Random.Range(0, 15);
    	PartyColours[3] = Random.Range(0, 15);

    	for(int i = 0; i < partySize; i++)
		{

			//SETTING AND OUTPUT OF ROBOT COLOUR
			PartyMembers[i].transform.GetComponent<SpriteRenderer>().color =
				SetColour(PartyColours[i]);
			//print(robotColour);

		}

    }

    // Update is called once per frame
    void Update()
    {

    	noRepeat = true;
    	EnemyHealthText.text = enemyHealth.ToString();
    	PartyHealthText.text = partyHealth.ToString();

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

        if(partyHealth <= 0 && gameover == false)
        {
        	partyHealth = 0;

        	for(int i = 0; i < partySize; i++)
			{
		    	Destroy(GameObject.FindWithTag("Party"));
		    }

		    print("The Super Robo Squad has been destroyed! Try again!");

		    gameover = true;

        }

        if(enemyHealth <= 0 && victory == false)
        {
        	enemyHealth = 0;

		    Destroy(GameObject.FindWithTag("Enemy"));

		    print("The enemy robot has been destroyed! Congrats!");

		    victory = true;

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

				attack.SetTrigger("AttackTrigger");

				print(PartyColours[OrderNumbers[i]-1]);

				//CHECKING IF AN "ATTACK" OR "NON-ATTACK" ROBOT IS GOING
				if(((Colours)PartyColours[OrderNumbers[i]-1] != Colours.Grey && 
					(Colours)PartyColours[OrderNumbers[i]-1] != Colours.White)
					&& sheild != true)
				{

					Hit.transform.GetComponent<SpriteRenderer>().color = 
					PartyMembers[OrderNumbers[i]-1].transform.GetComponent<SpriteRenderer>().color;

					Hit.SetActive(true);

					Animator hit = Hit.transform.GetComponent<Animator>();
					hit.SetTrigger("HitTrigger");

					yield return new WaitForSeconds(
						hit.GetCurrentAnimatorStateInfo(0).length
						+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

					Hit.SetActive(false);
					hit.SetTrigger("HitTrigger");
				
				} 

				//GREY ROBOT (SHEILD) IS GOING
				if ((Colours)PartyColours[OrderNumbers[i]-1] == Colours.Grey){

					sheild = true;
					Sheild.transform.GetComponent<SpriteRenderer>().color = 
					PartyMembers[OrderNumbers[i]-1].transform.GetComponent<SpriteRenderer>().color;
					Sheild.SetActive(true);

				} 

				//GREEN/LIME ROBOT (HEALTH) IS GOING
				if ((Colours)PartyColours[OrderNumbers[i]-1] == Colours.White){

					EnemyHit.transform.GetComponent<SpriteRenderer>().color = 
					PartyMembers[OrderNumbers[i]-1].transform.GetComponent<SpriteRenderer>().color;

					EnemyHit.SetActive(true);

					Animator hit = EnemyHit.transform.GetComponent<Animator>();
					hit.SetTrigger("HitTrigger");

					yield return new WaitForSeconds(
						hit.GetCurrentAnimatorStateInfo(0).length
						+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

					EnemyHit.SetActive(false);
					hit.SetTrigger("HitTrigger");

				}

				//SETTING AND OUTPUT OF ROBOT COLOUR
				robotColour = (Colours)PartyColours[OrderNumbers[i]-1];
				//print(robotColour);

				//PARTY MEMEBER ATTACKING ENEMY
				DamageParty(partyAttack, robotColour);
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

		StartCoroutine(AttackEnemy());

    }

    IEnumerator AttackEnemy()
    {

    	// RUNS ATTACK ANIMATION OF ENEMY
		Animator enemyAttackAnim = EnemyParts[2].transform.parent.GetComponent<Animator>();
		enemyAttackAnim.SetTrigger("AttackTrigger");

		yield return new WaitForSeconds(
			enemyAttackAnim.GetCurrentAnimatorStateInfo(0).length
			+enemyAttackAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

		EnemyHit.transform.GetComponent<SpriteRenderer>().color = 
			EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

		enemyAttackAnim.SetTrigger("AttackTrigger");

		//HIT EFFECT ONLY WORKS IF THERE IS NO SHEILD UP
		if(sheild == false)
		{

			EnemyHit.SetActive(true);

			Animator hit = EnemyHit.transform.GetComponent<Animator>();
			hit.SetTrigger("HitTrigger");

			yield return new WaitForSeconds(
				hit.GetCurrentAnimatorStateInfo(0).length
				+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

			hit.SetTrigger("HitTrigger");

			EnemyHit.SetActive(false);

			EnemyHit.transform.GetComponent<SpriteRenderer>().color = 
				EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

			EnemyHit.SetActive(true);

			hit.SetTrigger("HitTrigger");

			yield return new WaitForSeconds(
				hit.GetCurrentAnimatorStateInfo(0).length
				+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

			EnemyHit.SetActive(false);
			hit.SetTrigger("HitTrigger");

		}

		//ENEMY ATTACKING PARTY (DEFAULT DISPLAY TEXT TO RED FOR NOW)
		DamageEnemy(enemyAttack, Colours.Red);

    }

    void DamageParty(int partyAttack, Colours partyMember)
    {

    	int modifier = 1;
    	int modifiedAttack = 1;

    	if(partyMember == Colours.Grey || partyMember == Colours.White)
    	{

    		modifier = 0;

    		if(partyMember == Colours.White)
    		{

    			partyHealth += 4;

    			StartCoroutine(DisplayDamageText(4, PartyDamageText, partyMember));

    			print(partyMember + " healed the team for 4");
    			print("Party health is now: " + partyHealth + " HP");

    		}

    	}

    	if(sheild == true) 
    	{
    		modifier = 0;
    	}

    	modifiedAttack = partyAttack * modifier;

    	StartCoroutine(DisplayDamageText(modifiedAttack, EnemyDamageText, partyMember));

    	enemyHealth -= modifiedAttack;
    	print(partyMember + " robot attacked for " + modifiedAttack);
    	print("Enemy robot health is now: " + enemyHealth + " HP");

    }

    void DamageEnemy(int enemyAttack, Colours enemyColour)
    {

    	int modifier = 1;
    	int modifiedAttack = 4;

    	//HIT ONLY DAMAGES IF THERE IS NO SHEILD UP
    	if(sheild == true)
    	{

    		modifier = 0;
    		print("Sheild Hit!");
    		Sheild.SetActive(false);
    		sheild = false;

    	}

    	modifiedAttack = enemyAttack * modifier;

    	StartCoroutine(DisplayDamageText(modifiedAttack, PartyDamageText, enemyColour));

    	partyHealth -= modifiedAttack;
    	print(enemyColour + " enemy robot attacked for " + modifiedAttack);
    	print("Party health is now: " + partyHealth + " HP");

    }

    IEnumerator DisplayDamageText(int damage, Text display, Colours displayColour)
    {

    	display.color = SetColour((int)displayColour);
    	display.text = damage.ToString();

    	yield return new WaitForSeconds(1);

    	display.text = null;

    }

    void EnemyBuilder()
    {

    	EnemyParts[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyAntenna[Random.Range(0, EnemyAntenna.Length)];

    	EnemyParts[0].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyHead[Random.Range(0, EnemyHead.Length)];

    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyBody[Random.Range(0, EnemyBody.Length)];

    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];

    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];

    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyChest[Random.Range(0, EnemyChest.Length)];

    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyWeakness[Random.Range(0, EnemyWeakness.Length)];

    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(Random.Range(0, 12));

    }

    Color SetColour(int robotColour)
    {
    	Color colourValue = new Color(1, 1, 1, 1);

    	switch((Colours)robotColour)
			{
			case Colours.Red:
				colourValue
				= new Color(1, 0, 0, 1);
				break;
			case Colours.Yellow:
				colourValue
				= new Color(1, 1, 0, 1);
				break;
			case Colours.Blue:
				colourValue
				= new Color(0, 0, 1, 1);
				break;
			case Colours.Grey:
				colourValue
				= new Color(0.5f, 0.5f, 0.5f, 1);
				break;
			case Colours.Orange:
				colourValue
				= new Color(1, 0.5f, 0, 1);
				break;
			case Colours.Green:
				colourValue
				= new Color(0, 0.8f, 0, 1);
				break;
			case Colours.Purple:
				colourValue
				= new Color(0.5f, 0, 1, 1);
				break;
			case Colours.Black:
				colourValue
				= new Color(0, 0, 0, 1);
				break;
			case Colours.White:
				colourValue
				= new Color(1, 1, 1, 1);
				break;
			case Colours.Pink:
				colourValue
				= new Color(1, 0, 0.5f, 1);
				break;
			case Colours.Brown:
				colourValue
				= new Color(0.4f, 0.2f, 0, 1);
				break;
			case Colours.Lime:
				colourValue
				= new Color(0.5f, 1, 0, 1);
				break;
			case Colours.Cyan:
				colourValue
				= new Color(0, 1, 1, 1);
				break;
			case Colours.PurpleBlue:
				colourValue
				= new Color(0.2f, 0, 1, 1);
				break;
			case Colours.OrangeYellow:
				colourValue
				= new Color(1, 0.87f, 0, 1);
				break;
			default:
				colourValue
				= new Color(1, 1, 1, 1);
				break;
			}

    	return colourValue;

    }

}
