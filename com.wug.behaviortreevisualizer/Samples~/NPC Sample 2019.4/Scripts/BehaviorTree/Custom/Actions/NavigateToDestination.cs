using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace WUG.BehaviorTreeVisualizer
{
    public class NavigateToDestination : Node
    {
        private Vector3 m_TargetDestination;

        public NavigateToDestination()
        {
            Name = "Navigate";
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {

            if (GameManager.Instance == null || GameManager.Instance.NPC == null)
            {
                StatusReason = "GameManager or NPC is null";
                return NodeStatus.Failure;
            }

            if (IsFirstEvaluation)
            {
                GameObject destinationGO = GameManager.Instance.NPC.MyActivity == NavigationActivity.PickupItem ?  GameManager.Instance.GetClosestItem() : GameManager.Instance.GetNextWayPoint();

                if (destinationGO == null)
                {
                    StatusReason = $"Unable to find game object for {GameManager.Instance.NPC.MyActivity}";
                    return NodeStatus.Failure;
                }


                NavMesh.SamplePosition(destinationGO.transform.position, out NavMeshHit hit, 1f, 1);
                m_TargetDestination = hit.position;

                GameManager.Instance.NPC.MyNavMesh.SetDestination(m_TargetDestination);
                StatusReason = $"Starting to navigate to {destinationGO.transform.position}";
                
                return NodeStatus.Running;
            }

            float distanceToTarget = Vector3.Distance(m_TargetDestination, GameManager.Instance.NPC.transform.position);

            if (distanceToTarget < .25f)
            {
                StatusReason = $"Navigation ended. " +
                    $"\n - Evaluation Count: {EvaluationCount}. " +
                    $"\n - Target Destination: {m_TargetDestination}" +
                    $"\n - Distance to target: {Math.Round(distanceToTarget, 1)}";

                return NodeStatus.Success;
            }
            StatusReason = $"Distance to target: {distanceToTarget}";
            return NodeStatus.Running;

        }
    }
}
