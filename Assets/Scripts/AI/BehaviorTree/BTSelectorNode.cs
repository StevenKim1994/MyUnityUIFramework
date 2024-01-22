using System;
using System.Collections.Generic;

public class BTSelectorNode : IBTNode
{
    private List<IBTNode> BTNodeChildList;

    public BTSelectorNode(List<IBTNode> InputBTNodeChildList)
    {
        BTNodeChildList = InputBTNodeChildList;
    }

    public IBTNode.EBTNodeState Evaluate()
    {
        if(BTNodeChildList == null)
        {
            return IBTNode.EBTNodeState.Failed;
        }

        for(int i =0;i<BTNodeChildList.Count;++i)
        {
            switch(BTNodeChildList[i].Evaluate())
            {
                case IBTNode.EBTNodeState.Progress:
                    return IBTNode.EBTNodeState.Progress;

                case IBTNode.EBTNodeState.Success:
                    return IBTNode.EBTNodeState.Success;
            }
        }

        return IBTNode.EBTNodeState.Failed;
    }
}
