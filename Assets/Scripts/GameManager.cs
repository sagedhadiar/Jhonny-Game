using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Use this for initialization
    void Start () {

        collectedknife = numberThrow;
        knifeText.text = numberThrow.ToString();
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
