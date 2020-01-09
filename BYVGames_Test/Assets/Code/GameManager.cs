using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //vvvv SETUP vvvv
    private int[] PlayerHealth = new int[2] { 5, 5 };
    private int m_playerMaxHealth = 5;
    private float m_reloadTime = 1f;
    private float[] m_reloadPlayer = new float[2] { 0f, 0f };
    private string[] str_playerWon = { "Left", "Right" };
    private string str_gameOver = " player won the game!";
    private float m_bonusRespawnTimeMax = 10f;
    private float m_bonusRespawnTimeCurrent = 10f;
    //^^^^----^^^^

    //vvvv Unity Editor SETUP vvvv
    public GameObject pref_bullet;
    public GameObject[] m_player;
    public GameObject GameOverMenu;
    public Text txt_gameOver;
    public List<GameObject> Bonuses;
    //^^^^----^^^^

    //----Overhead----
    private bool GameOver = false;
    private List<GameObject> list_bullets = new List<GameObject>();
    private bool m_activeBonus = false;  
    //^^^^----^^^^

    //Public call for other objects to check if the game is over; This should be static...
    public bool IsGameOver()
    {
        return GameOver;
    }

    //Shoots a bullet from a players'gun
    public void Shoot(GameObject gun)
    {
        if (GameOver) return;

        //Bullets are pooled; First take some variables requred for bullet instantiation e.g. position and rotation
        Vector3 pos = gun.transform.position;
        int whichPlayer = 0;
        if (pos.x > 0) whichPlayer = 1;
        if (m_reloadPlayer[whichPlayer] > 0f)
            return;
        else
            m_reloadPlayer[whichPlayer] = m_reloadTime;
        
        Quaternion rot = gun.transform.rotation;

        ActivateBullet(pos, rot);
    }

    void Update()
    {
        if (GameOver) return;

        InputManager();

        PlayerManagement();

        BonusesManagement();
    }

    //Manages mouse clicks and touches
    void InputManager()
    { 
#if UNITY_EDITOR
        ///Mouse Testing 
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int playerNum = 0;
            if (pos.x > 0) playerNum = 1; //This test scene allows for the position to be used to check which player is affected
            m_player[playerNum].GetComponent<PlayerController>().Jump();
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        //Touch Input Manager
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < 0)
            {
                ActivateBullet(touch.position, Quaternion.identity);
            }

        }
#endif
    }

    void PlayerManagement()
    {
        // Player reload time
        for (int i = 0; i < 2; i++)
        {
            if (m_reloadPlayer[i] > 0) m_reloadPlayer[i] -= Time.deltaTime;

        }
    }

    void BonusesManagement()
    {
        //Bonus spawn
        if (!m_activeBonus && m_bonusRespawnTimeCurrent < 0f)
        {
            //Spawn bonus
            m_activeBonus = true;
            int r = Random.Range(0, Bonuses.Count);
            Bonuses[r].GetComponent<BonusesScript>().ResetBonus();
        }
        else if (!m_activeBonus)
        {
            //reduce cooldown
            m_bonusRespawnTimeCurrent -= Time.deltaTime;
        }
    }

    public void SetReadyForNewBonus()
    {
        m_bonusRespawnTimeCurrent = m_bonusRespawnTimeMax;
        m_activeBonus = false;
    }

    //---BULLET MANAGEMENT----


    public void BulletNeeded(Vector3 pos, Quaternion rot)
    {
        if (GameOver) return;

        ActivateBullet(pos, rot);
    }
    private void ActivateBullet(Vector3 pos, Quaternion rot)
    {
        if (GameOver) return;

        GameObject activateThisBullet = null;

        if (list_bullets.Count == 0) CreateNewBullet();

        //This can be reduced by putting active bullets at the front of the list and inactive bullets at the back
        //Code would do a single check for an inactive bullet at the back and if none is found create a new one
        //instead of looking through the whole list; Would be more efficient and would requre less processingtime in a single moment 
        // if(gotTime) fixThis;
        bool needNewBullet = true;
        for (int i = 0; i < list_bullets.Count; i++)
        {
            if (list_bullets[i].active == false)
            {
                activateThisBullet = list_bullets[i];
                needNewBullet = false;
            }
        }

        if (needNewBullet == true)
            activateThisBullet = CreateNewBullet();

        activateThisBullet.transform.position = pos;
        //vvvv This fixes a bug with the hinge joints vvvv
        //Not a good solutuion but it works for a cheap dirty quick fix
        if (pos.x > 0) rot.y = 180f;
        // ^^^^ ---- ^^^^
        activateThisBullet.transform.rotation = rot;
        activateThisBullet.SetActive(true);
    }

    private GameObject CreateNewBullet()
    {
        GameObject obj = Instantiate(pref_bullet, new Vector3(0, 0, 0), Quaternion.identity);
        obj.SetActive(false);
        list_bullets.Add(obj);
        return obj;
    }

    public void PlayerGotShot(int dmg, int num)
    {
        if (GameOver) return;

        PlayerHealth[num] -= dmg;
        if (PlayerHealth[num] > m_playerMaxHealth) PlayerHealth[num] = m_playerMaxHealth; // This prevents overheal
        m_player[num].GetComponent<PlayerController>().ManageHealthSprites(PlayerHealth[num]);
        if (PlayerHealth[num] < 1)
        {
            //Player num is dead
            Debug.Log("Murder call the police !");
            GameOver = true;
            PlayerGotKilledByBullet(num);
        }
    }

    //In case something else need player HP
    public int GetPlayerHP(int num)
    {
        return PlayerHealth[num];
    }

    void PlayerGotKilledByBullet(int pNum)
    {
        //Disable the animator that is controlling the legs
        m_player[pNum].GetComponent<Animator>().enabled = false;

        //Change rigidbody type to Dynamic so if is physics controlled rather than animated
        m_player[pNum].transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //Does the same for all children
        Rigidbody2D[] allChildren = m_player[pNum].GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D chil in allChildren)
        {
            chil.bodyType = RigidbodyType2D.Dynamic;
        }
        //m_player[pNum].GetComponent<Rigidbody2D>().AddForce(-transform.right * 100f);

        
        GameOverSequence(pNum);
    }

    void GameOverSequence(int pNum)
    {
        //Changes game over text and activates menu
        str_gameOver = str_playerWon[pNum] + str_gameOver;

        GameOverMenu.SetActive(true);
        txt_gameOver.text = str_gameOver;
    }

    //Scene manager
    public void LoadScene(int sNum)
    {
        switch (sNum)
        {
            case 0:
                SceneManager.LoadScene(sNum, LoadSceneMode.Single);
                break;
            case 1:
                SceneManager.LoadScene(sNum, LoadSceneMode.Single);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); or SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //This seems to be more cumbersome; Maybe if I am to expect an unknown index of a scene...
                break;
            default:
                break;
        }
    }

    public void PlayerHasBeenExploded(GameObject expPlayer)
    { 
        //When a player explodes
    }
}
