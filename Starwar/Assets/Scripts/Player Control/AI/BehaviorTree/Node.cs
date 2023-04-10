using System.Collections.Generic;

namespace BehaviorTree
{
    public enum BehaviorNodeState
    {
        NONE,
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class BehaviorNode
    {
        protected BehaviorNodeState state;

        public BehaviorNode parent;
        protected List<BehaviorNode> children = new List<BehaviorNode>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public BehaviorNode()
        {
            parent = null;
        }
        public BehaviorNode(List<BehaviorNode> children)
        {
            foreach (BehaviorNode child in children)
                _Attach(child);
        }

        private void _Attach(BehaviorNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual BehaviorNodeState Evaluate() => BehaviorNodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            BehaviorNode node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            BehaviorNode node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }

}
