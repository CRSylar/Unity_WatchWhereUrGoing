using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject focalPoint;
    private Rigidbody playerRb;
    private float powerUpStrenght = 10.0f;
    private GameObject tmpProjectile;

    public bool gameOver = false;
    public GameObject powerUpIndicator;
    public GameObject projectile;
    public float speed = 5.0f;
    public bool pushAway = false;
    public bool jumpSlam = false;
    public bool shotFire = false;
    public float hangTime;
    public float smashSpeed;
    public float explosionRadius;
    public float explosionForce;

    bool smashing = false;
    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("FocalPoint");
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {

            float forwardInput = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

            powerUpIndicator.transform.position = transform.position;

            if ( jumpSlam && !smashing && Input.GetKeyDown( KeyCode.Space ) )
            {
                smashing = true;
                StartCoroutine( Smash() );
            }

            if (shotFire && Input.GetKeyDown(KeyCode.V))
			{
                LaunchRockets();
			}

            if ( transform.position.y < -10 )
            {
                gameOver = true;
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                transform.position = new Vector3( 0, 0, 0 );
                Debug.Log( "Game Over !" );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            pushAway = true;
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdown());
        }
        else if ( other.CompareTag( "JumpSlam" ) )
		{
            jumpSlam = true;
            powerUpIndicator.gameObject.SetActive( true );
            Destroy( other.gameObject );
            StartCoroutine( PowerUpCountdown() );
        }
        else if ( other.CompareTag( "ShotFire" ) )
        {
            shotFire = true;
            powerUpIndicator.gameObject.SetActive( true );
            Destroy( other.gameObject );
            StartCoroutine( PowerUpCountdown() );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && pushAway )
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 repulsion = (transform.position - enemyRb.transform.position ).normalized;

            enemyRb.AddForce(-repulsion * powerUpStrenght, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
	{
        foreach(var enemy in FindObjectsOfType<Enemy>())
		{
            tmpProjectile = Instantiate( projectile, transform.position + Vector3.up, Quaternion.identity );
            tmpProjectile.GetComponent<Projectile>().Fire( enemy.transform );
        }
    }

    IEnumerator PowerUpCountdown()
    {
        yield return new WaitForSeconds(7);
        pushAway = false;
        jumpSlam = false;
        shotFire = false;
        powerUpIndicator.gameObject.SetActive(false);
    }

    IEnumerator Smash()
	{
        var enemies = FindObjectsOfType<Enemy>();

        // Store the y position before taking off
        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;

        // make the ball "Jump up"
        while (Time.time < jumpTime)
		{
            playerRb.velocity = new Vector2( playerRb.velocity.x, smashSpeed );
            yield return null;
		}
        // And get Down
        while (transform.position.y > floorY)
		{
            playerRb.velocity = new Vector2( playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        for ( int i = 0; i < enemies.Length; i++ )
		{
            if ( enemies[i] != null )
			{
                enemies[i].GetComponent<Rigidbody>()
                    .AddExplosionForce( explosionForce,
                        transform.position,
                            explosionRadius,
                                0.0f,
                                    ForceMode.Impulse );
			}
		}

        smashing = false;
    }
}
