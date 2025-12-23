using System.Collections;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [Header("Enemy Settings")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed = 2;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float coolDown=2f; //between Attacks
    private float nextAttackTime = 0f;
    CapsuleCollider2D punchCollider;
    [SerializeField] GameObject punchArea;
    BoxCollider2D boxCollider;
    [SerializeField] Transform healthBarTransform;
    private Vector3 originalBarScale;
    bool getHurt=false;
    [SerializeField] float lifePoints=100f;
    private SpriteRenderer mySprite;
    
    private Coroutine attackRoutine = null;
    
    private Vector3 orginalScale;
    

    
    
    bool playerInsideAttackRange=false;// need to change in the firstSpawnPoint is inside the range
    
    string tagPlayer = "Player";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        _player = GameObject.FindGameObjectWithTag(tagPlayer);
        punchCollider=punchArea.GetComponent<CapsuleCollider2D>();
        boxCollider=GetComponentInChildren<BoxCollider2D>();
        orginalScale =  transform.localScale;
        originalBarScale = healthBarTransform.localScale;
        mySprite = GetComponent<SpriteRenderer>();
        if(gameObject.name.Contains("Strong"))
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void FixedUpdate()
    {
        
        if (rb == null)
        {
            Debug.LogWarning("Missing Rigidbody2D");
            return;
        }
        if(!playerInsideAttackRange && getHurt==false )
        {
            animator.SetBool("idle", true);
            Vector2 direction = (_player.transform.position - transform.position).normalized*( _speed * Time.deltaTime);
            rb.MovePosition(rb.position + direction * (_speed * Time.fixedDeltaTime));
            if(direction.x <= 0) 
            {
                float scaleX = orginalScale.x;
                transform.localScale = new Vector3(scaleX, orginalScale.y, 1);
                float newBarX = originalBarScale.x;
                healthBarTransform.localScale = new Vector3(newBarX, originalBarScale.y, originalBarScale.z);
            }
            else
            {
                float scaleX = -orginalScale.x;
                transform.localScale = new Vector3(scaleX, orginalScale.y, 1);
                float newBarX = -originalBarScale.x;
                healthBarTransform.localScale = new Vector3(newBarX, originalBarScale.y, originalBarScale.z);
            }
            
        }
        
        
        

    }
    IEnumerator DieSequence()
    {

        float duration = 1f;      
        float blinkSpeed = 0.1f;  
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            
            mySprite.enabled = false;
            yield return new WaitForSeconds(blinkSpeed);
            mySprite.enabled = true;
            yield return new WaitForSeconds(blinkSpeed);
        }
        Destroy(gameObject);
        Time.timeScale = 0;
    }
    public void StartHurtProcess(float damage)
    {
        lifePoints-=damage;
        
        getHurt = true;
        animator.SetBool("idle", false);
        if(lifePoints<=0)
        {
            animator.SetTrigger("dead");
            StartCoroutine(DieSequence());
            return;
        }
        if(attackRoutine!=null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
            turnOffPunchAREA();//to make sure that punch area opened and we stop the coroutine before turn off
            
        }
        StartCoroutine(HurtSequence());
    }
    IEnumerator HurtSequence()
    {
        
        animator.SetTrigger("hurt");
        yield return null; 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HurtAnimation"))
        {
            yield return null; 
        }
        while (!IsAnimationFinised(animator,"HurtAnimation"))
        {
            yield return null; 
        }
        processAfterHurt();
    }

    void processAfterHurt()
    {
        getHurt = false;
        nextAttackTime = Time.time + coolDown;//reset the time to attack
        animator.SetBool("idle", true); 
    }
    public static bool IsAnimationFinised(Animator animator, string AnimationName)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        
        if (!info.IsName(AnimationName)) return true;
        
        if (info.normalizedTime >= 1)
            return true;
        
        return false;
    }
    void OnTriggerEnter2D(Collider2D collision)//THE PLAYER enter the area stop walking
    {
        if (collision.CompareTag("Player"))
        {
            
            playerInsideAttackRange=true;
            animator.SetBool("idle", false);
        
        }
        
    }
    void OnTriggerStay2D(Collider2D collision) //while the player in the area - attack!
    {
        if (getHurt) return;//if get hurt dont attack 
        if (collision.CompareTag("Player"))
        {
            
            if(Time.time>=nextAttackTime)
            {
                
                attackRoutine=StartCoroutine(attackSequence());
                nextAttackTime=Time.time+coolDown;
            }
        }
        
        
    }
    
    IEnumerator attackSequence()
    {
        
        animator.SetTrigger("attack");
        yield return null; 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("PunchAnimation"))
        {
            
            yield return null; 
        }
        punchCollider.enabled=true;
       
        while (!IsAnimationFinised(animator,"PunchAnimation"))
        {
            yield return null; 
        }
        
        Invoke("turnOffPunchAREA",0.1f);//without delay not working becuase too fast
        
        
    }
    void turnOffPunchAREA()
    {
        punchCollider.enabled=false;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        
        if(boxCollider.IsTouching(collision))return;
        //because we have two colliders we need to make sure that the currect collider call trigger exit
        if (collision.CompareTag("Player"))
        {
            
            playerInsideAttackRange=false;
            
            
            
        
        }
        
    }





}