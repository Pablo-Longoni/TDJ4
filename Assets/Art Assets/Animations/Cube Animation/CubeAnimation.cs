using UnityEngine;
using System.Collections;

public class CubeAnimation : MonoBehaviour
{

 //   public float fallThreshold = -1f; // velocidadY m�nima para detectar ca�da
  //  public float minFallDuration = 0.2f; // tiempo m�nimo cayendo para hacer stretch
    public float stretchAmount = 1.3f; // cu�nto se estira al caer
    public float squashAmount = 0.8f;  // cu�nto se aplasta al aterrizar
  //  public float stretchDuration = 0.1f; // tiempo que dura el estiramiento
    public float squashDuration = 0.1f;  // tiempo que dura el squash
    public float rayDistance = 0.5f;
 //   private Rigidbody rb;

    private Vector3 originalScale;
 //   private bool isFalling = false;
//    private bool wasGroundedLastFrame = true;
    private bool isGrounded = false;
    private Coroutine currentRoutine;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject _dustPrefab;
    [SerializeField] private GameObject _miniDustPrefab;

    private bool dustSpawned = false;
  //  public float squashCooldown = 0.3f;
    private float lastSquashTime = -10f;
    private bool ignoreGroundCheck = false;
    private float fallStartTime = 0f;

    private bool isActuallyFalling;
    private bool firstFrameChecked = false;

  //  private CameraChange _camera;
    [SerializeField] private GameObject _cube; 
    private bool canStretchAndSquash = true;
    void Start()
    {
     //   _camera = GameObject.FindAnyObjectByType<CameraChange>();
       /* rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; */
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
      //  CheckGrounded();
    }

  /*  void LateUpdate()
    {
        if (!canStretchAndSquash)
            transform.localScale = originalScale;
    }*/
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
        StopActiveRoutine();
        Vector3 stretched = new Vector3(originalScale.x, originalScale.y * stretchAmount, originalScale.z);
        currentRoutine = StartCoroutine(ScaleOverTime(transform.localScale, stretched, 0.1f));
        yield return currentRoutine;
     //   Debug.Log("Stretch");
    }

    IEnumerator Squash()
    {
        StopActiveRoutine();
        ignoreGroundCheck = true;
        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._playerLand); 
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

        Vector3 squashed = new Vector3(originalScale.x, originalScale.y * squashAmount, originalScale.z);

        yield return StartCoroutine(ScaleOverTime(transform.localScale, squashed, squashDuration));
        yield return StartCoroutine(ScaleOverTime(squashed, originalScale, 0.15f));

        ignoreGroundCheck = false;
        dustSpawned = false;
    }

    IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }

    private void OnTriggerExit(Collider other)
    {
        if (canStretchAndSquash && other.gameObject.layer == LayerMask.NameToLayer("Enviroment"))
        {
            bool groundBelow = Physics.Raycast(groundCheck.position, Vector3.down, rayDistance, groundLayer);
            if (!isActuallyFalling && !groundBelow)
            {
                isActuallyFalling = true;
                fallStartTime = Time.time;
                StopActiveRoutine();
                currentRoutine = StartCoroutine(Stretch());
             //   Debug.Log("on trigger exit");
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
             //   Debug.Log("on trigger enter");
            }
        }
    }
    
    //se llama en CubeRotation
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
        StartCoroutine(ScaleObject(_cube, Vector3.zero, 0.7f));
        Debug.Log("EnterPortalAnim");
    }

    IEnumerator ScaleObject(GameObject obj, Vector3 targetScale, float duration)
    {
        float t = 0f;
        Vector3 startScale = obj.transform.localScale;
        StopActiveRoutine();
        while (t < duration)
        {
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
 