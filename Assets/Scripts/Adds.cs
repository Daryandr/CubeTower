using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Adds : MonoBehaviour
{
    private Coroutine showAdd;
    private string gameID = "3712955", type = "video";
    private bool testMode = false, stop = false;
    private static int count = 0;
    private void Start()
    {
        Advertisement.Initialize(gameID, testMode);
        count++;
        if(count % 5 == 0)
        showAdd = StartCoroutine(ShowAdd());
    }
    private void Update()
    {
        if (stop)
        {
            stop = false;
            StopCoroutine(showAdd);
        }
    }
    IEnumerator ShowAdd()
    {
        while (true)
        {
            if (Advertisement.IsReady(type))
            {
                Advertisement.Show(type);
                stop = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
