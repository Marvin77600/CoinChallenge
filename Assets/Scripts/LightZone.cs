using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightZone : MonoBehaviour
{
    [SerializeField] Light[] lights;
    [SerializeField] Light directionalLight;

    BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            TurnLights(true);
            TurnDirectionalLight(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            TurnLights(false);
            TurnDirectionalLight(true);
        }
    }

    void TurnLights(bool _flag)
    {
        foreach (var light in lights)
        {
            light.gameObject.SetActive(_flag);
        }
    }

    void TurnDirectionalLight(bool _flag)
    {
        directionalLight.gameObject.SetActive(_flag);
    }
}
