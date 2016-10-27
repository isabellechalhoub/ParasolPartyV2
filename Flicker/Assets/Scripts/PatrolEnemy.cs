using UnityEngine;
using System.Collections;
using Prime31;

public class PatrolEnemy : MonoBehaviour {

	public float speed = 1f;
	public float startingPos;
	public float endingPos;
	private float direction = -1f;
    private int health = 3;
    private float distance;
	private Vector2 walking;
	public BoxCollider2D player;
	public PolygonCollider2D shield;
    public BoxCollider2D enemy;
    public GameObject me;
    public PolygonCollider2D sword;

	void Start () 
	{
        distance = startingPos - endingPos;
		endingPos = transform.position.x - distance;
		startingPos = transform.position.x;
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<BoxCollider2D> ();
        shield = GameObject.FindGameObjectWithTag ("Shield").GetComponent<PolygonCollider2D> ();
        me = gameObject;
        enemy = me.GetComponent<BoxCollider2D>();
        sword = GameObject.FindGameObjectWithTag("Sword").GetComponent<PolygonCollider2D>();
    }

	// Update is called once per frame
	void Update () 
	{
        if (health == 0)
        {
            me.SetActive(false);
        }
        if (enemy.IsTouching(sword))
        {
            health--;
            direction *= -1f;
        }
		if (enemy.IsTouching (player) || enemy.IsTouching (shield))
		{
			if (direction == -1f)
				direction = 1f;
			else
				direction = -1f;
		}
		walking.x = direction * speed * Time.deltaTime;
		if (direction > 0f && transform.position.x >= startingPos) {
			direction = -1f;
		}
		else if(direction < 0f && transform.position.x <= endingPos) {
			direction = 1f;
		}
		transform.Translate (walking);
	}
}
