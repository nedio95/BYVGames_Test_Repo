using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //----Overhead----
    private bool GameOver = false;
    public GameObject pref_bullet;
    public GameObject[] m_player;
    private int[] PlayerHealth = new int[2] { 5, 5 };

    private List<GameObject> list_bullets = new List<GameObject>();

    private float m_reloadTime = 1f;
    private float[] m_reloadPlayer = new float[2] { 0f, 0f };

    public GameObject GameOverMenu;
    public Text txt_gameOver;

    private string[] str_playerWon = { "Left", "Right" };
    private string str_gameOver = " player won the game!";

    private bool m_activeBonus = false;
    private float m_bonusRespawnTimeMax = 10f;
    private float m_bonusRespawnTimeCurrent = 10f;
    public List<GameObject> Bonuses;
    private Vector3 m_bonusSpawnPos = new Vector3(0, 10f, 0); 


    public bool IsGameOver()
    {
        return GameOver;
    }

    void Start()
    {
    }

    public void Shoot(GameObject gun)
    {
        if (GameOver) return;
               
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

        //Touch Input Manager
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < 0)
            {
                ActivateBullet(touch.position, Quaternion.identity);
            }

        }

        // Player reload time
        for (int i = 0; i < 2; i++)
        {
            if (m_reloadPlayer[i] > 0) m_reloadPlayer[i] -= Time.deltaTime;

        }

        //Bonus spawn
        if (!m_activeBonus && m_bonusRespawnTimeCurrent < 0f)
        {
            //Spawn bonus
            //m_activeBonus = true;
            //int r = Random.Range(0, 4);
            //Bonuses[r].SetActive(true);
            //Bonuses[r].transform.position = m_bonusSpawnPos;
            //Bonuses[r].transform.rotation = Quaternion.identity;
            //Bonuses[r].GetComponent<BonusesScript>().DeactivateBonus();
            
        }
        else if (!m_activeBonus)
        {
            //reduce cooldown
            m_bonusRespawnTimeCurrent -= Time.deltaTime;
        }
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
        //Not a good solutuion but it works for a quick fix
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
        m_player[num].GetComponent<PlayerController>().ManageHealthSprites(PlayerHealth[num]);
        if (PlayerHealth[num] < 1)
        {
            //Player num is dead
            Debug.Log("Murder call the police !");
            GameOver = true;
            PlayerGotKilledByBullet(num);
        }
    }

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
        str_gameOver = str_playerWon[pNum] + str_gameOver;

        GameOverMenu.SetActive(true);
        txt_gameOver.text = str_gameOver;
        
    }

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
}
