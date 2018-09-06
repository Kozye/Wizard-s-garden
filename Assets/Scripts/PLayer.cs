using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PLayer : MonoBehaviour
{

    public float speed;
    Animator _anim;
    public Image[] hearts;
    public int maxHelath;
    public int currentHealth;
    public GameObject deathParticle;
    public GameObject sword;
    public float thrustPower;
    public bool canMove;
    public bool canAttack;
    public bool invincFrames;
    SpriteRenderer sr;
    float invincTimer = 1f;



    // Use this for initialization
    void Start()
    {
        
        _anim = GetComponent<Animator>();
        if (PlayerPrefs.HasKey("maxHealth"))
        {
            LoadGame();

        }else
        currentHealth = maxHelath;
       
        canMove = true;
        canAttack = true;
        invincFrames = false;
        sr = GetComponent<SpriteRenderer>();
    }

    void GetHealth()
    {
        for (int i = 0; i <= hearts.Length-1; i++)
        {
            hearts[i].gameObject.SetActive(false);

        }


        for (int i = 0; i <= currentHealth-1; i++)
			{
            hearts[i].gameObject.SetActive(true);
			}                  
    }


    
    // Update is called once per frame
	void Update ()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathParticle, transform.position, transform.rotation);

            Destroy(this.gameObject);
            GameMenager.instance.GameOver();

           
            PlayerPrefs.DeleteAll(); 
        }

        Movement();
//Attack Input
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();

//ako dodavanje healtha preskoci max health
        if (currentHealth > maxHelath)
            currentHealth = maxHelath;
        if (invincFrames == true)
        {
            invincTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            if (rn < 50) sr.enabled = false;
            if (rn > 50) sr.enabled = true;

            if (invincTimer <= 0)
            {
                invincTimer = 1f;
                invincFrames = false;
                sr.enabled = true;
            }

        }
            GetHealth();
	}

    void Attack()
    {
        if (!canAttack)
            return;
        
        canMove = false;
        canAttack = false;
        thrustPower = 200;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation);
        if (currentHealth == maxHelath)
        {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 400;


        }
        
        
        
        #region //SwordRotation
        int swordDir = _anim.GetInteger("Dir");
        _anim.SetInteger("attackDir", swordDir);
        if (swordDir == 0)
        {

            newSword.transform.Rotate(0, 0, 180);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.down * thrustPower);

        }
        else if (swordDir == 1)
        {
            newSword.transform.Rotate(0, 0, 0);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);

        }
        else if (swordDir == 2)
        {
            newSword.transform.Rotate(0, 0, 90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.left * thrustPower);

        }
        else if (swordDir == 3)
        {
            newSword.transform.Rotate(0, 0, -90);
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);

        }
        #endregion


    }

    void Movement()
    {

        if (!canMove)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            _anim.SetInteger("Dir", 1);
            _anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            _anim.SetInteger("Dir", 0);
            _anim.speed = 1;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            _anim.SetInteger("Dir", 2);
            _anim.speed = 1;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            _anim.SetInteger("Dir", 3);
            _anim.speed = 1;

        }
        else
            _anim.speed = 0;
    }

     void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.gameObject.tag == "EnemyBullet")
        {
            if (!invincFrames)
            {
                invincFrames = true;
                currentHealth--;
            }
            col.gameObject.GetComponent<Bullet>().CreateParticle();
            Destroy(col.gameObject);

        }
        if (col.gameObject.tag == "Potion")
        {
            currentHealth = maxHelath;
            Destroy(col.gameObject);
            
            if (maxHelath >= 5)
                return;
            maxHelath++;
            currentHealth = maxHelath;
            
        }
    }
    public void SaveGame()
    {

        PlayerPrefs.SetInt("maxHealth", maxHelath);
        PlayerPrefs.SetInt("currentHealth", currentHealth);

    }
    void LoadGame()
    {
        maxHelath = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");

    }
    


}
