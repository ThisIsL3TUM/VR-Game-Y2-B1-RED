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
        
        StartCoroutine(LifetimeTimer());
    }


    private System.Collections.IEnumerator LifetimeTimer()
    {
        while (growthProgress < growthTime)
        {
            
            // Apply speed multiplier
            growthProgress += Time.deltaTime * speedMultiplier * hydration;
            if(growthProgress/growthTime  > 0.5)
            {
                this.gameObject.transform.localScale = skibidi;
            }
            // Optional: update a UI bar here
            yield return null;
        }
        
        CarrotsGrown++;
        //Debug.Log("carrots grown" + CarrotsGrown);
        Vector3 spawnPos = (spawn.transform.position);

        Instantiate(carrot, spawnPos, Quaternion.identity);
        audioSource.PlayOneShot(grabClip);
        yield return new WaitForSeconds(grabClip.length);
        condition = true;
        Destroy(this.gameObject);
    }

    public void Update()
    {
        
    }
}