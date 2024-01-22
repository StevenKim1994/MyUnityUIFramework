using System;
using System.Collections.Generic;

public class BTSequenceNode : IBTNode
{
    private List<IBTNode> BTNodeChildList;

    public BTSequenceNode(List<IBTNode> InputBTNodeChildList)
    {
        BTNodeChildList = InputBTNodeChildList;
    }

    public IBTNode.EBTNodeState Evaluate()
    {
        if(BTNodeChildList == null || BTNodeChildList.Count == 0)
        {
            return IBTNode.EBTNodeState.Failed;
        }

        for(int i = 0; i< BTNodeChildList.Count; ++i)
        {
            switch(BTNodeChildList[i].Evaluate())
            {
                case IBTNode.EBTNodeState.Progress:
                    return IBTNode.EBTNodeState.Progress;
                case IBTNode.EBTNodeState.Success:
                    continue;
                case IBTNode.EBTNodeState.Failed:
                    return IBTNode.EBTNodeState.Failed;
            }
        }

        return IBTNode.EBTNodeState.Success;
    }
}
