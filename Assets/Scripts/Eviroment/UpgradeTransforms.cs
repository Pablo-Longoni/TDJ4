using TMPro;
using UnityEngine;
using System.Collections;
public class UpgradeTransforms : MonoBehaviour
{
    [SerializeField] public PlayerTransformation _playerTransformation;
    [SerializeField] private GameObject _item;
 //   [SerializeField] private TextMeshProUGUI _textDestination;
  //  [SerializeField] private Canvas _canvas;
 //   [SerializeField] private GameObject _itemIconPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._flip);
        _playerTransformation.TransformUpgrade();
        _item.SetActive(false);
        /*  // --- calcular posición inicial en coordenadas locales del Canvas ---
          RectTransform canvasRect = _canvas.transform as RectTransform;
          Camera cam = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : (_canvas.worldCamera != null ? _canvas.worldCamera : Camera.main);

          Vector2 startLocal;
          Vector3 itemScreen = Camera.main.WorldToScreenPoint(_item.transform.position);
          RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, itemScreen, cam, out startLocal);

          // --- calcular posición objetivo en coordenadas locales del Canvas ---
          Vector3 targetScreen = RectTransformUtility.WorldToScreenPoint(cam, _textDestination.rectTransform.position);
          RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, targetScreen, cam, out Vector2 targetLocal);

          // --- instanciar icono en Canvas ---
          GameObject iconGO = Instantiate(_itemIconPrefab, _canvas.transform);
          RectTransform iconRect = iconGO.GetComponent<RectTransform>();
          iconRect.anchoredPosition = startLocal;

          // --- ocultar visual del item 3D sin desactivar su GameObject  ---
          var rend = _item.GetComponent<Renderer>();
          if (rend) rend.enabled = false;
          var collider = _item.GetComponent<Collider>();
          if (collider) collider.enabled = false;

          StartCoroutine(MoveIcon(iconRect, targetLocal, 0.3f));*/
    }

    private IEnumerator MoveIcon(RectTransform icon, Vector2 targetLocalPos, float duration)
    {
        float elapsed = 0f;
        Vector2 start = icon.anchoredPosition;

        Debug.Log($"Empieza a moverse el icono. start={start} target={targetLocalPos}");

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curvedT = Mathf.Sin(t * Mathf.PI * 0.5f); // ease out
            icon.anchoredPosition = Vector2.Lerp(start, targetLocalPos, curvedT);

            Debug.Log($"Moviendo icono t={t:0.00} pos={icon.anchoredPosition}");

            yield return null;
        }
       
        // garantizar posición final exacta
        icon.anchoredPosition = targetLocalPos;
        Debug.Log("Termina de moverse el icono");
        this.gameObject.SetActive(false);
        Destroy(icon.gameObject);
    }
}

  