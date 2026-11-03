using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private float _updateInterval = 0.5f;

    private float _timer;
    private int _frames;

    private void Update()
    {
        _frames++;
        _timer += Time.unscaledDeltaTime;

        if (_timer >= _updateInterval)
        {
            float fps = _frames / _timer;
            float milliseconds = (_timer / _frames) * 1000f;

            _fpsText.text = $"{fps:0} FPS ({milliseconds:0.0} ms)";

            _frames = 0;
            _timer = 0f;
        }
    }
}
