using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{

    TMP_Text scoreText;
    public static int scoreValue;
    public GameObject score25Duck; 
    public GameObject score50Duck;
    public GameObject score100Duck; 
    private int state;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<TMP_Text>();
        scoreValue = 0;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {scoreValue}"; 
        switch(state) {
            case 0: 
                if (scoreValue == 25) { 
                    Instantiate(score25Duck);
                    state++;
                }
                break;
            
            case 1:
                if (scoreValue == 50) {
                    Instantiate(score50Duck);
                    state++;
                }
                break;
            case 2:
                if (scoreValue == 100) {
                    Instantiate(score100Duck);
                    state++;
                }    
                break;            
            default:
                break;
        }
    }

}
