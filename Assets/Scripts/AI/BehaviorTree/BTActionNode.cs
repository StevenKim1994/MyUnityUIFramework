using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTActionNode : IBTNode
{
    private Func<IBTNode.EBTNodeState> OnUpdate;

    public BTActionNode(Func<IBTNode.EBTNodeState> InputOnUpdate)
    {
        this.OnUpdate = InputOnUpdate;
    }

    public IBTNode.EBTNodeState Evaluate()
    {
        if(this.OnUpdate != null)
        {
            return this.OnUpdate.Invoke();
        }

        return IBTNode.EBTNodeState.Failed; 
    }
}
