using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    private static GameManager instance;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Text cointText;

    private int collectedCoins;

    [SerializeField]
    private GameObject knifePrefab;

    [SerializeField]
    private Text knifeText;

    private int collectedknife;

    [SerializeField]
    private Text enemyText;

    [SerializeField]
    private int collectedEnemyKilled;

    [SerializeField]
    private Text numberOfHealthText;

    private int numberOfHealth;

    public int NumberOfHealth
    {
        get
        {
            return numberOfHealth;
        }
        set
        {
            numberOfHealthText.text = value.ToString();
            this.numberOfHealth = value;
        }
    }
    public int CollectedEnemyKilled
    {
        get
        {
            return collectedEnemyKilled;
        }
        set
        {
            EnemyText.text = value.ToString();
            this.collectedEnemyKilled = value;
        }
    }


    public int CollectedCoins {
        get {
            return collectedCoins;
        }
        set {
            CointText.text = value.ToString();
            this.collectedCoins = value;
        }
    }

    public Text CointText {
        get {
            return cointText;
        }
        set {
            this.cointText = value;
        }
    }

    public Text EnemyText
    {
        get
        {
            return enemyText;
        }
        set
        {
            this.enemyText = value;
        }
    }

    public GameObject CoinPrefab
    {
        get {
            return coinPrefab;
        }
    }

    public int CollectedKnifes
    {
        get
        {
            return collectedknife;
        }
        set
        {
            knifeText.text = value.ToString();
            this.collectedknife = value;
        }
    }

    public Text KnifeText
    {
        get
        {
            return knifeText;
        }
        set
        {
            this.knifeText = value;
        }
    }

    public GameObject KnifePrefab
    {
        get
        {
            return knifePrefab;
        }
    }

    public static GameManager Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [SerializeField]
    private int numberThrow;

    private bool isGamePaused;

    public bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
        set
        {
            this.isGamePaused = value;
        }
    }

    // Use this for initialization
    void Start () {
        collectedEnemyKilled = 4;
        enemyText.text = collectedEnemyKilled.ToString();

        collectedknife = numberThrow;
        knifeText.text = numberThrow.ToString();

        numberOfHealth = 3;
        numberOfHealthText.text = numberThrow.ToString();

        isGamePaused = false;

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
