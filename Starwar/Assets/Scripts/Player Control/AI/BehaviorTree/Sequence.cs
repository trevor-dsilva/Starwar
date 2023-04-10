using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : BehaviorNode
    {
        public Sequence() : base() { }
        public Sequence(List<BehaviorNode> children) : base(children) { }

        public override BehaviorNodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (BehaviorNode node in children)
            {
                switch (node.Evaluate())
                {
                    //TODO
                    case BehaviorNodeState.NONE:
                    case BehaviorNodeState.SUCCESS:
                        break;
                    case BehaviorNodeState.FAILURE:
                        state = BehaviorNodeState.FAILURE;
                        return state;
                    case BehaviorNodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                }
            }

            state = anyChildIsRunning ? BehaviorNodeState.RUNNING : BehaviorNodeState.SUCCESS;
            return state;
        }

    }

}
