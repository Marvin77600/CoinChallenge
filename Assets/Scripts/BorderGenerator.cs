using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator : MonoBehaviour
{

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private GameObject[] objects;


    // Start is called before the first frame update
    void Start()
    {
        for (float i = startPos.x; i < endPos.x; i += 1.4f)
        {
            int randomInt = Random.Range(0, objects.Length);
            float randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(i, 10.5f, startPos.z), Quaternion.Euler(0, randomRotation, 0));
            randomInt = Random.Range(0, objects.Length);
            randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(i, 11.9f, startPos.z), Quaternion.Euler(0, randomRotation, 0));
        }
        for (float i = startPos.z; i < endPos.z; i += 1.4f)
        {
            int randomInt = Random.Range(0, objects.Length);
            float randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(startPos.x, 10.5f, i), Quaternion.Euler(0, randomRotation, 0));
            randomInt = Random.Range(0, objects.Length);
            randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(startPos.x, 11.9f, i), Quaternion.Euler(0, randomRotation, 0));
        }
        for (float i = endPos.x; i >= startPos.x; i -= 1.4f)
        {
            int randomInt = Random.Range(0, objects.Length);
            float randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(i, 10.5f, endPos.z), Quaternion.Euler(0, randomRotation, 0));
            randomInt = Random.Range(0, objects.Length);
            randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(i, 11.9f, endPos.z), Quaternion.Euler(0, randomRotation, 0));
        }
        for (float i = endPos.z; i >= startPos.z; i -= 1.4f)
        {
            int randomInt = Random.Range(0, objects.Length);
            float randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(endPos.x, 10.5f, i), Quaternion.Euler(0, randomRotation, 0));
            randomInt = Random.Range(0, objects.Length);
            randomRotation = Random.Range(0, 361);
            Instantiate(objects[randomInt], new Vector3(endPos.x, 11.9f, i), Quaternion.Euler(0, randomRotation, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
