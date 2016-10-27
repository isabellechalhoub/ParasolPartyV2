using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
#region Vars
	public GameObject gameCamera;
	public GameObject healthBar;
	public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject winPanel;
	public float walkSpeed = 3;
	public float gravity = -35;
	public float jumpHeight = 2;
	public int health = 100;
	public BoxCollider2D coll;
	public BoxCollider2D enemy;
	public GameObject healthNum;
    public GameObject shield;
    public GameObject sword;

    private Vector3 swordStartPos;
    private Vector3 swordEndPos;
	private bool shieldin = false;
	private bool floatin = false;
	private bool playerControl = true;
	private int currHealth = 0;
	public CharacterController2D _controller;
	private AnimationController2D _animator;
    private bool swinging = false;
    private bool pause = false;
    private bool wind;
    #endregion

    void Start ()
    {
        shield = GameObject.FindGameObjectWithTag("Shield");
        sword = GameObject.FindGameObjectWithTag("Sword");
		coll = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D> ();
		enemy = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<BoxCollider2D> ();
		_controller = gameObject.GetComponent<CharacterController2D>();
		_animator = gameObject.GetComponent<AnimationController2D>();

		gameCamera.GetComponent<CameraFollow2D> ().startCameraFollow (this.gameObject);
		winPanel.SetActive(false);
		gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
		currHealth = health;
        _animator.setAnimation("Fall");
	}

	void Update ()
    {
		if (playerControl) 
		{
			Vector3 velocity = PlayerInput ();
			_controller.move (velocity * Time.deltaTime);
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if (pause)
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                playerControl = false;
                _animator.setAnimation("Idle");
            }
            else
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                playerControl = true;
            }
        }
	}
		
#region Movement
	private Vector3 PlayerInput()
	{
		Vector3 velocity = _controller.velocity;
		velocity.x = 0;

		#region moving platform parenting
		if (_controller.isGrounded && _controller.ground != null && (_controller.ground.tag.Equals("MovingPlatform") || _controller.ground.tag.Equals("Wind"))) 
		{
			this.transform.parent = _controller.ground.transform;
            if (_controller.ground.tag.Equals("Wind"))
            {
                _animator.setAnimation("Deploy");
                floatin = true;
                wind = true;
            }
		}
		else 
		{
			if (this.transform.parent != null)
				this.transform.parent = null;
		}
		#endregion

		#region running left/right
		// Left arrow key
		if (Input.GetAxis ("Horizontal") < 0 && !shieldin && !swinging)
		{
			velocity.x = -walkSpeed;
			if (_controller.isGrounded && !floatin) 
			{
				_animator.setAnimation ("Walk");
				_animator.setFacing ("Left");
			}
		}

		// Right arrow key
		else if (Input.GetAxis ("Horizontal") > 0 && !shieldin && !swinging) 
		{
			velocity.x = walkSpeed;
			if (_controller.isGrounded && !floatin) 
			{
				_animator.setAnimation ("Walk");
				_animator.setFacing ("Right");
			}
		}
		#endregion

		#region idle
		//Idle
		else 
		{
			if (_controller.isGrounded && currHealth != 0 && !shieldin && !swinging && !floatin) 
			{
				_animator.setAnimation("Idle");
			}
		}
		#endregion

		#region Jump/Float
		// Space bar - Jump
		if (Input.GetKeyDown (KeyCode.Space) && !shieldin && _controller.isGrounded && !swinging && !floatin) 
		{
			_animator.setAnimation("Jump");
			velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
		} 
		else if ((Input.GetKeyDown (KeyCode.Space) && !_controller.isGrounded) || floatin) 
		{
            _animator.setAnimation("Deploy");
            velocity.y = -2;
			floatin = true;
		}
		if (_controller.isGrounded || Input.GetKeyUp (KeyCode.Space))
		{
			if (!_controller.isGrounded)
			{
				_animator.setAnimation("Fall");
                wind = false;
			}
            else
            {
                //_animator.setAnimation("Land");
            }
            if (!wind)
            {
                floatin = false;
                gravity = -35;
            }
		}

        if (!_controller.isGrounded)
        {
            wind = false;
        }
        #endregion

        #region shield
        //Shield up and down
        //if (Input.GetAxis("Fire1") > 0) {
        //	shieldin = true;
        //} else
        //	shieldin = false;

        if (Input.GetKey(KeyCode.X) && !swinging)
        {
            _animator.setAnimation("Preblock");
            shieldin = true;
            shield.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.X) && shieldin)
        {
            _animator.setAnimation("Unblock");
            shieldin = false;
            shield.SetActive(false);
        }
        else
        {
            shieldin = false;
            shield.SetActive(false);
        }
        #endregion

        #region sword swing
        // swing dat sword bb
        if (Input.GetKey(KeyCode.C) && !shieldin)
        {
            _animator.setAnimation("Slash");
            swinging = true;
            sword.SetActive(true);
            //Transform pos = sword.GetComponent<Transform>();
            //swordStartPos = pos.localPosition;
            //Vector3 axis = new Vector3(pos.localPosition.x, pos.localPosition.y - pos.localPosition.sqrMagnitude, 0);
            //pos.RotateAround(axis, axis, 20 * Time.deltaTime);
        }
        else
        {
            swinging = false;
            sword.SetActive(false);
        }
        #endregion

        // Change velocity.
        velocity.y += gravity * Time.deltaTime;
		return velocity;
	}

//	void OnCollisionEnter(Collision col)
//	{
//		if (GetComponent<Collider>().GetComponent<Collider>().name == "Enemy")
//			PlayerDamage(5);
//	}

#endregion

#region Damage/Death/Winning
	// When the player collides with the death collider
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "KillZ")
			PlayerFallDeath ();
		else if (col.tag == "Damaging")
			PlayerDamage (10);
		else if (col.tag == "YouWin") 
            Winning();
		else if (col.tag == "Enemy" && (Input.GetKey (KeyCode.X) || Input.GetKey(KeyCode.C))) {}
		else if(col.tag == "Enemy")
			PlayerDamage (10);
	}
		
	private void Winning()
	{
		playerControl = false;
		_animator.setAnimation("Idle");
		winPanel.SetActive(true);
	}

	// Changes player health when damage is taken. checks for death
	private void PlayerDamage(int damage)
	{
		currHealth -= damage;
		float normHealth = (float)currHealth/(float)health;
		GameObject.Find ("Health").GetComponent<Text> ().text = currHealth.ToString();
		//healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normHealth*256, 32);

		if (currHealth <= 0)
			PlayerDeath();
	}

	// Play death animation
	private void PlayerDeath()
	{
		//_animator.setAnimation("Death");
		playerControl = false;
		gameOverPanel.SetActive(true);
	}

	// Stops the camera follow and reduces health
	private void PlayerFallDeath()
	{
		currHealth = 0;
		GameObject.Find ("Health").GetComponent<Text> ().text = currHealth.ToString();
		//healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 32);
		gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();
		gameOverPanel.SetActive(true);
	}
#endregion
}
