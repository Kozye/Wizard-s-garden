using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : MonoBehaviour {

    public int health;
    public GameObject particleEfect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 1f;
    public float speed;
    public Sprite faceingUp;
    public Sprite faceingDown;
    public Sprite faceingLeft;
    public Sprite faceingRight;
    float changeTimer = .2f;
    bool shouldChange;



    // Use this for initialization
    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.Range(0, 3);
        shouldChange = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1.5f;
            direction = Random.Range(0, 3);
        }
        Movement();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
    }

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            spriteRenderer.sprite = faceingDown;
        }
        else if (direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime,0, 0);
            spriteRenderer.sprite = faceingLeft;

        }
        else if (direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = faceingRight;

        }
        else if (direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            spriteRenderer.sprite = faceingUp;

        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0)
            {
                Instantiate(particleEfect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
               
            col.GetComponent<Sword>().CreateParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PLayer>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PLayer>().canMove = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackDir", 5);

            Destroy(col.gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            health--;
            if (!col.gameObject.GetComponent<PLayer>().invincFrames)
            {
                col.gameObject.GetComponent<PLayer>().currentHealth--;
                col.gameObject.GetComponent<PLayer>().invincFrames = true;

            }

            if (health <= 0)
            {
                Instantiate(particleEfect, transform.position, transform.rotation);
                Destroy(gameObject);

            }
        }
        else if (col.gameObject.tag == "Wall")
        {
            if (shouldChange)
                return;

            if (direction == 0)
                direction = 3;
            else if (direction == 1)
                direction = 2;
            else if (direction == 2)
                direction = 1;
            else if (direction == 3)
                direction = 0;

            shouldChange = true;

        }
    }
}
