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
 //   private bool isFalling = false;
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

    private bool isActuallyFalling;
    private bool firstFrameChecked = false;

    private CameraChange _camera;
    [SerializeField] private GameObject _cube;
    private bool canStretchAndSquash = true;
    void Start()
    {
        _camera = GameObject.FindAnyObjectByType<CameraChange>();
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; 
        originalScale = transform.localScale;
        isActuallyFalling = true;
    }



    void Update()
    {
        if (!firstFrameChecked)
        {
            firstFrameChecked = true;

            bool groundedAtStart = Physics.Raycast(groundCheck.position, Vector3.down, rayDistance, groundLayer);

            if (!groundedAtStart)
            {
                isActuallyFalling = true;
                fallStartTime = Time.time;
                StopActiveRoutine();
                currentRoutine = StartCoroutine(Stretch());
            }
        }
        CheckGrounded();

       bool justLanded = isGrounded && !wasGroundedLastFrame && isActuallyFalling;
       bool justStartedFalling = !isGrounded && wasGroundedLastFrame;
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
    }

    IEnumerator Squash()
    {
        ignoreGroundCheck = true;

        float fallDuration = Time.time - fallStartTime;
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

        yield return new WaitForSeconds(.5f); 
        transform.localScale = originalScale;
        ignoreGroundCheck = false;
        dustSpawned = false;
        Debug.Log("Squash");
    }

    private void OnTriggerExit(Collider other)
    {
        if (canStretchAndSquash && other.gameObject.layer == LayerMask.NameToLayer("Enviroment"))
        {
            if (!isActuallyFalling && rb.linearVelocity.y < fallThreshold)
            {
                isActuallyFalling = true;
                fallStartTime = Time.time;
                StopActiveRoutine();
                currentRoutine = StartCoroutine(Stretch());
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (canStretchAndSquash && other.gameObject.layer == LayerMask.NameToLayer("Enviroment"))
        {
            if (isActuallyFalling)
            {
                isActuallyFalling = false;
                lastSquashTime = Time.time;
                StopActiveRoutine();
                currentRoutine = StartCoroutine(Squash());
           //     Debug.Log(">>> TOCÓ SUELO");
            }
        }
    }
    
    public void IgnoreStretchAndSquash(float duration)
    {
        StartCoroutine(IgnoreTemporarily(duration));
    }

    private IEnumerator IgnoreTemporarily(float duration)
    {
        canStretchAndSquash = false;
        StopActiveRoutine();
        transform.localScale = originalScale; // reset seguro
        yield return new WaitForSeconds(duration);
        canStretchAndSquash = true;
    }

    // Entering portal and Teleport / cube deformation

    public void EnterPortalAnim()
    {
        canStretchAndSquash = false;
        Vector3 originalScale = _cube.transform.localScale;
        if (this != null)
        {
            StartCoroutine(ScaleObject(_cube, Vector3.zero, 0.7f));
        }
    }

    IEnumerator ScaleObject(GameObject obj, Vector3 targetScale, float duration)
    {
        float t = 0f;
        Vector3 startScale = obj.transform.localScale;

        while (t < duration)
        {
            StopCoroutine(Stretch());
            t += Time.deltaTime;
            float normalized = t / duration;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, normalized);
            yield return null;
        }
    }
    public void ExitPortalAnim()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleObject(_cube, originalScale, 0.7f));
        canStretchAndSquash = true;
    }
}
 