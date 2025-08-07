using UnityEngine;
using System.Collections;

public class CubeAnimation : MonoBehaviour
{

    public float fallThreshold = -1f; // velocidadY m�nima para detectar ca�da
    public float minFallDuration = 0.2f; // tiempo m�nimo cayendo para hacer stretch
    public float stretchAmount = 1.3f; // cu�nto se estira al caer
    public float squashAmount = 0.8f;  // cu�nto se aplasta al aterrizar
  //  public float stretchDuration = 0.1f; // tiempo que dura el estiramiento
    public float squashDuration = 0.1f;  // tiempo que dura el squash
    public float rayDistance = 0.5f;
    private Rigidbody rb;

    private Vector3 originalScale;
    private bool isFalling = false;
    private bool wasGroundedLastFrame = true;
    private bool isGrounded = false;
    private Coroutine currentRoutine;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject _dustPrefab;
    [SerializeField] private GameObject _miniDustPrefab;

    private bool dustSpawned = false;
    public float squashCooldown = 0.3f;
    private float lastSquashTime = -10f;
    private bool ignoreGroundCheck = false;
    private float fallStartTime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // <--- importante
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
       // CheckGrounded();
    }

    void Update()
    {
        CheckGrounded();

        bool justLanded = isGrounded && !wasGroundedLastFrame;
        bool justStartedFalling = !isGrounded && wasGroundedLastFrame;

        if (justStartedFalling)
        {
            isFalling = true;
            fallStartTime = Time.time;
            StopActiveRoutine();
            currentRoutine = StartCoroutine(Stretch());
        }

        if (justLanded && isFalling /*&& Time.time - lastSquashTime > squashCooldown*/)
        {
            isFalling = false;
            lastSquashTime = Time.time;
            StopActiveRoutine();
            currentRoutine = StartCoroutine(Squash());
        }

        wasGroundedLastFrame = isGrounded;
    }

    void CheckGrounded()
    {
        if (ignoreGroundCheck)
            return;
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, rayDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector3.down * rayDistance, isGrounded ? Color.green : Color.red);
    }

    void StopActiveRoutine()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
    }

    IEnumerator Stretch()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y * stretchAmount, originalScale.z);
        yield return null;
        Debug.Log("Stretch");
    }

    IEnumerator Squash()
    {
        ignoreGroundCheck = true;

        float fallDuration = Time.time - fallStartTime;
        Debug.Log("Fall Duration: " + fallDuration);
     //   float offSetY = 2f;
        // Instancia partículas según duración de la caída
        if (!dustSpawned)
        {
            if (fallDuration >= 1.2f)
            {
                Instantiate(_dustPrefab, groundCheck.position, _dustPrefab.transform.rotation);
            }
            else
            {
                Instantiate(_miniDustPrefab, groundCheck.position, _miniDustPrefab.transform.rotation);
            }
            dustSpawned = true;
        }

        transform.localScale = new Vector3(originalScale.x, originalScale.y * squashAmount, originalScale.z);
        yield return new WaitForSeconds(squashDuration);
        transform.localScale = originalScale;

        yield return new WaitForSeconds(.5f); // tiempo extra sin hacer CheckGrounded
        transform.localScale = originalScale;
        ignoreGroundCheck = false;
        dustSpawned = false;
        Debug.Log("Squash");
    }
}
