using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    //----Overhead----
    public GameObject TheGameManager;
    private GameManager TheGameManagerScript;
    public GameObject pref_blood;
    public int PlayerNumber;

    public int DamageToDeal;
    private string m_bulletName = "Bullet(Clone)";

    private Animator Anim;
    private bool m_isJumping = false;
    private bool m_canDoubleJump = true;

    private float m_addJumpHeight = 3.5f;
    private float m_currentJumpHeight = 0f;
    private float m_jumpSpeed = 7.0f;
    private float m_fallSpeed = -14f;
    private float m_currentSpeed;
    private Vector3 m_groundPos;
    private float m_groundY;

    private AudioSource m_audSrs;
    public AudioClip aud_Grunt;

    public List<GameObject> HealthSprites;

    void Start()
    {
        m_audSrs = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        TheGameManagerScript = TheGameManager.GetComponent<GameManager>();

        m_groundPos = transform.position;
        m_groundY = m_groundPos.y;
    }

    void Update()
    {
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

        Debug.Log("SomethingGotHit");
        GameObject obj = col.gameObject;
        if (obj.name == m_bulletName)
        {
            Transform ttrans = col.gameObject.transform;
            GotHit(DamageToDeal, ttrans);
        }
    } //ttrans.rotation

    public void GotHit(int dmg, Transform trans)
    {
        if (TheGameManagerScript.IsGameOver()) return;    

        TheGameManagerScript.PlayerGotShot(dmg, PlayerNumber);
        //vvvv This fixes/avoids a bug with the hinge joints vvvv
        //Not a good solutuion but it works for a quick fix
        float angleOfSplatter = 90f;
        if (trans.position.x > 0) angleOfSplatter += 180f; 
        //^^^^ ---- ^^^^
        GameObject blood = Instantiate(pref_blood, trans.position, transform.rotation * Quaternion.Euler(0f, angleOfSplatter, 0f));
        m_audSrs.PlayOneShot(aud_Grunt, 0.5f);
    }
    

    public void Jump()
    {
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
            m_canDoubleJump = false;
            Anim.SetBool("IsJumping", true);
            AddJumpTime();
        }
    }

    private void AddJumpTime()
    {
        m_currentJumpHeight += m_addJumpHeight;
        m_currentSpeed = m_jumpSpeed;
    }

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
