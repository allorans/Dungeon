using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera thisCamera;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(thisCamera.transform);
    }
}
