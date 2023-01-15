using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed = 10f;
    private void Start()
    {
        
    }
    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
