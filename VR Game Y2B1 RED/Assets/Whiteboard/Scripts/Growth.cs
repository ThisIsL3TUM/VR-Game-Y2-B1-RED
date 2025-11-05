using UnityEngine;

public class Growth : MonoBehaviour
{
    
    [SerializeField]
    public GameObject carrot;
    [SerializeField]
    public GameObject spawn;
    public int CarrotsGrown = 0;
    
    
    public bool condition = false;
    public float growthProgress;
    public float growthTime;
    public float speedMultiplier;
    public float hydration = 0;
    public Vector3 skibidi = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;



    private void Start()
    {
        //Debug.Log("Object created! Timer started.");
        
        StartCoroutine(LifetimeTimer());//start the timer right away
    }


    private System.Collections.IEnumerator LifetimeTimer()
    {
        while (growthProgress < growthTime)//keep the coroutine running
        {
            
            // Apply speed multiplier
            growthProgress += Time.deltaTime * speedMultiplier * hydration;//ensuring all our multiplier have effect plus the passage of time
            if(growthProgress/growthTime  > 0.5)//at half of the growth progress we give the player some visual update
            {
                this.gameObject.transform.localScale = skibidi;//changing the scale of the seedling made into a variable due to different seedling used
            }
            
            yield return null;
        }
        
        CarrotsGrown++;//unnecesary part which i havent removed
        //Debug.Log("carrots grown" + CarrotsGrown);
        Vector3 spawnPos = (spawn.transform.position);

        Instantiate(carrot, spawnPos, Quaternion.identity);//making our crop pop into existance
        audioSource.PlayOneShot(grabClip);//sound
        yield return new WaitForSeconds(grabClip.length);//wait for the length of the clip before destroying seedling
        condition = true;// a condition to communicate with other objects to ensure the destruction of parent
        Destroy(this.gameObject);
    }

    public void Update()
    {
        
    }
}