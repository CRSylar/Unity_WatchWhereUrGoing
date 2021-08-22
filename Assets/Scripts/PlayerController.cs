using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject focalPoint;
    private Rigidbody playerRb;
    private float powerUpStrenght = 10.0f;

    public bool gameOver = false;
    public GameObject powerUpIndicator;
    public float speed = 5.0f;
    public bool hasPowerUp = false;

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
            hasPowerUp = true;
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdown());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 repulsion = collision.gameObject.transform.position;

            enemyRb.AddForce(repulsion * powerUpStrenght, ForceMode.Impulse);
        }
    }

    IEnumerator PowerUpCountdown()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerUpIndicator.gameObject.SetActive(false);
    }
}
