using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StickMan : MonoBehaviour
{
    public int puan;
    bool isGameOver;
    bool isGameStarted;
    bool isGrounded;
    bool hareketEdebilir = true;
    public bool isRight;
    private Animator _anim;
    bool isFirst = true;
    [SerializeField] [Range(0,10)] public float movementSpeed;
    [SerializeField] private GameObject _ladder;
    public List<GameObject> laddersOnTheBack;
    public List<GameObject> createdLadders;
    public int laddersOnTheBackCount;
    public Transform firstLadderPosition;
    public float createLadderRate = 0.05f;
    public bool isCoolDown;
    public bool isTriggerWithLadder;
    public float isFirstCooldown = .5f;
    public Canvas startCanvas;
    public GameObject finishCanvas;
    private void Start() 
    {
        DOTween.Init();
        _anim = transform.GetComponentInChildren<Animator>();
        laddersOnTheBackCount = 0;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "yerdekiMerdiven")
        {
            laddersOnTheBack[laddersOnTheBackCount].SetActive(true);
            laddersOnTheBackCount++;
            other.gameObject.SetActive(false);
        }
        if(isGameStarted)
        {
            if(other.gameObject.tag == "yol")
            {
                isGrounded = true;
            }
        }
        if(other.gameObject.tag == "Bosluk")
        {
            StartCoroutine(FinishGame());
        }
        if(other.gameObject.tag == "FinishPoint")
        {
            puan += other.GetComponent<Puan>().puan;
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "merdiven")
        {
            isTriggerWithLadder = true;
            // transform.position += (Vector3.back*movementSpeed);
            Debug.Log("merdivene degıyor");
            isFirst = false;
        }
        
        if(other.gameObject.tag == "merdiven" | other.gameObject.tag == "yol")
        {
            if(isGameStarted)
            {
                _anim.SetBool("isGround" ,true);
                _anim.SetBool("havadaMi" ,false);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "merdiven")
        {
            isTriggerWithLadder = false;
            //isFirst = true;
            Debug.Log("OnTriggerExit");
            _anim.SetBool("havadaMi" ,true);
        }
        if(other.gameObject.tag == "yol")
        {
            isGrounded = false;
        }
    }
    private void FixedUpdate() {
        if(isGameStarted && !isGameOver)
        {
            if(!isTriggerWithLadder)
            {
                if(!isGrounded)
                {
                    transform.Translate(Vector3.down*movementSpeed*2);
                }
                transform.Translate(Vector3.back*movementSpeed,Space.World);
            }
            else
            {
                transform.Translate(Vector3.back*movementSpeed*1.5f + Vector3.up*movementSpeed*1.5f,Space.World);
            }
        }
    }
    private void Update() {
        if(Input.anyKey)
        {
            startCanvas.enabled = false;
            isGameStarted = true;
        }
        if(isGameStarted && !isGameOver)
        {
            Movement();
            if(Input.GetKey(KeyCode.Space) && isCoolDown && hareketEdebilir)
            {
                CreateLadder();
                isCoolDown = false;
            }
            if(isFirst)
            {
                firstLadderPosition.position =  new Vector3(transform.localPosition.x,transform.localPosition.y-.1f,transform.localPosition.z - 0.2f);
            }
            else
            {
                if(createdLadders.Count>0)
                firstLadderPosition.position = createdLadders[createdLadders.Count-1].transform.GetChild(0).position;
            }
            createLadderRate -= Time.deltaTime;
            if(createLadderRate <= 0)
            {
                isCoolDown = true;
                createLadderRate = 0.02f;
            }
            isFirstCooldown -= Time.deltaTime;
            if(isFirstCooldown <= 0)
            {
                isFirstCooldown = .5f;
                isFirst = true;
            }
        }
    }
    public IEnumerator FinishGame()
    {
        isGameOver = true;
        yield return new WaitForSeconds(1);
        finishCanvas.SetActive(true);
    }
    public void Movement()
    {
        if(Input.GetKeyDown(KeyCode.A) && hareketEdebilir)
        {
            if(isRight)
            {
                hareketEdebilir = false;
                isRight = false;
                transform.DOMoveX(transform.position.x + 1.5f,.2f).OnComplete(()=> hareketEdebilir = true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.D) && hareketEdebilir)
        {
            if(!isRight)
            {
                hareketEdebilir = false;
                isRight = true;
                transform.DOMoveX(transform.position.x - 1.5f,.2f).OnComplete(()=> hareketEdebilir = true);
            }
        }
    }    
    public void CreateLadder()
    {
        if(laddersOnTheBackCount > 0)
        {
            GameObject yeniMerdiven = Instantiate(_ladder,firstLadderPosition.position,Quaternion.Euler(transform.rotation.eulerAngles.x +45,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z));
            createdLadders.Add(yeniMerdiven);
            laddersOnTheBack[laddersOnTheBackCount -1].SetActive(false);
            laddersOnTheBackCount--;
            isFirst = false;
        }
    }
}
