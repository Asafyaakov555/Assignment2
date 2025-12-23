using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    [SerializeField] float damageAmountPlayer = 50f; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
            
            EnemyController enemyController=collision.GetComponent<EnemyController>();
            enemyController.StartHurtProcess(damageAmountPlayer);

            
            MyhealthSystem enemyHealth = collision.GetComponentInChildren<MyhealthSystem>();

            
            if (enemyHealth != null)
            {
                
                enemyHealth.TakeDamage(damageAmountPlayer,"Enemy");
                
            }
        }
    }
}