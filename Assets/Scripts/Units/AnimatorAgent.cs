using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorAgent : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private static int s_isMoveSelf;
    private static string s_isMoveSelfName = "isMoveSelf";

    public int IsMoveSelf => s_isMoveSelf;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        s_isMoveSelf = Animator.StringToHash(s_isMoveSelfName);
    }

    public void SetBool(int id, bool value)
    {
        _animator.SetBool(id, value);
    }
}
