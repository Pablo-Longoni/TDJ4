using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class TutorialDisplay : MonoBehaviour
{
    [SerializeField] public GameObject _wasdImage;
    [SerializeField] public GameObject _spaceBarImage;
    [SerializeField] public GameObject _mouseClickImage;
    [SerializeField] public GameObject _mouseMoveImage;
    [SerializeField] public GameObject _mouseMiddleImage;
    private bool _didClick = false;
    private bool _didMove = false;
    private bool _didZoom = false;
    private bool _didTrans = false;
    private bool _canDetectMovement = false;
    private Vector3 _lastMousePosition;

    public string _scene;
    void Start()
    {
        _scene = SceneManager.GetActiveScene().name; 

        if (_scene == "Level2")
        {
            TransformTutorial();
        }
        else if (_scene == "Level3")
        {
            MouseTutorial();
        }

        _lastMousePosition = Input.mousePosition;
    }
    void Update()
    {
        if (_scene == "Level2")
        {
            TransformTutorial();
        }
        else if (_scene == "Level3")
        {
            MouseTutorial();
        }
    }

    public void TransformTutorial()
    {
        Debug.Log("Tutorial Transform");
   
        if (Input.GetKeyDown(KeyCode.Space) && !_didTrans)
        {
            _spaceBarImage.SetActive(false);
            _wasdImage.SetActive(true);
            _didTrans = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _wasdImage.SetActive(false);
        }
    }

    public void MouseTutorial()
    {
        Debug.Log("Tutorial mouse");
        // Paso 1: Hizo click izquierdo
        if (!_didClick && Input.GetMouseButtonDown(0))
        {
            _didClick = true;
            _mouseClickImage.SetActive(true);
            _mouseMoveImage.SetActive(true);
            StartCoroutine(EnableMovementDetection());
        }

        // Paso 2: Mueve el mouse mientras tiene el botón izquierdo
        if (_didClick && !_didMove && _canDetectMovement && Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Input.mousePosition;
            if (Vector3.Distance(currentMousePos, _lastMousePosition) > 5f)
            {
                _didMove = true;
                _mouseMoveImage.SetActive(false);
                _mouseClickImage.SetActive(false);
                _mouseMiddleImage.SetActive(true);
            }
            _lastMousePosition = currentMousePos;
        }

        // Paso 3: Hace zoom con la rueda del mouse
        if (_didMove && !_didZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                _didZoom = true;
                _mouseMiddleImage.SetActive(false);
            }
        }
    }

     public IEnumerator EnableMovementDetection()
    {
        yield return new WaitForSeconds(2.5f); 
        _canDetectMovement = true;
    }
}
