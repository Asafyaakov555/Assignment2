using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 25f;
    [SerializeField] GameObject punchArea;

    Vector3 direction = Vector3.zero;
    [SerializeField] Transform healthBarTransform;
    private Vector3 originalBarScale;
    [Header("Movement Limits")]
    [SerializeField] private float minY = -0.665f; 
    [SerializeField] private float maxY = 0.669f;  
    [SerializeField] float minX=-1.666f;
    [SerializeField] float maxX=1.727f;
    
    
    
    private Vector3 orginalScale;

    private Rigidbody2D rb;
    private Animator animator;
    CapsuleCollider2D punchCollider;
    bool startGame=false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("animator is null ");
            Time.timeScale = 0;

        }
        punchCollider=punchArea.GetComponent<CapsuleCollider2D>();
        if (punchCollider == null)
        {
            Debug.Log("punchCollider is null ");
            Time.timeScale = 0;

        }
        
        
        orginalScale =  transform.localScale;
        originalBarScale = healthBarTransform.localScale;
    }
    public void gameStarted()
    {
        startGame=true;
    }

    
    void Update()
    {
        if(!startGame)return;
        
        if (Input.GetMouseButtonDown(0))
        {
            
            StartCoroutine(attackSequence());
            
            
        }
        var y = Input.GetAxis("Vertical");
        var x = Input.GetAxis("Horizontal");
        if(x<0 && transform.position.x<=minX)
            x=0;
        if(x>0 && transform.position.x>=maxX)
            x=0;
        if(y<0 && transform.position.y<=minY)
            y=0;
        if(y>0 && transform.position.y>=maxY)
            y=0;
        direction = new Vector3(x, y, 0);
        
        
        bool isWalking =   x != 0 || y != 0;
        animator.SetBool("IsWalking", isWalking);


        if (isWalking)
        {
            transform.localScale = new Vector3(x < 0 ? -orginalScale.x : orginalScale.x, orginalScale.y, 1);
            if (healthBarTransform != null)
            {
                
                float newBarX = x < 0 ? -originalBarScale.x : originalBarScale.x;
                healthBarTransform.localScale = new Vector3(newBarX, originalBarScale.y, originalBarScale.z);
            }
        }
        
        if (rb == null)
        {
            Debug.LogWarning("Missing Rigidbody2D");
            return;
        }
        
        rb.MovePosition(rb.position + (Vector2)direction * (_speed * Time.fixedDeltaTime));
        

    }
    IEnumerator attackSequence()
    {
        
        animator.SetTrigger("Attack");
        yield return null; 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerPunch"))
        {
            
            yield return null; 
        }
        punchCollider.enabled=true;
        while (!IsAnimationFinised(animator,"PlayerPunch"))
        {
            yield return null; 
        }
        punchCollider.enabled=false;
        
        
        
    }
    

    public static bool IsAnimationFinised(Animator animator, string AnimationName)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        
        if (!info.IsName(AnimationName)) return true;
        
        if (info.normalizedTime >= 1)
            return true;
        
        return false;
    }

    

    

    

    
    
}
