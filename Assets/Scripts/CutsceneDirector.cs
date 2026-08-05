using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDirector : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;
    public void PlayCinematic()
    {
        _director.Play();
    }
}
