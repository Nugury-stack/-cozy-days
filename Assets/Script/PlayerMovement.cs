using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    Rigidbody _rigidbody;

    public float gravity = -100f;

    private Vector3 lastPosition;
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float slowSpeed;

    private bool isRunning = false;
    private float turnSmoothVelocity;
    public float rotationSmoothTime = 0.1f;

    private bool isGrounded = false;
    public float jumpPower;

    private bool isDead = false;
    private bool isFrozen = false; // 얼림 상태

    public GameObject player_trs;
    public Vector3 startPoint;
    private bool isSlowed = false;
    private Coroutine slowCoroutine;
    public int itemCount = 0;
    [SerializeField] private GameObject freezeCanvas;
    [SerializeField] private GameObject slowCanvas;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        player_trs = GameObject.Find("savePosition");
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        lastPosition = transform.position;

        moveSpeed = walkSpeed;
    }

    void Update()
    {
        Jump(jumpPower);

        if (isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            //SoundManager.Instance.PlaySFX("Stone");
            Throw_Stone.Instance.Create_Stone();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Player_Transition_Save.Instance.savePosition();
            SceneManager.LoadScene("StartScene");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            moveSpeed = runSpeed;
        }
        else
        {
            isRunning = false;
            moveSpeed = walkSpeed;
        }

        if (isFrozen) moveSpeed = 0f;

        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        Move();
        Move_animation_Setting();

        if (!isGrounded && _rigidbody.velocity.y < 0)
        {
            _rigidbody.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
        }
    }

    public void Move()
    {
        if (isDead || isFrozen) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v);
        bool hasInput = input.magnitude >= 0.1f;

        if (hasInput)
        {
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = (forward * v + right * h).normalized;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            _rigidbody.velocity = moveDir * moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
        }
        else
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
    }

    public void Move_animation_Setting()
    {
        float speed = _rigidbody.velocity.magnitude;
        float blendValue = 0f;

        if (speed < 0.1f)
            blendValue = 0f;
        else if (speed <= slowSpeed + 0.1f)
            blendValue = 0.3f;
        else if (!isRunning)
            blendValue = 0.5f;
        else if (isRunning)
            blendValue = 1f;

        _animator.SetFloat("Blend", blendValue);
    }

    public void Jump(float jump)
    {
        if (isDead || isFrozen) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            SoundManager.Instance.PlaySFX("Jump");
            _animator.SetBool("isJump", true);
            _rigidbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void mushroomJump(float super_jump)
    {
        if (isDead || isFrozen) return;

        _animator.SetBool("isJump", true);
        _rigidbody.AddForce(Vector3.up * super_jump, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            _animator.SetBool("isJump", false);
        }
        if (coll.gameObject.CompareTag("die") && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        SoundManager.Instance.PlaySFX("Die");
        isDead = true;
        _rigidbody.velocity = Vector3.zero;
        Death_Count.Instance.Death_C();

        if (player_trs != null)
        {
            player_trs.transform.position = startPoint;
        }

        _animator.SetBool("isDead", true);
        StartCoroutine(RestartSceneAfterDelay(2f));
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ApplySlow(float duration, float slowSpeedValue)
    {
        if (isSlowed) return;

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        // 슬로우 캔버스 켜기
        if (slowCanvas != null)
            slowCanvas.SetActive(true);

        slowCoroutine = StartCoroutine(SlowEffect(duration, slowSpeedValue));
    }

    private IEnumerator SlowEffect(float duration, float slowSpeedValue)
    {
        isSlowed = true;

        float originalWalkSpeed = walkSpeed;
        float originalRunSpeed = runSpeed;

        walkSpeed = slowSpeedValue;
        runSpeed = slowSpeedValue;
        moveSpeed = slowSpeedValue;

        yield return new WaitForSeconds(duration);

        walkSpeed = originalWalkSpeed;
        runSpeed = originalRunSpeed;
        moveSpeed = isRunning ? runSpeed : walkSpeed;

        isSlowed = false;
        slowCoroutine = null;

        // 슬로우 캔버스 끄기
        if (slowCanvas != null)
            slowCanvas.SetActive(false);
    }

    //  플레이어 얼림 처리
    public void FreezePlayer(float duration)
    {
        if (isFrozen) return;

        isFrozen = true;
        moveSpeed = 0f;
        _rigidbody.velocity = Vector3.zero;
        _animator.SetFloat("Blend", 0f); // 애니메이션 정지

        if (freezeCanvas != null)
            freezeCanvas.SetActive(true); // 얼음 UI 켜기

        StartCoroutine(UnfreezeAfterDelay(duration));
    }

    // UnfreezeAfterDelay 수정
    private IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isFrozen = false;
        moveSpeed = isRunning ? runSpeed : walkSpeed;

        if (freezeCanvas != null)
            freezeCanvas.SetActive(false); // 얼음 UI 끄기
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {


            itemCount++;
            Debug.Log("아이템 획득! 현재 개수: " + itemCount);
            Destroy(other.gameObject);
        }
    }
}