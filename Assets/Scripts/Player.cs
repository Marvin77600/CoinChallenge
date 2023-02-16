using UnityEngine;

public class Player : Entity, IInteractable
{
    private Rigidbody characterRigidbody;
    private Animator animator;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float playerWalkSpeed;
    [SerializeField]
    private float playerRunSpeed;
    [SerializeField]
    private CoinCollection coinCollection;
    private PlayerAnim playerAnim;
    private AudioSource audioSource;
    private int enemyKillCount;

    private int score = 0;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool characterIsOnTheFloor = true;

    public BlockDoubleDoor ClosestDoubleDoor { get; set; }
    public BlockLootable ClosestChest { get; set; }

    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAnim = GetComponent<PlayerAnim>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool spacePressed = Input.GetButtonDown("Jump");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (shiftPressed)
            {
                characterRigidbody.velocity = moveDirection.normalized * playerRunSpeed;
                playerAnim.Run();
            }
            else
            {
                characterRigidbody.velocity = moveDirection.normalized * playerWalkSpeed;
                playerAnim.Walk();
            }
        }

        if (direction.magnitude == 0)
        {
            playerAnim.Idle();
        }

        if (spacePressed && characterIsOnTheFloor)
        {
            playerAnim.Jump();
            characterRigidbody.velocity = new Vector3(0, 5, 0);
            characterIsOnTheFloor = false;
        }
    }

    void Update()
    {
        bool interactPressed = Input.GetButtonDown("Interact");
        bool leftClick = Input.GetKeyUp(KeyCode.Mouse0);
        bool rightClick = Input.GetKeyUp(KeyCode.Mouse1);

        if (leftClick)
        {
            playerAnim.Attack();
            if (Target != null && CanAttackTarget(Target))
            {
                if (Target.Health - DamageMelee <= 0) enemyKillCount++;
                AttackTarget(Target);
            }
        }

        if (rightClick)
        {
            if (enemyKillCount > 0) enemyKillCount--;
        }

        if (interactPressed)
        {
            if (ClosestChest != null && ClosestChest.CanLoot)
            {
                ClosestChest.Loot();
                AddPoint(ClosestChest.MoneyValue);
            }

            if (ClosestDoubleDoor != null && !ClosestDoubleDoor.Opened && enemyKillCount >= ClosestDoubleDoor.MinimumEnemyKillCount)
            {
                ClosestDoubleDoor.Open();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            characterIsOnTheFloor = true;
        }
    }

    public void AddPoint(Coin coin)
    {
        switch (coin.Type)
        {
            case Coin.TypeEnum.Simple:
                score += coinCollection.SimpleCoinPoints;
                break;
            case Coin.TypeEnum.Rare:
                score += coinCollection.RareCoinPoints;
                break;
            case Coin.TypeEnum.SuperRare:
                score += coinCollection.SuperRareCoinPoints;
                break;
        }
        PlayPickupSound();
    }

    public void AddPoint(int points)
    {
        score += points;
        PlayPickupSound();
    }

    public int Score => score;

    public int EnemyKillCount => enemyKillCount;

    private void PlayPickupSound()
    {
        audioSource.Play();
    }

    public void DisplayTooltip(string str, Color color)
    {
        UIManager.Instance.SetTooltip(str, color);
    }

    public void DisplayTooltip(string str)
    {
        UIManager.Instance.SetTooltip(str);
    }
}

public interface IInteractable
{
    void DisplayTooltip(string str, Color color);

    void DisplayTooltip(string str);

    BlockDoubleDoor ClosestDoubleDoor { get; set; }

    BlockLootable ClosestChest { get; set; }
}