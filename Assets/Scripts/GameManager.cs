using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Lista<Bird> birds = new Lista<Bird>();
    public Lista<Pig> pigs = new Lista<Pig>();
    public Bird[] birds_list;
    public Pig[] pigs_list;
    public static GameManager instance;

    public Vector3 originPos;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public int birdCount;
    public int pigCount;
    public GameObject[] stars;
    public int starCount = 0;
    private int totalLevel=6;
    void Initialized() {
        
        for (int i = 0; i < birds.Count; i++) {
            if (i == 0) {
                birds.search(birds_list[i]).gameObject.transform.position = originPos;
                birds.search(birds_list[i]).onGround = false;
                birds.search(birds_list[i]).enabled = true;
                birds.search(birds_list[i]).sj2d.enabled = true;
            }
            else {
                birds.search(birds_list[i]).enabled = false;
                birds.search(birds_list[i]).sj2d.enabled = false;
            }
        }
    }
    void Awake()
    {
        for (int i = 0; i < birds_list.Length; i++)
        {
            birds.crearLista(birds_list[i]);
        }
        for (int i = 0; i < pigs_list.Length; i++)
        {
            pigs.crearLista(pigs_list[i]);
        }
        instance = this;

        if (birds_list.Length > 0)
        {
            originPos = birds_list[0].transform.position;
        }
    }
    // Start is called before the first frame update
    void Start() {
        Initialized();
        birdCount = birds.Count;
        pigCount = pigs.Count;
    }

    // Update is called once per frame
    void Update() {
    }

    public void NextBird() {    
        if (pigs.Count > 0) {
            
            if (birds.Count > 0) {
               

                Initialized();
                
            }
            else {
                losePanel.SetActive(true);
            }
        }
        else {
            //win
            winPanel.SetActive(true);
        }
    }

    public void DisplayStars() {
        StartCoroutine("Stars");
    }

    public IEnumerator Stars() {
        if (birds.Count==birdCount-pigCount) {
            starCount = 2;
        }
        else {
            starCount = birds.Count > birdCount - pigCount ? 3 : 1;
        }
        for (int i = 0; i < starCount; i++) {
            yield return new WaitForSeconds(0.5f);
            stars[i].SetActive(true);
        }
       
        DataSave();
    }
    
    public void DataSave() {
       
        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        string currentMap = PlayerPrefs.GetString("CurrentMap");
        //string index = currentLevel.Substring(5);
        //PlayerPrefs.SetInt("Level"+index+"Pass",1);

        int historyScore = PlayerPrefs.GetInt(currentLevel);
       
        if (historyScore<starCount) {
            PlayerPrefs.SetInt(currentLevel, starCount);
        }
        
        int sum = 0;
        for (int i = 1; i <= totalLevel; i++) {
            sum+=PlayerPrefs.GetInt("Level" + i+"Of"+currentMap);
        }
        //totalNumOfStarInMap1

        PlayerPrefs.SetInt("TotalNumOfStarsIn"+currentMap,sum);
    }

    public void Pause() {
        pausePanel.GetComponent<Animator>().SetBool("isPause",true);
    }

    public void Retry() {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void Home() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}
