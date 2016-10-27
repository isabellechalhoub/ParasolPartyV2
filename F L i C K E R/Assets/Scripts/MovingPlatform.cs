using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{
	public Vector3 endPosition = Vector3.zero;
	public float speed = 2;

	private float timer = 0;
	private Vector3 startPosition = Vector3.zero;
	private bool outgoing = true;
    private GameObject player;
    private PlayerController controller;

	// Use this for initialization
	void Start () 
	{
		startPosition = this.gameObject.transform.position;
		endPosition += startPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
		float distance = Vector3.Distance(startPosition, endPosition);
		if (distance != 0)
		{
			speed /= distance;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime * speed;

		if (outgoing) 
		{
			this.transform.position = Vector3.Lerp(startPosition, endPosition, timer);
			if (timer > 1) 
			{
				outgoing = false;
				timer = 0;
			}
		} 
		else 
		{
            if (this.gameObject.tag.Equals("MovingPlatform"))
            {
                this.transform.position = Vector3.Lerp(endPosition, startPosition, timer);
                if (timer > 1)
                {
                    outgoing = true;
                    timer = 0;
                }
            }
            else if (this.gameObject.tag.Equals("Wind"))
            {
                this.transform.position = new Vector2(startPosition.x, startPosition.y);
                //if (controller._controller.ground == this.GetComponent<BoxCollider2D>())
                //{
                //player.transform.parent = null;
                //controller._controller.ground = null;
                //}
                outgoing = true;
            }
		}
		
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, endPosition + this.transform.position);
	}
}
