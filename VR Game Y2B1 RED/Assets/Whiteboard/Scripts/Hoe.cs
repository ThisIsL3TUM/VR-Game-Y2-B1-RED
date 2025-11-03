
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public int DirtTransform = 5;
    public GameObject HoedDirt;
    public CollisionTracker collisionTracker;
    public Ingredient ingredient;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dirtClip;
    public ParticleSystem waterEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hoe"))
        {
            DirtTransform--;
            waterEffect.Play();
            audioSource.PlayOneShot(dirtClip);
            if (DirtTransform == 0)
            {
                CreateDirt();
            }
        }
    }

    void CreateDirt()
    {
        Instantiate(HoedDirt, this.gameObject.transform.position, Quaternion.identity);
        ingredient.dirt = true;
        collisionTracker.collidingObjects.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
   
}
