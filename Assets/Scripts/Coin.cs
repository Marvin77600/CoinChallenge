using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] TypeEnum type;
    [SerializeField] AudioClip audioClip;

    bool onGround;
    bool canElevate;
    Rigidbody rb;
    float baseElevation;

    public TypeEnum Type => type;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canElevate = true;
    }

    void Update()
    {
        if (onGround)
        {
            Rotate();
            Elevate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Coin"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag.Equals("Floor"))
        {
            Destroy(rb);
            baseElevation = transform.localPosition.y;
            onGround = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            player.AddPoint(this);
            UIManager.Instance.SetScore(player.Score);
            //AudioSource.PlayClipAtPoint(audioClip, transform.position);
            Destroy(gameObject);
        }
    }

    void Rotate()
    {
        var rotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rotation.x, rotation.y + 1, rotation.z);
    }

    void Elevate()
    {
        if (canElevate)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + .01f, transform.localPosition.z);
            if (transform.localPosition.y >= baseElevation + 1)
                canElevate = false;
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - .01f, transform.localPosition.z);
            if (transform.localPosition.y <= baseElevation)
                canElevate = true;
        }
    }

    public enum TypeEnum
    {
        Simple, Rare, SuperRare
    }
}