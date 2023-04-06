using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private BehaviorNode _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract BehaviorNode SetupTree();
    }

}
