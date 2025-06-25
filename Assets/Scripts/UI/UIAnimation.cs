using UnityEngine;
using System.Collections;
public class UIAnimation : MonoBehaviour
{
    [SerializeField] private float _duaration;
    [SerializeField] private float _delay;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private RectTransform _target;
    [SerializeField] private Vector2 _startingPoint;
    [SerializeField] private Vector2 _endPoint;
    private bool _menuOnDisplay = false;

    public void DisplayMenu()
    {
        _menuOnDisplay = !_menuOnDisplay;
        if(_menuOnDisplay)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCorutine(_startingPoint, _endPoint));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCorutine(_endPoint, _startingPoint));
    }

    IEnumerator FadeInCorutine(Vector2 a, Vector2 b)
    {
        Vector2 _startingPoint = a;
        Vector2 _endPoint = b;
        float _elapsed = 0;
        while(_elapsed <= _delay)
        {
            _elapsed += Time.deltaTime;
            yield return null;
        }

        _elapsed = 0;

        while (_elapsed <= _duaration)
        {
            float _percentage = _elapsed / _duaration;
            float _curvePercenage = _animationCurve.Evaluate(_percentage);
            _elapsed += Time.deltaTime;
            Vector2 _currentPosition = Vector2.LerpUnclamped(_startingPoint, _endPoint, _curvePercenage);
            _target.anchoredPosition = _currentPosition;
            yield return null;
        }

        _target.anchoredPosition = _endPoint;
    }

 /*   IEnumerator FadeOutCorutine()
    {

    }*/
}
