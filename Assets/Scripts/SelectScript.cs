using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScript : MonoBehaviour
{

	private int gameMode;
	private int level;

    // Start is called before the first frame update
    void Start()
    {

    	gameMode = 1;
    	level = 1;
    	PlayerPrefs.SetInt("GameMode", gameMode);
    	PlayerPrefs.SetInt("Level", level);

    }

    // Update is called once per frame
    void Update()
    {

	    if (Input.GetMouseButtonDown(0))
	    {

	    	Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

		    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
	    	
	        if (hit.collider != null) {
		    	
		    	if(hit.collider.gameObject.tag == "Story")
		    	{
		    		gameMode = 0;
		    		PlayerPrefs.SetInt("GameMode", gameMode);
		    		SceneManager.LoadScene("Battle Scene");
		    	}

		    	if(hit.collider.gameObject.tag == "Random")
		    	{
		    		gameMode = 1;
		    		PlayerPrefs.SetInt("GameMode", gameMode);
		    		SceneManager.LoadScene("Battle Scene");
		    	}

		    	if(hit.collider.gameObject.tag == "Challenge")
		    	{
		    		gameMode = 2;
		    		PlayerPrefs.SetInt("GameMode", gameMode);
		    		SceneManager.LoadScene("Battle Scene");
		    	}

		    	if(hit.collider.gameObject.tag == "Training")
		    	{
		    		gameMode = 3;
		    		PlayerPrefs.SetInt("GameMode", gameMode);
		    		SceneManager.LoadScene("Battle Scene");
		    	}

			}
		}

    }
}
