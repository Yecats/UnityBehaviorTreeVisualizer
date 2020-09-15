using UnityEngine;

namespace WUG.BehaviorTreeDebugger
{

    public class Timer : Decorator
    {
        private float m_StartTime;
        private bool m_UseFixedTime;
        private float m_TimeToWait;

        /// <summary>
        /// Run the child node a specified amount of time in seconds before exiting
        /// </summary>
        /// <param name="name">Friendly name - displayed on the Behavior Tool Debugger UI if set</param>
        /// <param name="node">Node to run</param>
        /// <param name="timeToWait">Amount of time to wait in seconds</param>
        /// <param name="useFixedTime">Whether to use Time.fixedTime (true) or Time.time (false)</param>
        public Timer(float timeToWait, Node node,  bool useFixedTime = false) : base($"Timer for {timeToWait}", node) 
        {
            m_UseFixedTime = useFixedTime;
            m_TimeToWait = timeToWait;
        }

        protected override void OnReset() { }
        protected override NodeStatus OnRun()
        {
            if (ChildNodes.Count == 0 || ChildNodes[0] == null)
            {
                return NodeStatus.Failure;
            }

            NodeStatus originalStatus = (ChildNodes[0] as Node).Run();
            float elapsedTime = Time.fixedTime - m_StartTime;

            if (IsFirstEvaluation)
            {
                StatusReason = $"Starting timer for {m_TimeToWait}. Child node status is: {originalStatus}";
                m_StartTime = m_UseFixedTime ? Time.fixedTime : Time.time;
            }
            else if (elapsedTime > m_TimeToWait)
            {
                StatusReason = $"Timer complete - Child node status is: { originalStatus}";
                return NodeStatus.Success;
            }

            StatusReason = $"Timer is {elapsedTime} out of {m_TimeToWait}. Child node status is: {originalStatus}";
            return NodeStatus.Running;

        }
    }
}
