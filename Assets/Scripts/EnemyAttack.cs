using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    
    [SerializeField] float damageAmountEnemyStrong = 60f; 
    [SerializeField] float damageAmountEnemyNormal = 30f; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            
            MyhealthSystem PlayerHealth = collision.GetComponentInChildren<MyhealthSystem>();

            
            if (PlayerHealth != null)
            {
                
                if(gameObject.name.Contains("Normal"))
                PlayerHealth.TakeDamage(damageAmountEnemyNormal,"Player");
                if(gameObject.name.Contains("Strong"))
                PlayerHealth.TakeDamage(damageAmountEnemyStrong,"Player");
                
            }
            else
            {
                Debug.Log("not found Player health");
            }
            
        }
    }
}