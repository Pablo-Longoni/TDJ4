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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._flip);
        }
      
        _playerTransformation.TransformUpgrade();
        _item.SetActive(false);
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

  