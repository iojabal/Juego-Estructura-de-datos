using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {
    public bool canSelect = false;
    public Image image;
    public Sprite unlockSprite;
    public Button bt;
    public GameObject levelNum;

    public GameObject[] stars;
    public int totalLevel = 6;
    string currentMap;
    // Start is called before the first frame update
    void Start() {
        currentMap = PlayerPrefs.GetString("CurrentMap");
        stars = new[]
        {
            transform.Find("Star_1").gameObject,
            transform.Find("Star_2").gameObject,
            transform.Find("Star_3").gameObject
        };
        bt = GetComponent<Button>();
        bt.enabled = false;
        image = GetComponent<Image>();
        if (transform.parent.GetChild(0).name == gameObject.name) {
            canSelect = true;
        }

        else {
            int lastLevelIndex = Convert.ToInt32(gameObject.name) - 1;
            //if (PlayerPrefs.GetInt("Level" + lastLevelIndex + "Pass") == 1) {
            //   canSelect = true;
            //}
            
            if (PlayerPrefs.GetInt("Level" + lastLevelIndex+"Of"+currentMap)>0) {
                canSelect = true;
            }
        }
        
        if (canSelect) {
            bt.enabled = true;
            image.sprite = unlockSprite;
            //levelNum.SetActive(true);
            transform.Find("Num").gameObject.SetActive(true);
            int count = PlayerPrefs.GetInt("Level"+gameObject.name+"Of"+currentMap);
            for (int i = 0; i < count; i++) {
                stars[i].SetActive(true);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //关卡选择存储
    public void Select() {
        if (canSelect) {
            
            PlayerPrefs.SetString("CurrentLevel","Level"+levelNum.GetComponent<Text>().text+ "Of"+currentMap);

            SceneManager.LoadScene(2);
        }
    }
}
