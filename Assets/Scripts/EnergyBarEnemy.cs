using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarEnemy : MonoBehaviour
{
    public Slider energyBarEnemy;

    private int maxEnergyEnemy = 100;
    public int currentEnergyEnemy;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static EnergyBarEnemy instance;


    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergyEnemy = maxEnergyEnemy;
        energyBarEnemy.maxValue = maxEnergyEnemy;
        energyBarEnemy.value = maxEnergyEnemy; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseEnergyEnemy(int amount){
        if(currentEnergyEnemy - amount >= 0){
            currentEnergyEnemy -= amount;
            energyBarEnemy.value = currentEnergyEnemy;

            if(regen != null){
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenEnemyEnergy());

        }else{
            Debug.Log("No energy");
        }
    }

    private IEnumerator RegenEnemyEnergy(){
        yield return new WaitForSeconds(1f);
        while(currentEnergyEnemy < maxEnergyEnemy){
            currentEnergyEnemy += maxEnergyEnemy / 100;
            energyBarEnemy.value = currentEnergyEnemy;
            yield return regenTick;
            // if (currentEnergy <= 15){
            //     yield return new WaitForSeconds(5f);
            // }
        }
        regen = null;
    }
}
