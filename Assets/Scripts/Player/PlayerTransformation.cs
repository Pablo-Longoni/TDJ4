using TMPro;
using UnityEngine;
public class PlayerTransformation : MonoBehaviour
{
    public int _totalTrans = 3;
    public int _currentTrans = 0;

    public CameraChange _cameraChange;
    public TextMeshProUGUI _textTrans;
    void Start()
    {
        _textTrans.text = "Transformations: " + _currentTrans + "/" + _totalTrans;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTrans >= _totalTrans)
        {
            _cameraChange._canChange = false;
        }
    }

    public void PlayerTransformed()
    {
        _currentTrans = _currentTrans + 1;
        _textTrans.text = "Transformations: " + _currentTrans + "/" + _totalTrans; 
        Debug.Log("Transformations: " + _currentTrans);
    }
}
