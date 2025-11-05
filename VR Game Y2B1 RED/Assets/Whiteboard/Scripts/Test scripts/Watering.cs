
using UnityEngine;
using UnityEngine.UI;

public class Watering : MonoBehaviour
{
    public float maxWater = 100;
    public float currentWater = 0;
    public Growth growth;
    public Image Fill;
    [SerializeField] private AudioSource audioSource; 
    
    private bool isPlaying = false;


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
        if (other.gameObject.CompareTag("wCan"))//while the can is within the trigger
        {
            //Debug.Log("started colliding with can "+ other.gameObject.name);
            if(other.gameObject.transform.rotation.x <= 250 && currentWater <= maxWater)//ensuring the watering only happens upon a tilted angle and so we dont overfill the plant
            {
                if(audioSource.loop == true && isPlaying == false)//making sure the audio is played on loop
                {
                    isPlaying = true;
                    audioSource.Play();
                }
                
                
                if (currentWater == 0)//if the water level is zero we start hydration routine
                {
                    StartCoroutine(LifetimeTimer());
                }
                currentWater += 1f;//we add more water each frame
                
                Fill.fillAmount = currentWater / 100f;//so we see progress on the UI


            }
            else//once we stop watering we turn of the sound
            {
                audioSource.loop = false;
                isPlaying = false;
            }
        }
    }

    private System.Collections.IEnumerator LifetimeTimer()
    {
        
        yield return new WaitForSeconds(2); //THIS VERY VERY IMPORTANT!!!!!!!! without the pause we wouldnt be able to start the routine idk why
        while (0 < currentWater)
        {
            
            currentWater -= Time.deltaTime;//decreas sec
            Fill.fillAmount = currentWater / 100f;//update UI

            
            if (currentWater > 75 && triggered[0] == false)//check water levels adjust hydration
            {
                growth.speedMultiplier = 1;
                growth.hydration = 1;
                growth.speedMultiplier = growth.speedMultiplier * (growth.hydration);
                triggered[0] = true;
                triggered[1] = false;
                //Debug.Log("Growth speed normal");
            }
            else if (currentWater < 75 && currentWater > 25 && triggered[1] == false)//check water levels adjust hydration
            {
                growth.hydration = 0.75f;
                growth.speedMultiplier = growth.speedMultiplier * (growth.hydration);
                triggered[1] = true;
                triggered[2] = false;
                //Debug.Log("Growth speed reduced");
            }
            else if (currentWater < 25 && triggered[2] == false)//check water levels adjust hydration
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
