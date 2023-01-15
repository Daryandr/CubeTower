using UnityEngine;

public class Shake : MonoBehaviour
{
    private Transform camTransf;
    private float shakeDur = 1f, shakeAmount = 0.04f, decreaseFact = 1.5f;
    private Vector3 originPos;
    private void Start()
    {
        camTransf = GetComponent<Transform>();
        originPos = camTransf.localPosition;
    }
    private void Update()
    {
        if (shakeDur > 0)
        {
            camTransf.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFact;
        }
        else
        {
            shakeDur = 0;
            camTransf.localPosition = originPos;
        }
    }
}
