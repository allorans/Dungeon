using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }

    
}
