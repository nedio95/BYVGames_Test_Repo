using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    //vvvv SETUP vvv
    private string m_bulletName = "Bullet(Clone)"; // The name of the bullet objects
    private float m_addJumpHeight = 3.5f; // setup Jump height to be added on jump
    private float m_jumpSpeed = 7.0f; //setup Speed of jump up
    private float m_fallSpeed = -12f; //seetup Speed of fall after jump
    //^^^^----^^^^

    //vvvv Unity Editor Setup vvvv
    public int DamageToDeal; // This is how much damage is done when this part is hit
    public GameObject TheGameManager; // Reference to the GameManager
    private GameManager TheGameManagerScript; //Reference to the GameMaanger script to reduce function calls
    public GameObject pref_blood; // Reference to the bloo splatter prefab
    public int PlayerNumber; // The number of the player that is controlled by the instance of the script
    public List<GameObject> HealthSprites; //List of the UI health sprites
    public AudioClip aud_Grunt; //The sound clip of the player scream
    //^^^^^----^^^^

    //----Overhead----
    private bool m_isJumping = false; // Jumpstate
    private bool m_canDoubleJump = true; //Doublejump availability    
    private float m_currentJumpHeight = 0f; //dynamic target height for jump
    private float m_currentSpeed; // The current speed of a jump
    private Vector3 m_groundPos; //Save of the grounded position of the player object; Target for after a jump is completed
    private float m_groundY; // The height of the ground referenced by the player
    private Animator Anim; //This is the controlling animator
    private AudioSource m_audSrs; //Audio source
    //^^^^----^^^^


    void Start()
    {   
        //Setup of variables
        m_audSrs = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        TheGameManagerScript = TheGameManager.GetComponent<GameManager>();

        m_groundPos = transform.position;
        m_groundY = m_groundPos.y;
    }

    void Update()
    {
        // Control of jump; While the player is made out of Kinematic Rigidbodies they are not affected by physics
        //as such the jump has to be simulated
        if (m_isJumping)
        {
            if (transform.position.y >= m_currentJumpHeight)
            {
                m_currentSpeed = m_fallSpeed;
                Anim.SetBool("IsJumping", false);
            }

            transform.Translate(Vector3.up * m_currentSpeed * Time.smoothDeltaTime);

            if (transform.position.y <= m_groundY)
            {
                m_isJumping = false;
                m_canDoubleJump = true;
                Anim.SetBool("IsJumping", false);
                transform.position = m_groundPos;
            }
        }
    }
   
    void OnCollisionEnter2D(Collision2D col)
    {
        if(TheGameManagerScript.IsGameOver()) return;

        GameObject obj = col.gameObject;

        //When teh player gets hit by a nullet the position of the hit and the damage amount is passed to  different function
        if (obj.name == m_bulletName)
        {
            Transform ttrans = col.gameObject.transform;
            GotHit(DamageToDeal, ttrans);
        }
    }

    //This is called when the plaeyr is hit by a buller; 
    //it is public so it can be called from teh joints
    public void GotHit(int dmg, Transform trans)
    {
        if (TheGameManagerScript.IsGameOver()) return;    

        TheGameManagerScript.PlayerGotShot(dmg, PlayerNumber); //The game manager deals with the health and damage
        //vvvv This fixes/avoids a bug with the hinge joints vvvv
        //Not a good solutuion but it works for a quick fix
        float angleOfSplatter = 90f;
        if (trans.position.x > 0) angleOfSplatter += 180f; 
        //^^^^ ---- ^^^^
        //Creates the visual and sound effects
        GameObject blood = Instantiate(pref_blood, trans.position, transform.rotation * Quaternion.Euler(0f, angleOfSplatter, 0f));
        m_audSrs.PlayOneShot(aud_Grunt, 0.5f);
    }
    

    public void Jump()
    {
        //This manages and sets up jump requests; Called by GameManager
        Debug.Log("Jump");
        if (!m_isJumping)
        {
            m_currentJumpHeight = m_groundY;
            m_isJumping = true;
            AddJumpTime();
            Anim.SetBool("IsJumping", true);
            
        }
        else if (m_canDoubleJump)
        {
            // BUG: This would set the jump height to the max jump height regardless how much the first jump step has progressed
            // FIX?: Change the AddJumpTime to check if teh layer is falling and if so to work with current object height instead of m_currentJumpHeight
            m_canDoubleJump = false;
            Anim.SetBool("IsJumping", true);
            AddJumpTime();
        }
    }

    //Adds jump time on request; In case something else would like to add some jump time ? Avoids minor ducplication of code
    private void AddJumpTime()
    {
        m_currentJumpHeight += m_addJumpHeight;
        m_currentSpeed = m_jumpSpeed;
    }

    // Manages the UI health; Called from GameManager
    public void ManageHealthSprites(int hp)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i >= hp)
            {
                HealthSprites[i].SetActive(false);
            }
            else
            {
                HealthSprites[i].SetActive(true);
            }
        }
    }
}
