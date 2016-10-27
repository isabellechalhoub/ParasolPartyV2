using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour 
{
	public Vector3 endPosition = Vector3.zero;
	public float speed = 2;

	private float timer = 0;
	private Vector3 startPosition = Vector3.zero;
	private bool outgoing = true;
	private BoxCollider2D player;
	private GameObject wind;
    private BoxCollider2D windColl;
    private bool touching;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<BoxCollider2D>();
		wind = gameObject;
        windColl = wind.GetComponent<BoxCollider2D>();

		startPosition = this.gameObject.transform.position;
		endPosition += startPosition;

		float distance = Vector3.Distance(startPosition, endPosition);
		if (distance != 0)
		{
			speed /= distance;
		}
        touching = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime * speed;
		bool go = false;
  //      if (windColl.IsTouching(player))
  //      {
  //          Debug.Log("touch");
		//	touching = true;
		//} else
		//	touching = false;

		if (outgoing && touching) 
		{
			Debug.Log ("go");
			go = true;
			this.transform.position = Vector3.Lerp(startPosition, endPosition, timer);
			if (timer > 1) 
			{
				outgoing = false;
				timer = 0;
			}
		} 
		else if (!touching && go)
		{
			this.transform.position = Vector3.Lerp(endPosition, startPosition, timer);
			if (timer > 1) 
			{
				outgoing = true;
				timer = 0;
			}
		}
	}

    void OnCollisionEnter2D (Collision2D coll)
    {
        Debug.Log("here");
        if (coll.gameObject.tag.Equals("Player"))
        {
            Debug.Log("collide");
            touching = true;
        }
    }


    void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, endPosition + this.transform.position);
	}
}
