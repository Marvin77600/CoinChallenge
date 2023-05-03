using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Zone : MonoBehaviour
{
    [SerializeField] GameObject keyGO;
    [SerializeField] int numOfEnemyToKill;
    [SerializeField] Player player;

    bool keyActivate;
    VisualEffect smoke;
    new Camera camera;
    BoxCollider boxCollider;

    public List<Entity> Entities { get; set; }

    public int NumbOfEnemyToKill
    {
        get { return numOfEnemyToKill; }
        set { numOfEnemyToKill = value; }
    }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        camera = GetComponentInChildren<Camera>();
        smoke = GetComponentInChildren<VisualEffect>(true);
        camera.transform.LookAt(keyGO.transform.position);
    }

    public void SwitchCamera()
    {
        if (!keyActivate)
        {
            StartCoroutine(CameraManager.Instance.SwitchToCamera(player, camera, smoke, keyGO));
            keyActivate = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.ActualZone != null) player.ActualZone.DestroyEnemies();
            player.ActualZone = this;
        }
    }

    public void DestroyEnemies()
    {
        foreach (var entity in Entities)
        {
            Destroy(entity.gameObject);
        }
        Entities.Clear();
    }
}