using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnder : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void KillOnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
