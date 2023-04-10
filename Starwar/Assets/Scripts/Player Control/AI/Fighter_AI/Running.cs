using BehaviorTree;

public class Running : BehaviorNode
{
    public override BehaviorNodeState Evaluate()
    {
        return BehaviorNodeState.RUNNING;
    }
}
