using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserProjectile : Projectile
{
    public GameObject SmallExplosionAnimation;

	public void Start()
    {
		
	}
	
	public void Update()
    {
		
	}

    void OnCollisionEnter2D(Collision2D obj)
    {
        UnityEngine.Debug.Log("OnCollisionEnter2D - I am " + this.gameObject.name + ", and " + obj.gameObject.name + " hit me");

        // Whenever a laser hits anything (unless it's REFLECTIVE???) it gets destroyed.
        Destroy(gameObject);

        RectTransform rt = (RectTransform)SmallExplosionAnimation.transform;
        float width = rt.rect.width;
        float height = rt.rect.height;
        Vector3 sizeDelta = new Vector3(width / 2f, height / 2f, 0f);

        Rigidbody2D explosionInstance;
        explosionInstance = Instantiate(SmallExplosionAnimation.GetComponent<Rigidbody2D>(), transform.position + sizeDelta, transform.rotation) as Rigidbody2D;
    }

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
