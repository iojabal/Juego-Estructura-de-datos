using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour {
    public int unlockStarNum;
    public bool canSelect;

    public GameObject stars;
    public GameObject locks;

    public GameObject panel;
    public GameObject map;

    public Button bt;

    public int mapNum;
    // Start is called before the first frame update
    void Start() {
        //PlayerPrefs.DeleteAll();

        string currentMap = PlayerPrefs.GetString("CurrentMap");
 
        transform.Find("Stars").transform.Find("Rate").GetComponent<Text>().text = PlayerPrefs.GetInt("TotalNumOfStarsIn" + transform.name) + "/ 18";

        bt = GetComponent<Button>();
        bt.enabled = false;

        if (transform.parent.GetChild(0).name==gameObject.name) {
            canSelect = true;
        }
        else {
            if (PlayerPrefs.GetInt("TotalNumOfStarsInMap" + (mapNum-1)) >= unlockStarNum) {
                canSelect = true;
            }
        }
        
        if (canSelect) {
            bt.enabled = true;
            locks.SetActive(false);
            stars.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select() {
        if (canSelect) {
            panel.SetActive(true);
            map.SetActive(false);
            PlayerPrefs.SetString("CurrentMap", "Map" + mapNum);
        }
    }
}
