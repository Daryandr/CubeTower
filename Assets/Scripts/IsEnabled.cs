using UnityEngine;

public class IsEnabled : MonoBehaviour
{
    public int needToUnlock;
    public Material black;
    private void Start()
    {
        if (PlayerPrefs.GetInt("best") < needToUnlock)
            GetComponent<MeshRenderer>().material = black;
    }
}
