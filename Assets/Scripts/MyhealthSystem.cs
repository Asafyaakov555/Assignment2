using System;
using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyhealthSystem : MonoBehaviour
{
    [Header("UI References")]
    public Image healthBarImage; 
    public Text healthText;  
    GameManager gameManager;


    [Header("Stats")]
    [SerializeField] float maxHitPoint = 100f;
    private float HitPoint;

    void Start()
    {
        HitPoint = maxHitPoint;
        gameManager=FindAnyObjectByType<GameManager>();
        UpdateGraphics();
    }

    public void TakeDamage(float damage,String toKnowWhoGetHurt)
    {
        HitPoint -= damage;

        if (HitPoint < 0)
            HitPoint = 0;

        UpdateGraphics();
        Debug.Log(toKnowWhoGetHurt +" get Hurt. The new health value is "+HitPoint);
        if(HitPoint==0)
        {
            if(toKnowWhoGetHurt.Contains("Player"))
            {
                gameManager.turnOnTEXT(false);//lose
                
            }
            else
            gameManager.turnOnTEXT(true);//win
            
        }
    }

    private void UpdateGraphics()
    {
        float ratio = HitPoint / maxHitPoint;
		healthText.text = HitPoint.ToString ("0") + "/" + maxHitPoint.ToString ("0");
        healthBarImage.fillAmount = ratio; 

        
    }

    
}