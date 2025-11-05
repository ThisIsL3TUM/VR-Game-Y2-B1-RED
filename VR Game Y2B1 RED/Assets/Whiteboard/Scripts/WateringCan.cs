using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public bool isWatering = false;
    [SerializeField] private float tiltThreshold = 90f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (waterEffect == null)//just a check to avoid errors
            return;

        float angle = Vector3.Angle(transform.up, Vector3.up);//setting up the desired angle

        
        if (angle > tiltThreshold && !isWatering)//check if the watering state should change
        {
            isWatering = true;
            if (!waterEffect.isPlaying)//extra check kept running into buggy behaviour adding to checks fixed it
                waterEffect.Play();//play effect
            Debug.Log("Started watering");
        }
        else if (angle <= tiltThreshold && isWatering)//check if the watering state should change
        {
            isWatering = false;
            if (waterEffect.isPlaying)
                waterEffect.Stop();
            Debug.Log("Stopped watering");
        }
    }
}
