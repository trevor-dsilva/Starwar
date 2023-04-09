using BehaviorTree;
using UnityEngine;

public class Message : BehaviorNode
{
    public string message;
    public Message(string message = "Message")
    {
        this.message = message;
    }
    public override BehaviorNodeState Evaluate()
    {
        // Debug.Log(message);
        return BehaviorNodeState.RUNNING;
    }
}
