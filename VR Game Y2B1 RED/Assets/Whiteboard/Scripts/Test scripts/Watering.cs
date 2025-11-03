
using UnityEngine;
using UnityEngine.UI;

public class Watering : MonoBehaviour
{
    public float maxWater = 100;
    public float currentWater = 0;
    public Growth growth;
    public Image Fill;
    
    
    bool[] triggered = { false, false, false };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("wCan"))
        {
            //Debug.Log("started colliding with can "+ other.gameObject.name);
            if(other.gameObject.transform.rotation.x <= 250 && currentWater <= maxWater)
            {
                if(currentWater == 0)
                {
                    StartCoroutine(LifetimeTimer());
                }
                currentWater += 1f;
                Fill.fillAmount = currentWater / 100f;


            }
        }
    }

    private System.Collections.IEnumerator LifetimeTimer()
    {
        
        yield return new WaitForSeconds(2); //THIS VERY VERY IMPORTANT!!!!!!!!
        while (0 < currentWater)
        {
            
            currentWater -= Time.deltaTime;
            Fill.fillAmount = currentWater / 100f;

            
            if (currentWater > 75 && triggered[0] == false)
            {
                growth.speedMultiplier = 1;
                growth.hydration = 1;
                growth.speedMultiplier = growth.speedMultiplier * (growth.hydration);
                triggered[0] = true;
                triggered[1] = false;
                //Debug.Log("Growth speed normal");
            }
            else if (currentWater < 75 && currentWater > 25 && triggered[1] == false)
            {
                growth.hydration = 0.75f;
                growth.speedMultiplier = growth.speedMultiplier * (growth.hydration);
                triggered[1] = true;
                triggered[2] = false;
                //Debug.Log("Growth speed reduced");
            }
            else if (currentWater < 25 && triggered[2] == false)
            {
                growth.hydration = 0;
                growth.speedMultiplier = growth.speedMultiplier * (growth.hydration);
                triggered[2] = true;
                triggered[0] = false;
                //Debug.Log("Growth speed zero");
            }
            
            yield return null;
        }
        

    }
}
