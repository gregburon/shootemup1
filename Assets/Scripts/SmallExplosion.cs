using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplosion : MonoBehaviour
{
	void Start()
    {
	}

	void Update()
    {	
	}

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
