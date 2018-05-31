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

    public static GameManager Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
