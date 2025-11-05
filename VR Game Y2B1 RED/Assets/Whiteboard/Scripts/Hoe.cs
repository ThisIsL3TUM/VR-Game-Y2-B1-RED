
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
    //on trigger enter we check if its the hoe if so we play vfx sound and lower the count of dirt transform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hoe"))
        {
            DirtTransform--;
            waterEffect.Play();//Just plays a sound effect. confusing naming due to copy pasting it between scripts
            audioSource.PlayOneShot(dirtClip);
            if (DirtTransform == 0)//a check to start our function
            {
                CreateDirt();
            }
        }
    }

    //we just create tilled dirt in the place of our old
    void CreateDirt()
    {
        Instantiate(HoedDirt, this.gameObject.transform.position, Quaternion.identity);
        ingredient.dirt = true;
        collisionTracker.collidingObjects.Remove(this.gameObject);//making sure its not kept within the collision tracker
        Destroy(this.gameObject);//make we delete our previous dirt
    }
   
}
