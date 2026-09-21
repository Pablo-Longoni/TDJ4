using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;
using System.Collections;

public class SplashVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Animator _transitionAnimator;
    public float _transitionTime;

    private void Start()
    {
        _videoPlayer.prepareCompleted += OnVideoPrepared;
        _videoPlayer.loopPointReached += OnVideoFinished;

        _videoPlayer.Prepare();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        _transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        _videoPlayer.prepareCompleted -= OnVideoPrepared;
        _videoPlayer.loopPointReached -= OnVideoFinished;
    }
}