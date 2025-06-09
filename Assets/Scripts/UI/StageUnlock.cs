using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class StageUnlock : MonoBehaviour
{

    public Button[] _stageButtons;
    public TextMeshProUGUI[] _stageText;
    void Start()
    {
        int _stageAt = PlayerPrefs.GetInt("Stage", 1);
        Debug.Log("Stage: " +  _stageAt); 
        for (int i = 0; i < _stageButtons.Length; i++)
        {
            if(i + 1 > _stageAt)
            {
                bool isUnlocked = (i < _stageAt);
                _stageButtons[i].interactable = isUnlocked;

                _stageText[i].color = isUnlocked ? Color.white : Color.grey;

                ChangeFont changeFont = _stageButtons[i].GetComponent<ChangeFont>();
                if (changeFont != null)
                {
                    changeFont.isLocked = !isUnlocked;
                }
            }
        }
    }


    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
