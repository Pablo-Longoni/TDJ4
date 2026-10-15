using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDirector : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayCinematic();
        }
    }
    public void PlayCinematic()
    {
        _director.Play();
    }
}
