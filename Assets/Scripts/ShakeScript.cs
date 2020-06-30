using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScript : MonoBehaviour
{
    
	public Animator camAnim;

	public void camShake(){

		int shakeSelect = Random.Range(0, 4);

		switch (shakeSelect)
		{
			case 0:
				camAnim.SetTrigger("Shake1");
				break;
			case 1:
				camAnim.SetTrigger("Shake2");
				break;
			case 2:
				camAnim.SetTrigger("Shake3");
				break;
			case 3:
				camAnim.SetTrigger("Shake4");
				break;
		}

	}

}
