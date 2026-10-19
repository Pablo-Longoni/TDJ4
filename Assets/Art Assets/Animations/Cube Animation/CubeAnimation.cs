using UnityEngine;
using System.Collections;

public class CubeAnimation : MonoBehaviour
{
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float rayDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Effects")]
    [SerializeField] private GameObject _dustPrefab;
    [SerializeField] private GameObject _miniDustPrefab;
    [SerializeField] private Vector3 _offSet = new Vector3(0, 1, 0);

    [Header("References")]
    [SerializeField] private GameObject _cube;
    [SerializeField] private PlayerAnimationController _playerAnimator;
    [SerializeField] private GameObject _fakeShadow;
    private Vector3 originalScale;

    private bool canStretchAndSquash = true;


    [SerializeField] private PlayerGroundDetection groundDetector;

    private bool isFalling;
    private float fallStartTime;

    void Start()
    {
        originalScale = transform.localScale;
    }


    private void Update()
    {
        if (!canStretchAndSquash)
            return;

        bool grounded = groundDetector.IsGrounded;

        if (!grounded && !isFalling)
        {
            StartFalling();
        }
    }

    public void OnGroundDetected()
    {
        if (!isFalling)
            return;

        Land();
    }

    private void StartFalling()
    {
        if (isFalling)
            return;

        isFalling = true;
        fallStartTime = Time.time;

        _playerAnimator.PlayAnimation("Fall", false, true);
        _playerAnimator.LockAnimation();
    }

    public void CallFalling()
    {
        StartFalling();
    }

 
    private void Land()
    {
        isFalling = false;

        float fallDuration = Time.time - fallStartTime;

        _playerAnimator.PlayAnimation("Land", true, true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.soundSource.PlayOneShot(
                AudioManager.Instance._playerLand
            );
        }

        SpawnLandingDust(fallDuration);
    }

    private void SpawnLandingDust(float fallDuration)
    {
        if (fallDuration >= 1.2f)
        {
            Instantiate(_dustPrefab ,groundCheck.position + _offSet, _dustPrefab.transform.rotation);
        }
        else
        {
            Instantiate( _miniDustPrefab,  groundCheck.position + _offSet, _miniDustPrefab.transform.rotation);
        }
    }

    // --------------------------------------------------
    // Ignore fall / landing temporarily
    // --------------------------------------------------

    public void IgnoreStretchAndSquash(float duration)
    {
        StartCoroutine(IgnoreTemporarily(duration));
    }

    private IEnumerator IgnoreTemporarily(float duration)
    {
        canStretchAndSquash = false;
        isFalling = false;

        transform.localScale = originalScale;

        yield return new WaitForSeconds(duration);

        canStretchAndSquash = true;
    }

    // --------------------------------------------------
    // Portal
    // --------------------------------------------------

    public void EnterPortalAnim(float duration)
    {
        _fakeShadow.SetActive(false);
        canStretchAndSquash = false;

        StartCoroutine(ScaleObject(_cube, Vector3.zero, duration) );

        Debug.Log("EnterPortalAnim");
    }

    private IEnumerator ScaleObject(
        GameObject obj,
        Vector3 targetScale,
        float duration)
    {
        float t = 0f;
        Vector3 startScale = obj.transform.localScale;

        while (t < duration)
        {
            t += Time.deltaTime;

            float normalized = t / duration;

            obj.transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                normalized
            );

            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    public void ExitPortalAnim()
    {
        transform.localScale = Vector3.zero;

        StartCoroutine(
            ScaleObject(_cube, originalScale, 0.7f)
        );

        canStretchAndSquash = true;
    }
}