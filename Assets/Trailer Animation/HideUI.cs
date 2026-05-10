using UnityEngine;
public class HideUI : MonoBehaviour
{
    public GameObject[] _canvasUI;
    public bool _isHidden = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _isHidden = !_isHidden;

            if(_isHidden)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }


    private void Hide()
    {
        foreach (GameObject go in _canvasUI)
        {
            go.SetActive(false);
        }
    }

    private void Show()
    {
        foreach (GameObject go in _canvasUI)
        {
            go.SetActive(true);
        }
    }
}
