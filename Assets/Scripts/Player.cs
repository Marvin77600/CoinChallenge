using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : Entity, IInteractable
{
    [SerializeField] new Camera camera;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerRunSpeed;
    [SerializeField] CoinCollection coinCollection;
    [SerializeField] ScoreCollection scoreCollection;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSmooth;
    [SerializeField] AudioClip ambiantClip;
    [SerializeField] AudioClip pickUpCoinClip;
    [SerializeField] UnityEvent onTouchWater;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] UnityEvent onDamage;
    [SerializeField] UnityEvent onHeal;

    Rigidbody characterRigidbody;
    PlayerAnim playerAnim;
    PlayerInventory inventory;
    AudioSource audioSource;
    int enemyKillCount;
    int score = 0;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public BlockDoubleDoor ClosestDoubleDoor { get; set; }
    public BlockLootable ClosestChest { get; set; }
    public Zone ActualZone { get; set; }
    public PlayerInventory Inventory => inventory;
    public Camera Camera => camera;
    public ScoreCollection ScoreCollection => scoreCollection;
    public bool BlockMovement { get; set; }
    public bool BlockMovementNotJump { get; set; }

    public bool IsJumping
    {
        get
        {
            if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, .1f))
            {
                return false;
            }
            return true;
        }
    }

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnim>();
        audioSource = GetComponent<AudioSource>();
        inventory = GetComponent<PlayerInventory>();
        if (SceneManager.GetActiveScene().name == "EndScene") return;
        UIManager.Instance.HealthUI.SetHealthText();
        audioSource.Play();
    }

    void FixedUpdate()
    {
        if (BlockMovement) return;
        if (!camera) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool spacePressed = Input.GetButtonDown("Jump");
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        if (direction.magnitude >= .5f && !BlockMovementNotJump)
        {
            if (shiftPressed && !IsJumping)
            {
                Utils.LogColor(Utils.ColorEnum.Blue, $"Run {moveDirection}");
                var newVelocity = characterRigidbody.velocity;
                newVelocity.x = moveDirection.normalized.x * playerRunSpeed;
                newVelocity.z = moveDirection.normalized.z * playerRunSpeed;
                characterRigidbody.velocity = newVelocity;
                playerAnim.Run();
            }
            else if (!shiftPressed && !IsJumping)
            {
                Utils.LogColor(Utils.ColorEnum.Green, $"Walk {moveDirection}");
                var newVelocity = characterRigidbody.velocity;
                newVelocity.x = moveDirection.normalized.x * playerWalkSpeed;
                newVelocity.z = moveDirection.normalized.z * playerWalkSpeed;
                characterRigidbody.velocity = newVelocity;
                playerAnim.Walk();
            }
        }

        if (spacePressed && !IsJumping)
        {
            Utils.LogColor(Utils.ColorEnum.Magenta, IsJumping);
            playerAnim.Jump();
            if (shiftPressed) characterRigidbody.velocity = moveDirection.normalized * playerRunSpeed + Vector3.up * 5;
            else if (direction.magnitude == 0) characterRigidbody.velocity = Vector3.up * 5;
            else characterRigidbody.velocity = moveDirection.normalized * playerWalkSpeed + Vector3.up * 5;
        }

        if (direction.magnitude < .5f)
        {
            playerAnim.Idle();
        }

        StepClimb();
    }

    void StepClimb()
    {
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitLower, .1f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUpper, .2f))
            {
                characterRigidbody.position -= new Vector3(0, -stepSmooth, 0);
            }
        }

        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out RaycastHit hitLower45, .1f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out RaycastHit hitUpper45, .2f))
            {
                characterRigidbody.position -= new Vector3(0, -stepSmooth, 0);
            }
        }

        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out RaycastHit hitLowerMinus45, .1f))
        {
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out RaycastHit hitUpperMinus45, .2f))
            {
                characterRigidbody.position -= new Vector3(0, -stepSmooth, 0);
            }
        }
    }

    void Update()
    {
        if (BlockMovement) return;
        bool interactPressed = Input.GetButtonDown("Interact");
        bool leftClick = Input.GetKeyDown(KeyCode.Mouse0);
        bool rightClick = Input.GetKeyDown(KeyCode.Mouse1);

        if (BlockMovement) return;
        if (leftClick)
        {
            if (playerAnim) playerAnim.Attack();
            if (Target != null && CanAttackTarget(Target, out int damageValue))
            {
                if (Target.Health - DamageMelee <= 0) enemyKillCount++;
                AttackTarget(damageValue);
            }
        }

        if (interactPressed)
        {
            if (ClosestChest != null && ClosestChest.CanLoot)
            {
                ClosestChest.Loot();
                AddPoint(ClosestChest.MoneyValue);
                return;
            }

            if (ClosestDoubleDoor != null && !ClosestDoubleDoor.Opened && (inventory.HaveItem(ClosestDoubleDoor.ItemNameNeeded) || ClosestDoubleDoor.ItemNameNeeded == string.Empty))
            {
                ClosestDoubleDoor.Open(this);
                return;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Floor"))
        {
        }
        if (collision.gameObject.tag.Equals("Water"))
        {
            onTouchWater?.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out LittleEnemy littleEnemy))
        {
            littleEnemy.Death();
        }
    }

    public void AddPoint(Coin _coin)
    {
        switch (_coin.Type)
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

    public void AddPoint(int _points)
    {
        score += _points;
        PlayPickupSound();
    }

    public void FinishLevel()
    {
        ScoreCollection.Save(Health, TimerUI.Instance.TimeElapsed, score, IsDead);
    }

    public int Score => score;

    public int EnemyKillCount => enemyKillCount;

    public void BlockMov(bool _flag)
    {
        BlockMovement = _flag;
    }

    public void BlockMovNotJump(bool _flag)
    {
        BlockMovementNotJump = _flag;
    }

    void PlayPickupSound()
    {
        audioSource.PlayOneShot(pickUpCoinClip);
    }

    public void DisplayTooltip(string _str, Color _color)
    {
        UIManager.Instance.SetTooltip(_str, _color);
    }

    public void DisplayTooltip(string _str)
    {
        UIManager.Instance.SetTooltip(_str);
    }

    public override void Death()
    {
        Health = 0;
        playerAnim.Death();
        onDeath?.Invoke();
    }

    public override void Damage(int _value)
    {
        base.Damage(_value);
        onDamage?.Invoke();
    }

    public void DisplayRestartScreen(bool _flag) => UIManager.Instance.DisplayRestartOrQuitMenu(_flag);
}

public interface IInteractable
{
    void DisplayTooltip(string _str, Color _color);

    void DisplayTooltip(string _str);

    BlockDoubleDoor ClosestDoubleDoor { get; set; }

    BlockLootable ClosestChest { get; set; }
}