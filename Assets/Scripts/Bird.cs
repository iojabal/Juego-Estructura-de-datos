using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour {
    public Vector3 currPos;
    public bool isClick;
    public Vector3 launchPos;
    public float maxDis;
    public SpriteRenderer sr;
    public SpringJoint2D sj2d;
    public Rigidbody2D r2d;

    public LineRenderer lrRight;
    public LineRenderer lrLeft;
    public Transform rightPos;
    public Transform leftPos;
    public GameObject boom;
    public bool isFly;
    public bool canMove = true;

    public WeaponTrail trail;
    public float smooth = 3;

    public AudioClip select;
    public AudioClip fly;
    public Sprite yellowSpeedUp;
    public bool launch = false;

    public Sprite redHurt;
    public Sprite yellowHurt;
    public Sprite greenHurt;
    public Sprite blackHurt;
    public bool onGround = true;
    public enum BirdType {
        Red, Yellow, Black, Green
    }
    public BirdType bt;
    public bool isReleased ;
    
    public void Awake() {
        isReleased = false;
        onGround = true;
        sr = GetComponent<SpriteRenderer>();
        trail = GetComponent<Trails>().trail;
        sj2d = GetComponent<SpringJoint2D>();
        r2d = GetComponent<Rigidbody2D>();
        sj2d.connectedBody = GameObject.Find("Right").GetComponent<Rigidbody2D>();
        lrRight = GameObject.Find("Right").GetComponent<LineRenderer>();
        lrLeft = GameObject.Find("Left").GetComponent<LineRenderer>();
        rightPos = GameObject.Find("RightPos").transform;
        leftPos = GameObject.Find("LeftPos").transform;

    }
    // Start is called before the first frame update
    public void Start() {
        isFly = false;
        isClick = false;
        launchPos = GameObject.Find("LaunchPos").transform.position;
        
        trail.SetTime(0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update() {
        
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        currPos = transform.position;
        if (isClick) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将鼠标坐标转化到屏幕空间坐标系
            
            //transform.position = new Vector3(transform.position.x,transform.position.y,0);
            
            transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);

            
            if (Vector3.Distance(transform.position, launchPos) > maxDis) {
                
                Vector3 dir = (transform.position - launchPos).normalized;
                dir *= maxDis;
                transform.position = dir + launchPos;
            }
            
            DrawLine();
        }
        
        float posX = transform.position.x;
        //Debug.Log(transform.name+" "+posX);
        
        Vector3 tarPos = new Vector3(Mathf.Clamp(posX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, tarPos, Time.deltaTime * smooth);


        if (isFly && Input.GetMouseButtonDown(0)) {
            if (bt == BirdType.Yellow) {
                DirectionalSpeedUpSkill();
            }
            if (bt == BirdType.Green) {
                BoomerangSkill();
            }
            if (bt == BirdType.Black) {
                BoomSkill();
            }
        }
    }

    public virtual void BoomSkill() {

    }

    public void BoomerangSkill() {
        isFly = false;
        GameObject bo = Instantiate(boom, transform.position, Quaternion.identity);
        bo.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        r2d.velocity = new Vector2(-r2d.velocity.x * 1.5f, r2d.velocity.y * 0.5f);
    }


    public void DirectionalSpeedUpSkill() {
        isFly = false;
        GameObject bo = Instantiate(boom, transform.position, Quaternion.identity);
        bo.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        sr.sprite = yellowSpeedUp;
        r2d.velocity *= 2.2f;
    }

    public void OnMouseDown() {
        if (Input.GetMouseButtonDown(0) && onGround == false) {
            AudioPlay(@select);
            if (canMove) {
                isClick = true;

                r2d.isKinematic = true;
            }
        }
    }

    public void OnMouseUp() {

        if (Input.GetMouseButtonUp(0) && onGround == false) {
            launch = true;
            if (canMove) {
                isClick = false;

                r2d.isKinematic = false;

                Invoke("Fly", 0.1f);


                lrRight.enabled = false;
                lrLeft.enabled = false;
                canMove = false;

                //Debug.Log(currTime);
            }
        }



    }

    public void Fly() {
        isReleased = true;
        AudioPlay(fly);

        isFly = true;
     
        trail.SetTime(0.2f, 0.0f, 1.0f);

        trail.StartTrail(0.5f, 0.4f);

        
        sj2d.enabled = false;

    }
    
    public void DrawLine() {

        
        lrRight.enabled = true;
        lrLeft.enabled = true;

        lrRight.SetPositions(new[] { rightPos.position, transform.position });
        lrLeft.SetPositions(new[] { leftPos.position, transform.position });
        //lrRight.SetPosition(0,rightPos.position);
        //lrRight.SetPosition(1,transform.position);
        //lrLeft.SetPosition(0,leftPos.position);
        //lrLeft.SetPosition(1,transform.position);
    }

  
    public void DestroyMyself() {
        int index = Array.IndexOf(GameManager.instance.birds_list,this);
        GameManager.instance.birds.deleteNode(this);
        GameManager.instance.birds_list = GameManager.instance.birds_list.Where((e, i) => i != index).ToArray();
        Instantiate(boom, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

        GameManager.instance.NextBird();
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        trail.ClearTrail();
        if (collision.transform.tag == "Enemy" || collision.transform.tag == "Ground" && launch) {
            isFly = false;
            switch (bt) {
                case BirdType.Red: sr.sprite = redHurt; break;
                case BirdType.Yellow: sr.sprite = yellowHurt; break;
                case BirdType.Green: sr.sprite = greenHurt; break;
                case BirdType.Black: sr.sprite = blackHurt; break;
            }
            Invoke("DestroyMyself", 3);
        }
    }
    public void AudioPlay(AudioClip ac) {
        AudioSource.PlayClipAtPoint(ac, transform.position);
    }

}
