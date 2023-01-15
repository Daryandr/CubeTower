using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cSpeed = 0.5f;
    public Transform cubeToPlace;
    public GameObject[] cubesToCreate;
    public GameObject allCubes, vfx;
    private Rigidbody rb;
    private bool lose = false, firstCube = false;
    private Coroutine showPlace;
    public GameObject[] startPage;
    private float camMoveToY, camSpeed = 2f;
    private Transform mainCam;
    private int count = 0;
    public Color[] bgColors;
    private Color toCamCol;
    public Text best,now;
    private List<GameObject> possible = new List<GameObject>();
    private List<Vector3> allPos = new List<Vector3>()
    {
        new Vector3(0,0,0),
        new Vector3(0,1,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(-1,0,-1)
    };
    private void Start()
    {
        if (PlayerPrefs.GetInt("best") < 5) possible.Add(cubesToCreate[0]);
        else if (PlayerPrefs.GetInt("best") < 10) AddCubes(2);
        else if (PlayerPrefs.GetInt("best") < 15) AddCubes(3);
        else if (PlayerPrefs.GetInt("best") < 20) AddCubes(4);
        else if (PlayerPrefs.GetInt("best") < 25) AddCubes(5);
        else if (PlayerPrefs.GetInt("best") < 30) AddCubes(6);
        else if (PlayerPrefs.GetInt("best") < 35) AddCubes(7);
        else if (PlayerPrefs.GetInt("best") < 40) AddCubes(8);
        else if (PlayerPrefs.GetInt("best") < 50) AddCubes(9);
        else AddCubes(10);
        best.text = "<size=40>Best: </size>" + PlayerPrefs.GetInt("best");
        now.text = "<size=30>Now: </size>0";
        toCamCol = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToY = 5.9f + nowCube.y - 1f;
        rb = allCubes.GetComponent<Rigidbody>();
        showPlace = StartCoroutine(ShowPlace());
    }
    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace!=null && allCubes!=null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
			if (Input.GetTouch(0).phase != TouchPhase.Began || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif
            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in startPage) Destroy(obj);
            }
            GameObject cubeToCreate = null;
            if (possible.Count == 1) cubeToCreate = cubesToCreate[0];
            else cubeToCreate = possible[UnityEngine.Random.Range(0, possible.Count)];
            GameObject newCube = Instantiate(cubeToCreate, cubeToPlace.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(allCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allPos.Add(nowCube.getVector());
            if (PlayerPrefs.GetString("music") == "Yes")
                GetComponent<AudioSource>().Play();
            GameObject newVfx = Instantiate(vfx, cubeToPlace.position, Quaternion.identity) as GameObject;
            Destroy(newVfx,1.5f);
            rb.isKinematic = true;
            rb.isKinematic = false;
            SpawnPos();
            MoveCamera();
        }
        if (!lose && rb.velocity.magnitude > 0.1f){
            Destroy(cubeToPlace.gameObject);
            lose = true;
            StopCoroutine(showPlace);
        }
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveToY, mainCam.localPosition.z), camSpeed * Time.deltaTime);
        if (Camera.main.backgroundColor != toCamCol)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCamCol, Time.deltaTime / 1.5f);
    }
    IEnumerator ShowPlace()
    {
        while (true)
        {
            SpawnPos();
            yield return new WaitForSeconds(cSpeed);
        }
    }
    private void SpawnPos()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (IsEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0) lose = true;
        else cubeToPlace.position = positions[0];
    }
    private bool IsEmpty(Vector3 pos)
    {
        if (pos.y == 0 || allPos.Contains(pos)) return false;
        else return true;
    }
    private void MoveCamera()
    {
        int maxX = 0, maxY = 0, maxZ = 0, max;
        foreach(Vector3 pos in allPos)
        {
            if (Math.Abs((int)pos.x) > maxX) maxX = (int)pos.x;
            if ((int)pos.y > maxY) maxY = (int)pos.y;
            if (Math.Abs((int)pos.z) > maxZ) maxZ = (int)pos.z;
        }
        maxY--;
        if (PlayerPrefs.GetInt("best") < maxY)
            PlayerPrefs.SetInt("best", maxY);
        best.text = "<size=40>Best: </size>" + PlayerPrefs.GetInt("best");
        now.text = "<size=30>Now: </size>" + maxY;
        camMoveToY = 5.9f + nowCube.y - 1f;
        max = maxX > maxZ ? maxX : maxZ;
        if (max % 3 == 0 && count != max)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            count = max;
        }
        if (maxY >= 15)
            toCamCol = bgColors[2];
        else if (maxY >= 10)
            toCamCol = bgColors[1];
        else if (maxY >= 5)
            toCamCol = bgColors[0];
    }
    private void AddCubes(int n)
    {
        for(int i = 0; i < n; i++)
        {
            possible.Add(cubesToCreate[i]);
        }
    }
}
struct CubePos
{
    public int x, y, z;
    public CubePos(int x1,int y1,int z1)
    {
        x = x1;
        y = y1;
        z = z1;
    }
    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }
    public void setVector(Vector3 pos)
    {
        x = (int)(pos.x);
        y = (int)(pos.y);
        z = (int)(pos.z);
    }
}
