using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Cinemachine;
using System.Collections;
public class StageUnlock : MonoBehaviour
{


    [SerializeField] private CinemachineCamera _stageCamera;
    [SerializeField] private CinemachineCamera _canvasCamera;
    [SerializeField] private Canvas[] _canvas;
    [SerializeField]  private Canvas _mainCanvas;
    private bool _isCanvasCamera;

    [SerializeField] private CubeMenu _cubeMenu;
    [SerializeField] private GameObject _cube;
    public float _fadeDuration = 1f;
      public Renderer _cubeRenderer;
      private Material _cubeMaterial;
      private Color originalColor;
     
    void Start()
    {
        _isCanvasCamera = true;
        _cubeMaterial = _cubeRenderer.material;
        originalColor = _cubeMaterial.color;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (_isCanvasCamera)
        {
            foreach (Canvas _canvas in _canvas)
            {
                _canvas.gameObject.SetActive(false);
            }
        }
    }

    public void GoToSelector()
    {
        _cubeMenu.StopRotation();
        _cube.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
     //   FadeIn();
        _isCanvasCamera = false;
        foreach (Canvas _canvas in _canvas)
        {
            _canvas.gameObject.SetActive(true);
        }

        _cubeMenu.onStage = true;
        _canvasCamera.Priority = 1;
        _stageCamera.Priority = 2;
      //  StartCoroutine(ChangeRenderMode());
    }

    public void GoToMenuCanvas()
    {
      //  FadeOut();
        _isCanvasCamera = true;
        foreach (Canvas _canvas in _canvas)
        {
            _canvas.gameObject.SetActive(false);
        }
        _cubeMenu.onStage = false;
        _canvasCamera.Priority = 2;
        _stageCamera.Priority = 1;
       // StartCoroutine(ChangeRenderMode());
    }

    //change canvas render
    IEnumerator ChangeRenderMode()
    {
        if(_mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            yield return new WaitForSeconds(0.5f);
            _mainCanvas.renderMode = RenderMode.WorldSpace;
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            _mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }
    //fades 
    public void FadeOut()
    {
        StartCoroutine(FadeTo(1f));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTo(1f));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = _cubeMaterial.color.a;
        float time = 0f;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            Color newColor = _cubeMaterial.color;
            newColor.a = alpha;
            _cubeMaterial.color = newColor;
            yield return null;
        }
        Debug.Log("fade terminado");
        Color _finalColor = _cubeMaterial.color;
        _finalColor.a = targetAlpha;
        _cubeMaterial.color = _finalColor;
    }
}
