using System.Collections.Generic;

namespace BehaviorTree
{
    public class Selector : BehaviorNode
    {
        public Selector() : base() { }
        public Selector(List<BehaviorNode> children) : base(children) { }

        public override BehaviorNodeState Evaluate()
        {
            foreach (BehaviorNode node in children)
            {
                switch (node.Evaluate())
                {
                    case BehaviorNodeState.FAILURE:
                        continue;
                    case BehaviorNodeState.SUCCESS:
                        //TODO (Change continue)
                        state = BehaviorNodeState.SUCCESS;
                        return state;
                    case BehaviorNodeState.RUNNING:
                        //TODO (Change continue)
                        state = BehaviorNodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = BehaviorNodeState.FAILURE;
            return state;
        }

    }

}
