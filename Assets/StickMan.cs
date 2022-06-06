using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StickMan : MonoBehaviour
{
    [SerializeField] AudioClip collectionSound;
    RaycastHit hit;
    public static event System.Action GameOverEvent;
    public static event System.Action<GameObject> WinEvent;
    public static event System.Action CollectLadder;
    public static event System.Action UseLadder;
    bool isGameStarted;
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
    GameManager gameManager;
    void OnEnable()
    {
        CollectLadder += Collect;  
        WinEvent += Dance;
    }
    void OnDisable()
    {
        CollectLadder -= Collect;
        WinEvent -= Dance;
        
    }
    private void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
        DOTween.Init();
        _anim = transform.GetComponentInChildren<Animator>();
        laddersOnTheBackCount = 0;
    }
    private void FixedUpdate() {
        if(isGameStarted && !gameManager.isGameOver)
        {
            if(!isTriggerWithLadder)
            {
                
                transform.Translate(Vector3.down*movementSpeed*2);
                
                transform.Translate(Vector3.back*movementSpeed,Space.World);
            }
            else
            {
                transform.Translate(Vector3.back*movementSpeed*1.5f + Vector3.up*movementSpeed*1.5f,Space.World);
            }
        }
    }
    private void Update() {

        Vector3 rayPos = new Vector3(
            rayPos.x = transform.position.x,
            rayPos.x  = transform.position.y + .5f,
            rayPos.x  = transform.position.z
        );

        if(Physics.Raycast(rayPos,transform.TransformDirection(Vector3.down), out hit,Mathf.Infinity))
        {
            
            // if(hit.collider.tag == "merdiven" | hit.collider.tag == "yol")
            // {
            //     if(isGameStarted)
            //     {
            //         _anim.SetBool("isGround" ,true);
            //         _anim.SetBool("havadaMi" ,false);
            //     }
            // }
            
           
            if(hit.collider.tag == "merdiven")
            {
                isTriggerWithLadder = true;
                // transform.position += (Vector3.back*movementSpeed);
                isFirst = false;
            }
            else
            {
                isTriggerWithLadder = false;
            }
        }

        if(Input.anyKey)
        {
            startCanvas.enabled = false;
            isGameStarted = true;
        }
        if(isGameStarted && !gameManager.isGameOver)
        {
            Movement();
            if(Input.GetKey(KeyCode.Space) && isCoolDown && hareketEdebilir)
            {
                CreateLadder();
                isCoolDown = false;
            }
            if(isFirst)
            {
                firstLadderPosition.localPosition =  new Vector3(transform.localPosition.x,transform.localPosition.y-.1f,transform.localPosition.z - 0.2f);
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
    
    
    public void Collect()
    {
        laddersOnTheBack[laddersOnTheBackCount].SetActive(true);
        laddersOnTheBackCount++;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "yerdekiMerdiven")
        {
            AudioSource.PlayClipAtPoint(collectionSound,Camera.main.transform.position);
            CollectLadder.Invoke();
            other.gameObject.SetActive(false);
        }
        if(other.gameObject.tag == "Bosluk" || other.gameObject.tag == "Barrier")
        {
            GameOverEvent.Invoke();
        }
        if(other.gameObject.tag == "FinishPoint")
        {
            WinEvent.Invoke(other.gameObject);
        }
        
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
            UseLadder.Invoke();
            isFirst = false;
        }
    }
    void Dance(GameObject other)
    {
        _anim.SetTrigger("Dance");
    }
}
