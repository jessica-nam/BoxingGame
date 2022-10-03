using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energyBar;

    private int maxEnergy = 100;
    public int currentEnergy;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static EnergyBar instance;

    private void Awake(){
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
        energyBar.maxValue = maxEnergy;
        energyBar.value = maxEnergy; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseEnergy(int amount){
        if(currentEnergy - amount >= 0){
            currentEnergy -= amount;
            energyBar.value = currentEnergy;

            if(regen != null){
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenEnergy());

        }else{
            Debug.Log("No energy");
        }
    }

    private IEnumerator RegenEnergy(){
        yield return new WaitForSeconds(2f);
        while(currentEnergy < maxEnergy){
            currentEnergy += maxEnergy / 100;
            energyBar.value = currentEnergy;
            yield return regenTick;
            // if (currentEnergy <= 15){
            //     yield return new WaitForSeconds(5f);
            // }
        }
        regen = null;
    }
}
