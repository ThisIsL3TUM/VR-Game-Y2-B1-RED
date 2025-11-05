using UnityEngine;

public class Suicidal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 2)//very specific its jsut for one case in the game but could be made into a serialize field variable
        {
            Destroy(gameObject);//destroy self
        }
    }
}
