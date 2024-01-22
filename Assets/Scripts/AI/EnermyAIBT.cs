using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyAIBT : MonoBehaviour
{
    private float DetectRange; 
    private float AttackRange; 
    private float MovementSpeed;
    private float DectectAngle;
    private Vector3 OriginPos;
    private Transform Target;
    private BTRunner BTRoot;
    private bool IsAttack;
    public Transform Player;

    private void Start()
    {
        OriginPos = transform.position;
        DetectRange = 10.0f;
        DectectAngle = 90.0f;
        AttackRange = 5.0f;
        MovementSpeed = 1.5f;

        BTRoot = new BTRunner(EnermyAIBTFunc());
    }

    private void Update()
    {
        BTRoot.Operate();
    }

    // Attack Sequence 
    private IBTNode.EBTNodeState CheckAttack()
    {
        if(IsAttack == true)
        {
            return IBTNode.EBTNodeState.Progress;
        }

        return IBTNode.EBTNodeState.Success;
    }

    private IBTNode.EBTNodeState CheckAttackInRange()
    {
        if(Target != null)
        {
            if(Vector3.SqrMagnitude(Target.position - transform.position) < AttackRange * AttackRange) // 범위안에 들면
            {
                return IBTNode.EBTNodeState.Success;
            }
        }

        return IBTNode.EBTNodeState.Failed;
    }

    private IBTNode.EBTNodeState DoAttack()
    {
        if(Target != null)
        {
           // IsAttack = true; // 원래는 여기서 완료처리를 하는거보다 특정 애니메이션이 끝났을때 콜백을 해주는 방식으로 수정해야할듯.
            return IBTNode.EBTNodeState.Success;
        }

        return IBTNode.EBTNodeState.Failed;
    }

    // Detect Sequence
    private IBTNode.EBTNodeState CheckDetect()
    {
        var OverlapCollider = Physics.OverlapSphere(transform.position, DetectRange, LayerMask.GetMask("Player"));

        if(OverlapCollider != null && OverlapCollider.Length > 0)
        {
            Target = OverlapCollider[0].transform;
            Vector3 DirToTarget = (Target.position - transform.position).normalized; // 탐지된 오브젝트와 방향단위벡터

            if(Vector3.Dot(transform.forward, DirToTarget) > Mathf.Cos((DectectAngle/2.0f) * Mathf.Deg2Rad)) // 시야각안에 포함된다면
            {
                return IBTNode.EBTNodeState.Success;
            }
        }
        Target = null;
        return IBTNode.EBTNodeState.Failed;
    }

    private IBTNode.EBTNodeState ChaseTarget()
    {
        if(Target != null)
        {
            if(Vector3.SqrMagnitude(Target.position - transform.position) < AttackRange * AttackRange) // 공격범위 안에 들고
            { // 공격범위 안에 든다면 
                return IBTNode.EBTNodeState.Success;
            }

            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * MovementSpeed);
            return IBTNode.EBTNodeState.Progress;
        }

        return IBTNode.EBTNodeState.Failed;
    }

    private IBTNode.EBTNodeState DoMoveOriginPosition()
    {
        if(Vector3.SqrMagnitude(OriginPos - transform.position) < float.Epsilon * float.Epsilon)
        {
            return IBTNode.EBTNodeState.Success;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OriginPos, Time.deltaTime * MovementSpeed);
            return IBTNode.EBTNodeState.Progress;
        }
    }


    public IBTNode EnermyAIBTFunc()
    {
        return new BTSelectorNode
        (
            new List<IBTNode>()
            {
                new BTSequenceNode // 공격
                (
                    new List<IBTNode>()
                    {
                        new BTActionNode(CheckAttack),
                        new BTActionNode(CheckAttackInRange),
                        new BTActionNode(DoAttack),
                    }
                ),
                new BTSequenceNode // 추적
                (
                    new List<IBTNode>()
                    {
                        new BTActionNode(CheckDetect),
                        new BTActionNode(ChaseTarget),
                    }
                ),
                new BTActionNode(DoMoveOriginPosition), //제자리
            }
        );
    }

    private Vector3 DirFromAngle(float InputAngle)
    {
        InputAngle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(InputAngle * Mathf.Deg2Rad), 0, Mathf.Cos(InputAngle * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        // Detect Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position , DetectRange);


        // AttackRange Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        if(IsAttack == true)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }

        Vector3 leftBound = DirFromAngle(-DectectAngle / 2.0f);
        Vector3 rightBound = DirFromAngle(DectectAngle / 2.0f);
        Gizmos.DrawLine(transform.position, transform.position + leftBound * DetectRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBound * DetectRange);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * DetectRange);

        if (Player != null)
        {
            Debug.ClearDeveloperConsole();
            Vector3 DirToTarget = (Player.position - transform.position).normalized; // 탐지된 오브젝트와 방향단위벡터

            // 자신의 forward 방향벡터와 목표물의 방향 벡터를 내적(Dot)하면 목표물이 자신의 앞에 있는지 같은지 (내적값이 0이면) 알수 있다.
            if (Vector3.Dot(transform.forward, DirToTarget) > 0) // 자신의 앞에 있으면
            {
                Debug.Log("Front");
            }
            else if(Vector3.Dot(transform.forward, DirToTarget) == 0.0f) // 중간에 잇으면
            {
                Debug.Log("Center");
            }
            else // 뒤에 있으면 
            {
                Debug.Log("Back");
            }

            // 자신의 Right 방향벡터와 목표물의 방향 벡터를 내적(Dot)하면 목표물이 오른쪽에 있는지 왼쪽에 있는지 중간에 있는지 (내적값이 0이면) 알수 있다.
            if (Vector3.Dot(transform.right, DirToTarget) > 0) 
            {
                Debug.Log("Right, 2");
            }
            else if(Vector3.Dot(transform.right, DirToTarget) == 0.0f) // 중간에 잇으면
            {
                Debug.Log("Center, 2");
            }
            else
            {
                Debug.Log("Left, 2");
            }

            
        }
    }
}
