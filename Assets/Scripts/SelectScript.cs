using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScript : MonoBehaviour
{

	public Sprite[] SideTrim;

	public AudioClip[] SelectionSounds;

	public GameObject PreTutorial;
	public GameObject PostTutorial;

	private int gameMode;
	private int level;
	private GameObject SideTrimObject;
	private GameObject SideTrimObjectFlip;

	private AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {

    	gameMode = 1;
    	level = 1;
    	PlayerPrefs.SetInt("GameMode", gameMode);
    	PlayerPrefs.SetInt("Level", level);

    	SideTrimObject = new GameObject("SideTrimLeft");
		SideTrimObject.AddComponent<SpriteRenderer>();
		SideTrimObject.transform.position = new Vector3(-5.5f, -0.5f, 0f);
		SideTrimObject.SetActive(false);
		SideTrimObject.tag = "SideTrim";

		SideTrimObjectFlip = new GameObject("SideTrimRight");
		SideTrimObjectFlip.AddComponent<SpriteRenderer>();
		SideTrimObjectFlip.transform.position = new Vector3(5.5f, -0.5f, 0f);
		SideTrimObjectFlip.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		SideTrimObjectFlip.SetActive(false);
		SideTrimObjectFlip.tag = "SideTrim";

		audioPlayer = this.GetComponent<AudioSource>();

		GameObject.FindGameObjectWithTag("Music").GetComponent<MusicScript>().StopMusic();

    }

    // Update is called once per frame
    void Update()
    {

    	Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

	    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

	    if(PlayerPrefs.GetInt("TutorialComplete") == 1)
	    {
	    	PreTutorial.SetActive(false);
	    	PostTutorial.SetActive(true);
	    } else {
	    	PreTutorial.SetActive(true);
	    	PostTutorial.SetActive(false);
	    }
    	
        if (hit.collider != null) {

        	SpriteRenderer renderer = SideTrimObject.GetComponent<SpriteRenderer>();
        	SpriteRenderer rendererFlip = SideTrimObjectFlip.GetComponent<SpriteRenderer>();
		    	
	    	if(hit.collider.gameObject.tag == "Story")
	    	{

	    		renderer.sprite = SideTrim[0];
	    		rendererFlip.sprite = SideTrim[0];

	    		if (Input.GetMouseButtonDown(0))
	    		{
		    		gameMode = 0;
		    		audioPlayer.PlayOneShot(SelectionSounds[0]);
		    		StartCoroutine(Selection(gameMode));
		    	}
	    	}

	    	if(hit.collider.gameObject.tag == "Random")
	    	{

	    		renderer.sprite = SideTrim[1];
	    		rendererFlip.sprite = SideTrim[1];

	    		if (Input.GetMouseButtonDown(0))
	    		{
	    			gameMode = 1;
	    			audioPlayer.PlayOneShot(SelectionSounds[0]);
		    		StartCoroutine(Selection(gameMode));
		    	}
	    	}

	    	if(hit.collider.gameObject.tag == "Challenge")
	    	{

	    		renderer.sprite = SideTrim[2];
	    		rendererFlip.sprite = SideTrim[2];

	    		if (Input.GetMouseButtonDown(0))
	    		{
	    			gameMode = 2;
	    			audioPlayer.PlayOneShot(SelectionSounds[0]);
		    		StartCoroutine(Selection(gameMode));
		    	}
	    	}

	    	if(hit.collider.gameObject.tag == "Training")
	    	{

	    		renderer.sprite = SideTrim[3];
	    		rendererFlip.sprite = SideTrim[3];

	    		if (Input.GetMouseButtonDown(0))
	    		{
		    		gameMode = 3;
		    		audioPlayer.PlayOneShot(SelectionSounds[0]);
		    		StartCoroutine(Selection(gameMode));
		    	}
	    	}

	    	if(hit.collider.gameObject.tag == "Quit")
	    	{

	    		renderer.sprite = SideTrim[4];
	    		rendererFlip.sprite = SideTrim[4];

	    		if (Input.GetMouseButtonDown(0))
	    		{
	    			audioPlayer.PlayOneShot(SelectionSounds[0]);
		    		Application.Quit();
		    	}
	    	}

			SideTrimObject.SetActive(true);
			SideTrimObjectFlip.SetActive(true);

		} else {

			GameObject[] sideTrims = GameObject.FindGameObjectsWithTag("SideTrim");
   			foreach(GameObject sidetrim in sideTrims)
   			sidetrim.SetActive(false);

		}

    }

    IEnumerator Selection(int selection)
    {

    	audioPlayer.PlayOneShot(SelectionSounds[selection+1]);
    	yield return new WaitForSeconds(
    		SelectionSounds[selection+1].length);
    	PlayerPrefs.SetInt("GameMode", selection);
		SceneManager.LoadScene("Battle Scene");

    }
}
