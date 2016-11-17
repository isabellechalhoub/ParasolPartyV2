using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public float range;
    public LayerMask playerLayer;
    public bool inRange;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        inRange = Physics2D.OverlapCircle(transform.position, range, playerLayer);

        if (inRange)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
	}

    void OnDrawGizmosSelected ()
    {
        Gizmos.DrawSphere(transform.position, range);
    }
}
