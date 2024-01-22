using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRunner 
{
    private IBTNode RootNode;
    public BTRunner(IBTNode InputRootNode)
    {
        RootNode = InputRootNode;
    }

    public void Operate()
    {
        RootNode.Evaluate();
    }
}
