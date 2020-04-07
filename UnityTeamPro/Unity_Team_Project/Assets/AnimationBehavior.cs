using UnityEngine;

public class AnimationBehavior : StateMachineBehaviour
{
    public event Delegate<AnimatorStateInfo, int> Enter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enter?.Invoke(stateInfo, layerIndex);
    }
}
