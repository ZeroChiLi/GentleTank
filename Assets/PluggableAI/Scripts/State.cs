using UnityEngine;

//包含要执行动作，下一个动作的决定
[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject 
{
    public Action[] actions;                        //动作：巡逻动作、
    public Transition[] transitions;                //通过决定，选择下一种动作决定。
    public Color sceneGizmoColor = Color.gray;      //拿来渲染eyes的Gizmos颜色
    
    //每一帧更新状态，在StateController的OnUpdate中调用。
    public void UpdateState(StateController controller)
    {
        DoActions(controller);                      //执行动作
        CheckTransition(controller);                //检测转换状态
    }

    //顺序执行动作列表的动作。
    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
            actions[i].Act(controller);
    }

    //检查所有转换状态，并改变状态
    private void CheckTransition(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            //这里条件转换只有两个，所以直接用Bool类型来判断。当然也可以有多种条件转换。
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
                controller.TransitionToState(transitions[i].trueState);
            else
                controller.TransitionToState(transitions[i].falseState);
        }
    }

}
