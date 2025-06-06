using TMPro;
using UnityEngine;
using System.Collections;
public class PlayerTransformation : MonoBehaviour
{
    public int _totalTrans = 3;
    public int _currentTrans = 0;

    public CameraChange _cameraChange;
    public TextMeshProUGUI _textTrans;

    private bool _isBlinking = false;
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

            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                if (!_isBlinking)
                {
                    StartCoroutine(BlinkText());
                }
            }
        } 
    }

    public void PlayerTransformed()
    {
        _currentTrans = _currentTrans + 1;
        _textTrans.text = "Transformations: " + _currentTrans + "/" + _totalTrans; 
        Debug.Log("Transformations: " + _currentTrans);
    }

    IEnumerator BlinkText()
    {
        _isBlinking = true;

        for (int i = 0; i < 4; i++) 
        {
            _textTrans.enabled = false;
            yield return new WaitForSeconds(0.15f);
            _textTrans.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        _isBlinking = false;
    }
}
