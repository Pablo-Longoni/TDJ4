using System.Collections;
using UnityEngine;
public class PlayerAnimationController : MonoBehaviour
{
    private bool _isLocked;
    private string _currentAnimation;
    [SerializeField] private Animator _animator;
    public bool PlayAnimation(string animationName, bool lockAnimation = false, bool force = false)
    {
        if (_isLocked && !force)
            return false;

        if (_currentAnimation == animationName)
            return true;

        _currentAnimation = animationName;
        _animator.Play(animationName);

        _isLocked = lockAnimation;

        if (lockAnimation)
            StartCoroutine(WaitForAnimation(animationName));

        return true;
    }

    public void LockAnimation()
    {
        _isLocked = true;
    }


    public void UnlockAnimation()
    {
        _isLocked = false;
    }

    private IEnumerator WaitForAnimation(string animationName)
    {
        // Espera a que el Animator entre realmente en ese estado.
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // Espera hasta que termine.
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        UnlockAnimation();
    }
}
