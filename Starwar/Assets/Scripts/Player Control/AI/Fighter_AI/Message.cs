using BehaviorTree;
using UnityEngine;

public class Message : BehaviorNode
{
    public override BehaviorNodeState Evaluate()
    {
        Debug.Log("Message");
        return BehaviorNodeState.RUNNING;
    }
}
