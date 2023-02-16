using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private TypeEnum type;

    private bool onGround;
    private bool canElevate;
    private Rigidbody rb;
    private float baseElevation;

    public TypeEnum Type => type;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canElevate = true;
    }

    private void Update()
    {
        if (onGround)
        {
            Rotate();
            Elevate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            print($"Destroy {collision.gameObject.name}");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Floor")
        {
            print("floor collision");
            Destroy(rb);
            baseElevation = transform.localPosition.y;
            onGround = true;
        }

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent(out Player c))
            {
                c.AddPoint(this);
                Destroy(gameObject);
            }
        }
    }

    private void Rotate()
    {
        var rotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rotation.x, rotation.y + 1, rotation.z);
    }

    private void Elevate()
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