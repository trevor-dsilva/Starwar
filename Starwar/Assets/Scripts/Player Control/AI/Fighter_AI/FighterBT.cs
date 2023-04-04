using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorTree;

public class FighterBT : Tree
{

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
               // new CheckEnemyInAttackRange(this.transform),
                //new TaskAttack(this.transform)
            }),
            //TODO :Add attack behavior
            new Sequence(new List<Node>
            {
                //new CheckEnemyInFOVRange(transform),
               // new TaskGoToTarget(transform),
               
            }),
       // new TaskPatrol(transform, waypoints)
        }); ;

        return root;
    }
}
