using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private static GameManager instance = null;

    public PlayerController playerController;
    public Transform backGroundList;
    public Transform floorList;
    public GameObject backGround;
    public GameObject[] groundType;
    public GameObject speedUpItem;
    public GameObject panel;
    public GameObject title;
    public GameObject gameOver;
    public Text timeCountText;
    public Text scoreText;
    public Text highScoreText;
    public Text lastScoreText;
    int currentScore;
    int highScore;

    public List<GameObject> floorList1 = new List<GameObject>();
    public List<GameObject> floorList2 = new List<GameObject>();
    public List<GameObject> floorList3 = new List<GameObject>();
    public List<GameObject> floorList4 = new List<GameObject>();
    public List<GameObject> floorList5 = new List<GameObject>();
    public List<GameObject> floorList6 = new List<GameObject>();
    public List<GameObject> bgList = new List<GameObject>();
    public List<List<GameObject>> floorListIndex = new List<List<GameObject>>();

    public static GameManager Instance {
        get { return instance; }
    }

    public void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {

        highScore = PlayerPrefs.GetInt("HighScore");
        SetScoreText();
        SetBackGroundList();
        SetFloorList(floorList1,0);
        SetFloorList(floorList2,1);
        SetFloorList(floorList3,2);
        SetFloorList(floorList4,3);
        SetFloorList(floorList5,4);
        SetFloorList(floorList6,5);
        floorListIndex.Add(floorList1);
        floorListIndex.Add(floorList2);
        floorListIndex.Add(floorList3);
        floorListIndex.Add(floorList4);
        floorListIndex.Add(floorList5);
        floorListIndex.Add(floorList6);
        title.SetActive(true);
        gameOver.SetActive(false);
        panel.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void StartGame() {
        title.SetActive(false);
        panel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        StartCoroutine("TimeCount");
    }

    IEnumerator TimeCount() {
        timeCountText.gameObject.SetActive(true);
        Debug.Log("In");
        int timer = 3;

        while(timer >= 1) {
            Debug.Log(timer);
            timeCountText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }
        timeCountText.gameObject.SetActive(false);
        playerController.enabled = true;
        Debug.Log("Out");
    }

    public void GameOver() {
        //highScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        lastScoreText.text = "Your Score: \n" + currentScore;
        PlayerPrefs.SetInt("HighScore",highScore);
        panel.SetActive(true);
        title.SetActive(false);
        gameOver.SetActive(true);
    }

    public void SetScore(int _value) {
        currentScore += _value;
        SetScoreText();
    }

    void SetScoreText() {
        scoreText.text = "SCORE: " + currentScore.ToString();
        if(currentScore > highScore) {
            highScoreText.text = "THE HIGH SCORE: " + currentScore.ToString();
            highScore = currentScore;
        } else {
            highScoreText.text = "THE HIGH SCORE: " + highScore.ToString();
        }
    }

    public void SetStage(float pos) {
        SetItem(pos);
        //SetFloor(pos);
        GameObject floorObj = GetFloor();
        //if(floorObj == null)
        //    return;
        floorObj.transform.position = new Vector2(pos,0f);
        floorObj.SetActive(true);

        GameObject bgObj = GetBackGround();
        if(bgObj == null)
            return;
        bgObj.transform.position = new Vector2(pos,0f);
        bgObj.SetActive(true);
    }

    void SetItem(float pos) {
        int i = Random.Range(0,10);
        if(i == 1) {
            GameObject item = Instantiate(speedUpItem,new Vector2(pos - 3f,0f),Quaternion.identity,backGroundList) as GameObject;
        }
    }

    void SetFloorList(List<GameObject> fList,int floorIndex) {
        //int i = Random.Range(0,groundType.Length);
        //GameObject floor = Instantiate(groundType[i],new Vector2(pos,0f),Quaternion.identity,floorList) as GameObject;
        for(int i = 0; i < 4; i++) {
            GameObject floor = Instantiate(groundType[floorIndex]) as GameObject;
            floor.transform.parent = floorList;
            floor.SetActive(false);
            fList.Add(floor);
        }
    }

    GameObject GetFloor() {
        int x = Random.Range(0,floorListIndex.Count);
        for(int i = 0; i < floorListIndex[x].Count; i++) {
            if(!floorListIndex[x][i].activeInHierarchy) {
                return floorListIndex[x][i];
            }
        }
        return null;
    }

    void SetBackGroundList() {
        for(int i = 0; i < 4; i++) {
            GameObject bg = Instantiate(backGround) as GameObject;
            bg.transform.parent = backGroundList;
            bg.SetActive(false);
            bgList.Add(bg);
        }
    }

    GameObject GetBackGround() {
        //GameObject bg = Instantiate(backGround,new Vector2(pos,0f),Quaternion.identity,backGroundList) as GameObject;
        for(int i = 0; i < bgList.Count; i++) {
            if(!bgList[i].activeInHierarchy) {
                return bgList[i];
            }
        }
        return null;
    }

    public void RestartScene() {
        SceneManager.LoadScene(0);
    }
}
