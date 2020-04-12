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
	private bool buff = false;
	private bool gameover = false;
	private bool victory = false;
	private int partyHealth = 16;
	private int enemyHealth = 32;
	private int partyAttack = 2;
	private int enemyAttack = 4;
	private static int partySize = 4;
	private string[] PartyMemberOrder = new string[partySize];
	private int[] OrderNumbers = new int[partySize];
	private int[] PartyColours = new int[partySize];
	private int[] EnemtyPartsColours;
	private int[] EnemyKillSequence;

	enum Colours {
		Red, Brown, Orange,
		Amber, Yellow, Lime,
		Green, Cyan, Blue, 
		Indigo, Purple, Pink,
		White, Grey, Black
	}

	Colours robotColour;

    // Start is called before the first frame update
    void Start()
    {

    	EnemtyPartsColours = new int[EnemyParts.Length];
    	// KILL SEQUENCE RANGES FROM LENGTH OF 2 TO 5 HITS
    	EnemyKillSequence = new int[Random.Range(2, 5)];

    	// COLOURS FOR KILL SEQUENCE ARE GENERATED HERE
    	for(int i = 0; i < EnemyKillSequence.Length - 1; i++)
    	{
    		EnemyKillSequence[i] = Random.Range(0, 12);
    	}

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

        if (Input.GetMouseButtonDown(0) && gameover == false && victory == false)
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

		    	if(hit.collider.gameObject.tag == "Attack" && PartyMemberOrder[0] != null)
		    	{

		    		StartCoroutine(AttackParty(robotColour));

		    	}

			}
        }

        if(partyHealth <= 0 && gameover == false)
        {
        	partyHealth = 0;

        	GameObject[] party = GameObject.FindGameObjectsWithTag("Party");
   			foreach(GameObject memeber in party) GameObject.Destroy(memeber);

		    print("The Super Robo Squad has been destroyed! Try again!");

		    gameover = true;

        }

        if(enemyHealth <= 0 || victory == true)
        {
        	//enemyHealth = 0;

		    GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
   			foreach(GameObject parts in enemy) GameObject.Destroy(parts);

		    print("The enemy robot has been destroyed! Congrats!");

		    victory = false;
		    gameover = true;

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

    	float modifier = 1.0f;
    	float modifiedAttack = 2.0f;
    	int colourModifier = ColourCompare(partyMember, (Colours)EnemtyPartsColours[2]);

    	// ATTACK MODIFIER BASED ON THE RELATION OF THE COLOUR OF THE PARTY MEMBER
    	// AND THE COLOUR OF THE ENEMY ROBOT BASE BODY COLOUR
    	switch(colourModifier)
    	{
    		case(2):
    			modifier = 2;
    			break;
    		case(1):
    			modifier = 1.5f;
    			break;
    		case(-1):
    			modifier = 0.5f;
    			break;
    		case(-2):
    			modifier = 0;
    			break;
    		default:
    			break;
    	}

    	// DETERMINING A SEQUENCE BREAK ABSED ON THE NEXT COLOUR IN THE SEQUENCE
    	// IF IT'S THE LAST COLOUR THE ROBOT IS DESTROYED
    	// IF NOT COLOURS AND SPRITES AND CHANGED ACCORDINGLY
    	if(ColourCompare(partyMember, (Colours)EnemyKillSequence[0]) > 0)
    	{

    		print("Weakness hit!");

    		if(EnemyKillSequence[1] == -1)
    		{

    			EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    			victory = true;

			} else {

				for(int i = 0; i < EnemyKillSequence.Length - 1; i++)
				{
					EnemyKillSequence[i] = EnemyKillSequence[i+1];
				}

				EnemyKillSequence[EnemyKillSequence.Length-1] = -1;

				if(EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != null)
				{
					EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
				} else {
					EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
						SetColour(EnemyKillSequence[0]);
				}

			}

    	}

    	// ACTIONS DONE FOR NON-ATTACK ROBOTS
    	if(partyMember == Colours.Grey || 
    		partyMember == Colours.White || 
    		partyMember == Colours.Black)
    	{

    		if(partyMember == Colours.White)
    		{

    			if(buff)
		    	{
		    		modifiedAttack = (partyAttack * modifier) * 2;
				} else {
					modifiedAttack = partyAttack * modifier;
				}

				partyHealth += (int)modifiedAttack;

    			StartCoroutine(DisplayDamageText((int)modifiedAttack, PartyDamageText, partyMember));

    			print(partyMember + " healed the team for 4");
    			print("Party health is now: " + partyHealth + " HP");
    			
    		}

    		if(partyMember == Colours.Black)
    		{
    			StartCoroutine(DisplayDamageText(0, PartyDamageText, partyMember));

    			print(partyMember + " buffed the team!");
    			print("Party health is now: " + partyHealth + " HP");
    			buff = true;

    		}

    	
    		modifier = 0;

    	}

    	// ATTACKS ARE BLOCK IF SHEILD IS UP
    	if(sheild == true) 
    	{
    		modifier = 0;
    	}

    	// ATTACKS ARE DOUBLE DAMAGE IF BLACK ROBOT HAS DONE ITS ACTION
    	if(buff)
    	{
    		modifiedAttack = (partyAttack * modifier) * 2;
		} else {
			modifiedAttack = partyAttack * modifier;
		}

    	StartCoroutine(DisplayDamageText((int)modifiedAttack, EnemyDamageText, partyMember));

    	enemyHealth -= (int)modifiedAttack;
    	print(partyMember + " robot attacked for " + modifiedAttack);
    	print("Enemy robot health is now: " + enemyHealth + " HP");

    }

    void DamageEnemy(int enemyAttack, Colours enemyColour)
    {

    	int modifier = 0;
    	int modifiedAttack = 4;

    	for(int i = 0; i < 2; i++){
    		for(int j = 0; j < partySize; j++){

    			modifier += 
    			ColourCompare((Colours)EnemtyPartsColours[3+i], (Colours)PartyColours[j]);

    		}

    	}

    	modifiedAttack = enemyAttack + modifier;

    	//HIT ONLY DAMAGES IF THERE IS NO SHEILD UP
    	if(sheild == true)
    	{

    		modifiedAttack = 0;
    		print("Sheild Hit!");
    		Sheild.SetActive(false);
    		sheild = false;

    	}

    	buff = false;

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

    	EnemtyPartsColours[0] = Random.Range(0, 15);
    	EnemyParts[0].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[0]);

    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyHead[Random.Range(0, EnemyHead.Length)];

    	EnemtyPartsColours[1] = Random.Range(0, 15);
    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[1]);

    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyBody[Random.Range(0, EnemyBody.Length)];

    	EnemtyPartsColours[2] = Random.Range(0, 12);
    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[2]);

    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];

    	EnemtyPartsColours[3] = Random.Range(0, 12);
    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[3]);

    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];

    	EnemtyPartsColours[4] = Random.Range(0, 12);
    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[4]);

    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyChest[Random.Range(0, EnemyChest.Length)];

    	EnemtyPartsColours[5] = EnemyKillSequence[0];
    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[5]);
			EnemyKillSequence[0] = EnemtyPartsColours[5];

    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyWeakness[Random.Range(0, EnemyWeakness.Length)];

    	EnemtyPartsColours[6] = EnemyKillSequence[1];
    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemtyPartsColours[6]);
			EnemyKillSequence[1] = EnemtyPartsColours[6];

    }

    int ColourCompare(Colours AttackColour, Colours DefendColour)
    {

    	switch(AttackColour)
    		{
    		// WHITE, GREY AND BLACK ARE NEITHER WEAK NOR STRONG AGAINST
    		// DEFENDING COLOUR SINCE THEY ARE "NON-ATTACK" COLOURS
    		case Colours.White:
    			return 0;
    		case Colours.Grey:
    			return 0;
    		case Colours.Black:
    			return 0;
    		case Colours.Red:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Brown:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Orange:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Amber:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Yellow:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Lime:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Green:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Cyan:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Blue:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Indigo:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Purple:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		case Colours.Pink:
    			// DEFEND COLOUR HAS STRONG DEFENSE IF ATTACK COLOUR IS SAME
    			if(DefendColour == AttackColour)
    			{
    				return -2;
    			} 
    			// DEFEND COLOUR HAS GOOD DEFENSE IF ATTACK COLOUR IS AN ADJACENT COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour-1)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+1)%12))
    			{
    				return -1;
    			} 
    			// DEFEND COLOUR HAS NO DEFENSE IF ATTACK COLOUR IS OPPOSITE
    			else if (DefendColour == (Colours)(((int)AttackColour+6)%12))
    			{
    				return 2;
    			} 
    			// DEFEND COLOUR HAS WEAK DEFENSE IF ATTACK COLOUR IS ADJACENT TO OPPOSITE COLOUR
    			else if (DefendColour == (Colours)(((int)AttackColour+5)%12) ||
    				DefendColour == (Colours)(((int)AttackColour+7)%12))
    			{
    				return 1;
    			}
    			break;
    		default:
    			return 0;
    		}

    	// IF AN ATTACKING COLOUR DOESN'T MATCH WITH ANY OF THE ABOVE PARAMETERS
    	// IT IS NEITHER WEAK NOR STRONG AGAINST THE DEFENDING COLOUR
    	return 0;
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
			case Colours.Indigo:
				colourValue
				= new Color(0.2f, 0, 1, 1);
				break;
			case Colours.Amber:
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
