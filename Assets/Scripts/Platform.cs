using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    [SerializeField] Vector3 platformTarget;
    [SerializeField] float speed;
    
    public Player Player { get; set; }
    bool reverse = false;
    Vector3 basePosition;

    private void Start()
    {
        basePosition = transform.localPosition;
    }

    void FixedUpdate() => MovePlatform();

    void MovePlatform()
    {
        if (!reverse)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x >= platformTarget.x)
                reverse = true;
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= basePosition.x)
                reverse = false;
        }
    }
}
