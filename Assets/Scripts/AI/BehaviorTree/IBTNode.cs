
public interface IBTNode 
{
    public enum EBTNodeState
    {
        Progress,
        Success,
        Failed,
    }

    public EBTNodeState Evaluate();
}
