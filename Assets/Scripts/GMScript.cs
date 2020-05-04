﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour
{

	public bool isRandom;

	public int[] SetPartyColours = new int[partySize];
	public int[] SetEnemyColours =  new int[5];
	public int[] SetKillSequence = new int [8];
	public int SetModelNum;

	public GameObject[] PartyMembers;
	public Sprite[] OrderSprites;
	public GameObject Hit;
	public GameObject EnemyHit;
	public GameObject Shield;
	public GameObject EnemyShield;

	public GameObject Victory;
	public GameObject GameOver;
	public GameObject AttackStopSign;
	public GameObject ShieldHit;
	public GameObject WeakPointHit;

	public Text EnemyDamageText;
	public Text PartyDamageText;
	public Text EnemyHealthText;
	public Text PartyHealthText;
	public Text LevelNumber;
	public GameObject MaxLevelNumber;
	public GameObject Divider;

	public GameObject[] EnemyParts;

	public Sprite[] EnemyHead;
	public Sprite[] EnemyAntenna;
	public Sprite[] EnemyBody;
	public Sprite[] EnemyArm;
	public Sprite[] EnemyChest;
	public Sprite[] EnemyWeakness;
	public Sprite[] ModelNumber;

	public Sprite[] Symbols;

	private bool partyRedo = false;
	private bool cursorLock = false;
	private bool noRepeat = true;
	private bool shield = false;
	private bool enemyShield = false;
	private bool buff = false;
	private bool weakPointHit = false;
	private bool gameover = false;
	private bool victory;
	private int buffAmount = 0;
	private int modelNum;
	private int partyHealth = 8;
	private int enemyHealth = 16;
	private int partyAttack = 2;
	private int enemyAttack = 2;
	private int level;
	private int maxLevel;
	private static int partySize = 4;
	private string[] PartyMemberOrder = new string[partySize];
	private int[] OrderNumbers = new int[partySize];
	private int[] PartyColours = new int[partySize];
	private int[] EnemyPartsColours;
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
    	victory = false;
    	gameover = false;
    	enemyHealth = 16;
    	partyHealth = 8;
    	level = PlayerPrefs.GetInt("Level");

    	if(PlayerPrefs.GetInt("GameMode") == 0 ||
    	 PlayerPrefs.GetInt("GameMode") == 2)
    	{
    		isRandom = false;
    	} else {
    		isRandom = true;
    	}

    	if(PlayerPrefs.GetInt("GameMode") == 2)
    	{
    		partyHealth = 1;
    	}

    	if(isRandom){

    		maxLevel = 9999;

	    	// GENERATES PARTY MEMBER COLOURS
	    	do{

	    		partyRedo = false;
	    		buffAmount = 0;

	    		PartyColours[0] = Random.Range(0, 15);
		    	PartyColours[1] = Random.Range(0, 15);
		    	PartyColours[2] = Random.Range(0, 15);
		    	PartyColours[3] = Random.Range(0, 15);

		    	// CHECKS THAT NO ROBOTS SHARE A COLOUR AND THERE ARE NO MORE THAN 1 BUFF ABILITY ROBOT
		    	// IF EITHER OCCUR THE PARTY GETS RANDOMIZED AND CHECKED UNTIL THAT IS NO LONGER THE CASE
		    	for(int i = 0; i < partySize; i++)
		    	{
		    		for(int j = 0; j < partySize; j++)
		    		{

		    			if(PartyColours[i] == PartyColours[j] && i != j) partyRedo = true;

		    		}
		    		if(PartyColours[i] >= 12) buffAmount++;
		    	}

		    	if(buffAmount > 1) partyRedo = true;

	    	}while(partyRedo);

	    	EnemyPartsColours = new int[EnemyParts.Length];
	    	// KILL SEQUENCE RANGES FROM LENGTH OF 3 TO 8 HITS
	    	EnemyKillSequence = new int[Random.Range((4-buffAmount), 8)];

	    	EnemyPartsColours[0] = Random.Range(0, 15);
			EnemyPartsColours[1] = Random.Range(0, 15);
			EnemyPartsColours[2] = Random.Range(0, 12);
			EnemyPartsColours[3] = Random.Range(0, 12);
			EnemyPartsColours[4] = Random.Range(0, 12);

	    	// COLOURS FOR KILL SEQUENCE ARE GENERATED HERE
	    	for(int i = 0; i < EnemyKillSequence.Length; i++)
	    	{
	    		
	    		do{
	    			EnemyKillSequence[i] = Random.Range(0, 12);
	    		}while(EnemyKillSequence[i] != PartyColours[0] &&
	    			EnemyKillSequence[i] != PartyColours[1] &&
	    			EnemyKillSequence[i] != PartyColours[2] &&
	    			EnemyKillSequence[i] != PartyColours[3]);
	    		
	    		EnemyKillSequence[i] = (EnemyKillSequence[i]+6)%12;

	    	}

	    	modelNum = Random.Range(0, 5);

    	} else {

    		Divider.SetActive(true);
    		MaxLevelNumber.SetActive(true);

    		if(PlayerPrefs.GetInt("GameMode") == 0)
	    	{
	    		
	    		maxLevel = 5;

	    		if(PlayerPrefs.GetInt("Level") == 1)
	    		{
	    			SeedParse(new int[] {0,1,11,13,0,6,8,0,0,3,6,5,7,0});
	    		} else if(PlayerPrefs.GetInt("Level") == 2){
	    			SeedParse(new int[] {0,4,8,12,4,5,9,1,10,3,2,6,10,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 3){
	    			SeedParse(new int[] {2,6,10,7,12,0,8,11,5,4,1,4,0,8,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 4){
	    			SeedParse(new int[] {4,10,1,14,14,1,0,5,8,5,10,4,7,10,7,0});
    			} else if (PlayerPrefs.GetInt("Level") == 5){
    				SeedParse(new int[] {3,4,5,6,3,1,0,2,9,6,0,9,11,10,11,10,0});
				} else if (PlayerPrefs.GetInt("Level") >= 6){
					SeedParse(new int[] {6,0,8,4,4,6,10,2,8,7,6,0,10,2,10,2,6,0});
					SceneManager.LoadScene("Mode Select Scene");
				}
	    		
	    	}

    		if(PlayerPrefs.GetInt("GameMode") == 2)
	    	{
	    		
	    		maxLevel = 10;

	    		if(PlayerPrefs.GetInt("Level") == 1)
	    		{
	    			SeedParse(new int[] {7,8,6,9,4,4,4,4,4,3,2,2,2,0});
	    		} else if(PlayerPrefs.GetInt("Level") == 2){
	    			SeedParse(new int[] {0,11,1,14,4,4,4,4,4,3,7,5,6,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 3){
	    			SeedParse(new int[] {7,7,7,7,4,4,4,4,4,3,2,2,2,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 4){
	    			SeedParse(new int[] {7,7,7,7,4,4,4,4,4,3,2,2,2,0});
    			} else if (PlayerPrefs.GetInt("Level") == 5){
    				SeedParse(new int[] {7,7,7,7,4,4,4,4,4,3,2,2,2,0});
				} else if (PlayerPrefs.GetInt("Level") == 6){
					SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
				} else if(PlayerPrefs.GetInt("Level") == 7){
	    			SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 8){
	    			SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
	    		} else if (PlayerPrefs.GetInt("Level") == 9){
	    			SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
    			} else if (PlayerPrefs.GetInt("Level") == 10){
    				SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
				} else if (PlayerPrefs.GetInt("Level") >= 11){
					SeedParse(new int[] {7,7,7,7,4,4,4,4,4,4,2,2,2,2,0});
					SceneManager.LoadScene("Mode Select Scene");
				}

	    	}

	    	MaxLevelNumber.transform.GetChild(0).GetComponent<Text>().text = maxLevel.ToString();

    	}

    	EnemyPartsColours[5] = EnemyKillSequence[0];
		EnemyPartsColours[6] = EnemyKillSequence[1];

    	EnemyBuilder();

    	EnemyHealthText.text = enemyHealth.ToString();
    	PartyHealthText.text = partyHealth.ToString();
    	EnemyDamageText.text = null;
    	PartyDamageText.text = null;
    	LevelNumber.text = level.ToString();
     
    	robotColour = Colours.Red;

    	for(int i = 0; i < partySize; i++)
		{

			//SETTING AND OUTPUT OF ROBOT COLOUR
			PartyMembers[i].transform.GetComponent<SpriteRenderer>().color =
				SetColour(PartyColours[i]);
			PartyMembers[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
				Symbols[PartyColours[i]];
			PartyMembers[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
				SetColour(PartyColours[i]);
			//print(robotColour);

		}

		EnemyParts[5].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    	LevelNumber.text = level.ToString();

    	if(PlayerPrefs.GetInt("GameMode") == 3)
    	{
    		partyHealth = 100;
    	}

    	noRepeat = true;
    	EnemyHealthText.text = enemyHealth.ToString();
    	PartyHealthText.text = partyHealth.ToString();

        if (Input.GetMouseButtonDown(0) && gameover == false && victory == false)
        {
        	Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

		    if (hit.collider != null) {

		    	if(hit.collider.gameObject.tag == "Restart")
		    	{
		    		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		    	}

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

     				cursorLock = true;

		    		StartCoroutine(AttackParty(robotColour));

		    	} else if (hit.collider.gameObject.tag == "Attack" 
		    		&& PartyMemberOrder[0] == null) {

		    		cursorLock = true;

		    		StartCoroutine(AttackStop());

		    	}

			}
        }

        if(partyHealth <= 0 && gameover == false)
        {
        	partyHealth = 0;

        	GameObject[] party = GameObject.FindGameObjectsWithTag("Party");
   			foreach(GameObject memeber in party) GameObject.Destroy(memeber);

		    //print("The Super Robo Squad has been destroyed! Try again!");

		    gameover = true;

		    cursorLock = false;

		    StartCoroutine(EndGameDisplay(GameOver));

        }

        if(enemyHealth <= 0 || victory == true)
        {
        	enemyHealth = 0;

		    GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
   			foreach(GameObject parts in enemy) parts.SetActive(false);

		    //print("The enemy robot has been destroyed! Congrats!");

		    gameover = true;

		    cursorLock = false;

		    StartCoroutine(NextLevelDisplay(Victory));

		    if(PlayerPrefs.GetInt("Level") >= maxLevel)
		    {
		    	StartCoroutine(BackToModeSelect());
		    }

        }

        if (Input.GetMouseButtonDown(0))
        {

        	Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        	
	        if (hit.collider != null) {
		    	if(hit.collider.gameObject.tag == "Restart")
		    	{
		    		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		    	}

		    	if(hit.collider.gameObject.tag == "Back")
		    	{
		    		SceneManager.LoadScene("Mode Select Scene");
		    	}
			}
		}

        if(cursorLock)
        {

        	Cursor.lockState = CursorLockMode.Locked;
     		Cursor.visible = false;

    	} else {

    		Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

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

    IEnumerator AttackStop()
    {

    	AttackStopSign.SetActive(true);

    	yield return new WaitForSeconds(1);

    	AttackStopSign.SetActive(false);
    	cursorLock = false;

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

				//print(PartyColours[OrderNumbers[i]-1]);

				//SETTING AND OUTPUT OF ROBOT COLOUR
				robotColour = (Colours)PartyColours[OrderNumbers[i]-1];
				//print(robotColour);

				//PARTY MEMEBER ATTACKING ENEMY
				yield return DamageParty(partyAttack, robotColour);
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

		if(!victory && enemyHealth > 0)
		{
			yield return StartCoroutine(AttackEnemy());
		}

    }

    IEnumerator AttackEnemy()
    {

    	// PRE ATTACK BUFF EFFECT ANIMATION
    	yield return StartCoroutine(PreAttackBuffAnimEnemy());

    	// THIS DETERMINES IF THE ENEMY WILL ATTACK WITH THE RIGHT OR LEFT ARM FIRST
    	if(Random.Range(0, 2) == 0)
    	{

    		// ANIMATION FOR RIGHT ARM
	    	yield return StartCoroutine(AttackAnimEnemy(3));

			// RIGHT ARM EFFECT ANIMATION AND DAMAGE CALCULATIONS
			yield return StartCoroutine(DamageEnemy(enemyAttack, EnemyPartsColours[3]));

			// ANIMATION FOR LEFT ARM
			yield return StartCoroutine(AttackAnimEnemy(4));

			// LEFT ARM EFFECT ANIMATION AND DAMAGE CALCULATIONS
			yield return StartCoroutine(DamageEnemy(enemyAttack, EnemyPartsColours[4]));

    	} else {

    		// ANIMATION FOR LEFT ARM
			yield return StartCoroutine(AttackAnimEnemy(4));

			// LEFT ARM EFFECT ANIMATION AND DAMAGE CALCULATIONS
			yield return StartCoroutine(DamageEnemy(enemyAttack, EnemyPartsColours[4]));

			// ANIMATION FOR RIGHT ARM
	    	yield return StartCoroutine(AttackAnimEnemy(3));

			// RIGHT ARM EFFECT ANIMATION AND DAMAGE CALCULATIONS
			yield return StartCoroutine(DamageEnemy(enemyAttack, EnemyPartsColours[3]));

    	}
		
		// POST ATTACK ABILITY EFFECT ANIMATION
		yield return StartCoroutine(PostAttackBuffAnimEnemy());

    }

    IEnumerator HitEffectParty(int damage, Text display, Colours partyMember)
    {

    	//CHECKING IF AN "ATTACK" OR "NON-ATTACK" ROBOT IS GOING
		if(partyMember != Colours.Grey && 
			partyMember != Colours.White &&
			partyMember != Colours.Black)
		{

			Hit.transform.GetComponent<SpriteRenderer>().color = 
			SetColour((int)partyMember);

			Hit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
			Symbols[(int)partyMember];

			Hit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = 
			SetColour((int)partyMember);

			DisplayDamageText((int)damage, display, partyMember);
			
			if(!shield && !enemyShield)
			{
				Hit.SetActive(true);

				Animator hit = Hit.transform.GetComponent<Animator>();
				hit.SetTrigger("HitTrigger");

				yield return new WaitForSeconds(
					hit.GetCurrentAnimatorStateInfo(0).length * 2
					+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

				hit.SetTrigger("HitTrigger");
				Hit.SetActive(false);
			} else {

				yield return new WaitForSeconds(1);

			}

			display.text = null;

		} 

		// GREY ROBOT (SHIELD) IS GOING
		if (partyMember == Colours.Grey){

			shield = true;
			Shield.transform.GetComponent<SpriteRenderer>().color = 
			SetColour((int)partyMember);
			Shield.SetActive(true);

		} 

		// NON-ATTACK ROBOT IS GOING
		if (partyMember == Colours.White ||
			partyMember == Colours.Grey ||
			partyMember == Colours.Black){

			EnemyHit.transform.GetComponent<SpriteRenderer>().color = 
			SetColour((int)partyMember);

			EnemyHit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
			Symbols[(int)partyMember];

			EnemyHit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = 
			SetColour((int)partyMember);

			if(partyMember == Colours.White) DisplayDamageText((int)damage, display, partyMember);
			EnemyHit.SetActive(true);

			Animator hit = EnemyHit.transform.GetComponent<Animator>();
			hit.SetTrigger("HitTrigger");

			yield return new WaitForSeconds(
				hit.GetCurrentAnimatorStateInfo(0).length * 2
				+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

			hit.SetTrigger("HitTrigger");
			EnemyHit.SetActive(false);
			display.text = null;
			
		}

		ShieldHit.SetActive(false);
    	WeakPointHit.SetActive(false);

    }

    IEnumerator HitEffectEnemy(int attack, int attackColour)
    {

    	EnemyHit.transform.GetComponent<SpriteRenderer>().color = 
			SetColour(attackColour);

		EnemyHit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
			Symbols[attackColour];

		EnemyHit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = 
			SetColour(attackColour);

    	if((Colours)attackColour == Colours.White ||
			(Colours)attackColour == Colours.Grey ||
			(Colours)attackColour == Colours.Black)
		{
			Hit.transform.GetComponent<SpriteRenderer>().color = 
			SetColour(attackColour);

			Hit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
			Symbols[attackColour];

			Hit.transform.GetChild(0).GetComponent<SpriteRenderer>().color = 
			SetColour(attackColour);

			if((Colours)attackColour == Colours.White) 
			{
				DisplayDamageText(attack, EnemyDamageText, (Colours)attackColour);
			}
			Hit.SetActive(true);

			Animator hit = Hit.transform.GetComponent<Animator>();
			hit.SetTrigger("HitTrigger");

			yield return new WaitForSeconds(
				hit.GetCurrentAnimatorStateInfo(0).length * 2
				+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

			hit.SetTrigger("HitTrigger");
			EnemyDamageText.text = null;
			Hit.SetActive(false);
			
		}

		// ENEMY SHIELD GOES UP IF 
		if ((Colours)attackColour == Colours.Grey){

			enemyShield = true;
			EnemyShield.transform.GetComponent<SpriteRenderer>().color = 
			SetColour(attackColour);
			EnemyShield.SetActive(true);

		}

		//HIT EFFECT ONLY WORKS IF THERE IS NO SHIELD UP
		if( (Colours)attackColour != Colours.White &&
			(Colours)attackColour != Colours.Grey &&
			(Colours)attackColour != Colours.Black)
		{

			DisplayDamageText(attack, PartyDamageText, (Colours)attackColour);

			if(!shield && !enemyShield)
			{
				EnemyHit.SetActive(true);

				Animator hit = EnemyHit.transform.GetComponent<Animator>();
				hit.SetTrigger("HitTrigger");

				yield return new WaitForSeconds(
					hit.GetCurrentAnimatorStateInfo(0).length * 2
					+hit.GetCurrentAnimatorStateInfo(0).normalizedTime);

				hit.SetTrigger("HitTrigger");
				EnemyHit.SetActive(false);
			} else {

				yield return new WaitForSeconds(1);

			}

			PartyDamageText.text = null;

		}

		ShieldHit.SetActive(false);
    	WeakPointHit.SetActive(false);

    }

    IEnumerator DamageParty(int partyAttack, Colours partyMember)
    {

    	float modifier = 1.0f;
    	float modifiedAttack = 2.0f;
    	float modifiedHeal = 0f;
    	int colourModifier = ColourCompare(partyMember, (Colours)EnemyPartsColours[2]);

    	weakPointHit = false;

    	// ATTACK MODIFIER BASED ON THE RELATION OF THE COLOUR OF THE PARTY MEMBER
    	// AND THE COLOUR OF THE ENEMY ROBOT BASE BODY COLOUR
    	switch(colourModifier)
    	{
    		case(2):
    			modifier = 2;
    			if(EnemyPartsColours[1] == EnemyPartsColours[2]) modifier = 4;
    			break;
    		case(1):
    			modifier = 1.5f;
    			if(EnemyPartsColours[1] == EnemyPartsColours[2]) modifier = 2;
    			break;
    		case(-1):
    			modifier = 0.5f;
    			if(EnemyPartsColours[1] == EnemyPartsColours[2]) modifier = 0;
    			break;
    		case(-2):
    			modifier = 0;
    			if(EnemyPartsColours[1] == EnemyPartsColours[2]) modifier = -1;
    			break;
    		default:
    			break;
    	}

    	// DETERMINING A SEQUENCE BREAK ABSED ON THE NEXT COLOUR IN THE SEQUENCE
    	// IF IT'S THE LAST COLOUR THE ROBOT IS DESTROYED
    	// IF NOT COLOURS AND SPRITES AND CHANGED ACCORDINGLY
    	if(ColourCompare(partyMember, (Colours)EnemyKillSequence[0]) > 0 
    		&& !shield && !enemyShield)
    	{

    		//print("Weakness hit!");
    		weakPointHit = true;

    		if(EnemyKillSequence[1] == -1)
    		{

    			EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    			victory = true;
    			enemyHealth = 0;

			} else {

				for(int i = 0; i < EnemyKillSequence.Length - 1; i++)
				{
					EnemyKillSequence[i] = EnemyKillSequence[i+1];
				}

				EnemyKillSequence[EnemyKillSequence.Length-1] = -1;

				if(EnemyParts[5].activeSelf)
				{
					EnemyParts[5].SetActive(false);
				} else {
					EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
						SetColour(EnemyKillSequence[0]);
					EnemyParts[6].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
						Symbols[EnemyKillSequence[0]];
					EnemyParts[6].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
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
		    		modifiedHeal = (partyAttack * modifier) * 2;
				} else {
					modifiedHeal = partyAttack * modifier;
				}

				partyHealth += (int)modifiedHeal;

    			//print(partyMember + " healed the team for " + modifiedHeal + " HP");
    			//print("Party health is now: " + partyHealth + " HP");
    			
    		}

    		if(partyMember == Colours.Black)
    		{
    			

    			//print(partyMember + " buffed the team!");
    			//print("Party health is now: " + partyHealth + " HP");
    			buff = true;

    		}

    	
    		modifier = 0;

    	}

    	// ATTACKS ARE BLOCK IF SHIELD IS UP
    	if(shield || enemyShield && 
    		(partyMember != Colours.White 
    			&& partyMember != Colours.Grey 
    			&& partyMember != Colours.Black)) 
    	{
    		modifier = 0;

    		if(enemyShield)
    		{
    			//print("Enemy Shield Hit!");
    			EnemyShield.SetActive(false);
    		}

    	}

    	// IN CASE MODIFIER IS NEGATIVE
    	if(modifier < 0) modifier = 0;

    	// ATTACKS ARE DOUBLE DAMAGE IF BLACK ROBOT HAS DONE ITS ACTION
    	if(buff)
    	{
    		modifiedAttack = (partyAttack * modifier) * 2;
		} else {
			modifiedAttack = partyAttack * modifier;
		}

		if(partyMember == Colours.White ||
		 	partyMember == Colours.Grey ||
		  	partyMember == Colours.Black)
		{
			yield return StartCoroutine(HitEffectParty((int)modifiedHeal, PartyDamageText, partyMember));
		} else{
			yield return StartCoroutine(HitEffectParty((int)modifiedAttack, EnemyDamageText, partyMember));
			enemyShield = false;
		}

    	enemyHealth -= (int)modifiedAttack;
    	//print(partyMember + " robot attacked for " + modifiedAttack);
    	//print("Enemy robot health is now: " + enemyHealth + " HP");

    }

    IEnumerator DamageEnemy(int enemyAttack, int attackColour)
    {

    	int modifier = 0;
    	int modifiedAttack = 2;

    	weakPointHit = false;

    	for(int i = 0; i < partySize; i++){

			// ATTACK MODIFIER BASED ON THE RELATION OF THE COLOUR OF THE ENEMY ARM
	    	// AND THE COLOUR OF THE PARTY MEMBERS COLOURS
    		if(ColourCompare((Colours)attackColour, (Colours)PartyColours[i]) > 0)
    		{
    			modifier += 1;
    		} else if (ColourCompare((Colours)attackColour, (Colours)PartyColours[i]) < 0) {
    			modifier -= 1;
    		}

    	}

    	// ADD DAMAGE IF EITHER OF THE ARMS MATCHES THE COLOUR OF THE ANTENNA
    	if(EnemyPartsColours[0] == attackColour) 
    	{
    		//print("Antenna Buff!");
    		modifier += 2;
    	}

    	// IN CASE MODIFIER MAKES ENEMY ATTACK NEGATIVE
    	if(modifier < -enemyAttack) modifier = -enemyAttack;

    	modifiedAttack = enemyAttack + modifier;

    	// RUN "NON-ATTACK" BUFF FOR ENEMY IF ANTENNA IS BLACK SO ATTACKS ARE BUFFED 
    	if((Colours)EnemyPartsColours[0] == Colours.Black)
    	{
    		modifiedAttack *= 2;
    	}

    	//HIT ONLY DAMAGES IF THERE IS NO SHIELD UP
    	if(shield || enemyShield)
    	{

    		modifiedAttack = 0;
    		//print("Shield Hit!");
    		Shield.SetActive(false);

    	}

    	buff = false;

    	yield return StartCoroutine(HitEffectEnemy((int)modifiedAttack, attackColour));

    	shield = false;

    	partyHealth -= modifiedAttack;
    	//print((Colours)attackColour + " enemy robot arm attacked for " + modifiedAttack);
    	//print("Party health is now: " + partyHealth + " HP");

    }

    void DisplayDamageText(int damage, Text display, Colours displayColour)
    {

    	display.color = SetColour((int)displayColour);
    	display.text = damage.ToString();

    	// ATTACKS ARE BLOCK IF SHIELD IS UP
    	if((shield || enemyShield) && 
    		(displayColour != Colours.White 
    			&& displayColour != Colours.Grey 
    			&& displayColour != Colours.Black)) ShieldHit.SetActive(true);

    	if(weakPointHit) 
    	{
    		weakPointHit = false;
    		WeakPointHit.SetActive(true);
    	}

    }

    IEnumerator AttackAnimEnemy(int arm)
    {
    	// RUNS ATTACK ANIMATION OF ENEMY
		Animator enemyAttackAnim = EnemyParts[arm].transform.GetComponent<Animator>();
		enemyAttackAnim.SetTrigger("AttackTrigger");

		yield return new WaitForSeconds(
			enemyAttackAnim.GetCurrentAnimatorStateInfo(0).length
			+enemyAttackAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

		enemyAttackAnim.SetTrigger("AttackTrigger");
    }

    IEnumerator PreAttackBuffAnimEnemy()
    {
    	yield return new WaitForSeconds(0.5f);

    	// RUN "NON-ATTACK" BUFF ANIMATION FOR ENEMY IF ANTENNA IS BLACK SO ATTACKS ARE BUFFED
    	// BEFORE THE ENEMY DOES ITS ATTACK ANIMATION 
    	if((Colours)EnemyPartsColours[0] == Colours.Black)
    	{
    		yield return StartCoroutine(HitEffectEnemy(0, EnemyPartsColours[0]));
    	}
    }

    IEnumerator PostAttackBuffAnimEnemy()
    {
    	yield return new WaitForSeconds(0.5f);
    	
    	// RUN "NON-ATTACK" ABILITY ANIMATION FOR ENEMY IF ANTENNA IS WHITE OR GREY RESPECTIVELY
    	// NOW SO THESE ANIMATIONS APPEAR AFTER THE ENEMY IS DONE ATTACKING 
    	if((Colours)EnemyPartsColours[0] == Colours.White)
    	{
    		enemyHealth += 2;
    		yield return StartCoroutine(HitEffectEnemy(2, EnemyPartsColours[0]));
    		//StartCoroutine(DisplayDamageText(2, EnemyDamageText, (Colours)EnemyPartsColours[0]));
    	}

    	if((Colours)EnemyPartsColours[0] == Colours.Grey)
    	{
    		yield return StartCoroutine(HitEffectEnemy(0, EnemyPartsColours[0]));
    	}

    	cursorLock = false;
    }

    IEnumerator EndGameDisplay(GameObject display)
    {

    	display.SetActive(true);

    	yield return new WaitForSeconds(3);

    	display.SetActive(false);

    	SceneManager.LoadScene("Mode Select Scene");

    }

    IEnumerator NextLevelDisplay(GameObject display)
    {

    	display.SetActive(true);

    	yield return new WaitForSeconds(3);

    	display.SetActive(false);

    	level++;
    	PlayerPrefs.SetInt("Level", level);

    	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    	
    }

    IEnumerator BackToModeSelect()
    {

    	yield return new WaitForSeconds(3);

    	SceneManager.LoadScene("Mode Select Scene");
    }

    void SeedParse(int[] seed)
    {

    	PartyColours[0] = seed[0];
    	PartyColours[1] = seed[1];
    	PartyColours[2] = seed[2];
    	PartyColours[3] = seed[3];

    	EnemyPartsColours = new int[7];
    	EnemyPartsColours[0] = seed[4];
    	EnemyPartsColours[1] = seed[5];
    	EnemyPartsColours[2] = seed[6];
    	EnemyPartsColours[3] = seed[7];
    	EnemyPartsColours[4] = seed[8];

    	EnemyKillSequence = new int[seed[9]];

    	for(int i = 0; i < seed[9]; i++)
    	{
    		EnemyKillSequence[i] = seed[10+i];
    	}

    	modelNum = seed[10+seed[9]];

    }

    void EnemyBuilder()
    {

    	EnemyParts[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyAntenna[Random.Range(0, EnemyAntenna.Length)];
    	
    	EnemyParts[0].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[0]);

		EnemyParts[0].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[0]];

		EnemyParts[0].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[0]);

    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyHead[Random.Range(0, EnemyHead.Length)];
    	
    	EnemyParts[1].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[1]);

		EnemyParts[1].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[1]];

		EnemyParts[1].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[1]);

		EnemyParts[1].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
			ModelNumber[modelNum];

		EnemyParts[1].transform.GetChild(2).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[1]);

    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyBody[Random.Range(0, EnemyBody.Length)];
    	
    	EnemyParts[2].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[2]);

		EnemyParts[2].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[2]];

		EnemyParts[2].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[2]);

		EnemyParts[2].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[2]];

		EnemyParts[2].transform.GetChild(2).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[2]);

    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];
    	
    	EnemyParts[3].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[3]);

		EnemyParts[3].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[3]];

		EnemyParts[3].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[3]);

    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyArm[Random.Range(0, EnemyArm.Length)];
    	
    	EnemyParts[4].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[4]);

		EnemyParts[4].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[4]];

		EnemyParts[4].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[4]);

    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyChest[Random.Range(0, EnemyChest.Length)];
    	
    	EnemyParts[5].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[5]);

		EnemyParts[5].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[5]];

		EnemyParts[5].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[5]);

    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = 
    		EnemyWeakness[Random.Range(0, EnemyWeakness.Length)];

    	EnemyParts[6].transform.GetChild(0).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[6]);

		EnemyParts[6].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite =
			Symbols[EnemyPartsColours[6]];

		EnemyParts[6].transform.GetChild(1).GetComponent<SpriteRenderer>().color =
			SetColour(EnemyPartsColours[6]);

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
				= new Color(0.125f, 0.125f, 0.125f, 1);
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
