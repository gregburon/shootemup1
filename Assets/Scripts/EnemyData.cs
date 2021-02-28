using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyData : MonoBehaviour
{
    public int Health { get; private set; }

    public string Name { get; private set; }

    public SpriteRenderer SpriteRenderer;

    public void Initialize(string name, int health)
    {
        Health = health;
        Name = name;
        string path = "Enemies/" + Name;
        SpriteRenderer.sprite = Resources.Load<Sprite>(path);
    }

    void Start()
    {
	}
	
	void Update()
    {
	}

    void OnCollisionEnter2D(Collision2D obj)
    {
        UnityEngine.Debug.Log("OnCollisionEnter2D - I am " + this.gameObject.name + ", and " + obj.gameObject.name + " hit me");
    }

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
