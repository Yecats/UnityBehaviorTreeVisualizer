using System;
using UnityEngine;
using UnityEngine.AI;

namespace WUG.BehaviorTreeVisualizer.CommunityNodes
{
    [Serializable]
    public class NavigateToDestination : Node
    {
        private Func<Vector3> m_DestinationCalculation;
        private Vector3 m_TargetDestination = Vector3.zero;
        private NavMeshAgent m_NavMeshAgent;
        private float m_MaxDistanceToSample = 1f;

        /// <summary>
        /// Navigate a game object to a specific location using Unity's Navigation. 
        /// Behavior will get a SamplePosition within 1 meter of the destination to ensure that the location
        /// provided can be navigated.
        /// </summary>
        /// <param name="navMeshAgent">NavMesh on the game object that will be moved</param>
        /// <param name="destination">Function that can be used to calculate the target destination. Must return a Vector3. </param>
        /// <param name="maxDistance">Max distance that NavMesh.SamplePosition should check for</param>
        public NavigateToDestination(NavMeshAgent navMeshAgent, Func<Vector3> destination, float maxDistance = 1f)
        {
            Name = "Navigate";
            m_NavMeshAgent = navMeshAgent;
            m_DestinationCalculation = destination;
            m_MaxDistanceToSample = maxDistance;
        }

        /// <summary>
        /// Navigate a game object to a specific location using Unity's Navigation. 
        /// Behavior will get a SamplePosition within 1f of the destination to ensure that the location
        /// provided can be navigated.
        /// </summary>
        /// <param name="gameObject">Game object that will be moved. Must have a NavMeshAgent on it</param>
        /// <param name="destination">Function that can be used to calculate the target destination. Must return a Vector3. </param>
        /// <param name="maxDistance">Max distance that NavMesh.SamplePosition should check for</param>
        public NavigateToDestination(GameObject gameObject, Func<Vector3> destination, float maxDistance = 1f)
        {
            Name = "Navigate";
            m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            m_DestinationCalculation = destination;
            m_MaxDistanceToSample = maxDistance;
        }


        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {
            //Make sure that a reference to NavMeshAgent and a valid destination exists
            if (m_NavMeshAgent == null)
            {
                StatusReason = "NavMeshAgent is null";
                return NodeStatus.Failure;
            }

            //First Evaluation will get the official target destination and set it on the NavMeshAgent
            if (IsFirstEvaluation)
            {
                if (m_DestinationCalculation() == null)
                {
                    StatusReason = "Unable to calculate a destination position. Did you pass a function in?";
                    return NodeStatus.Failure;
                }

                m_TargetDestination = m_DestinationCalculation();

                if (m_TargetDestination  == Vector3.zero)
                {
                    StatusReason = "Unable to calculate a destination position. Does the function return a position?";
                    return NodeStatus.Failure;
                }

                NavMesh.SamplePosition(m_TargetDestination, out NavMeshHit hit, m_MaxDistanceToSample, 1);
                m_TargetDestination = hit.position;

                m_NavMeshAgent.SetDestination(m_TargetDestination);
                StatusReason = $"Starting to navigate to {m_TargetDestination}";

                return NodeStatus.Running;
            }

            //Evaluate the remaining distance
            float distanceToTarget = Vector3.Distance(m_TargetDestination, m_NavMeshAgent.transform.position);

            //Reached the destination
            if (distanceToTarget < .25f)
            {
                StatusReason = $"Navigation ended. " +
                    $"\n - Evaluation Count: {EvaluationCount}. " +
                    $"\n - Target Destination: {m_TargetDestination}" +
                    $"\n - Distance to target: {Math.Round(distanceToTarget, 1)}";

                return NodeStatus.Success;
            }

            //Still moving
            StatusReason = $"Distance to target: {distanceToTarget}";
            return NodeStatus.Running;

        }
    }
}

