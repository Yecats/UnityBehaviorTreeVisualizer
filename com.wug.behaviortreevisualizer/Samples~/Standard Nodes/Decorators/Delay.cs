using Disguise.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WUG.BehaviorTreeVisualizer;


namespace Disguise.Behaviors.Decorators
{
    public class Delay : Decorator
    {
        private float m_StartTime;
        private bool m_UseFixedTime;
        private float m_TimeToWait;

        /// <summary>
        /// Executes a timer and then runs the child node once
        /// </summary>
        /// <param name="node">Node to run</param>
        /// <param name="timeToWait">Amount of time to wait in seconds</param>
        /// <param name="useFixedTime">Whether to use Time.fixedTime (true) or Time.time (false)</param>
        public Delay(float timeToWait, Node node, bool useFixedTime = false) : base($"Runs after {timeToWait}", node)
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

            float elapsedTime = Time.fixedTime - m_StartTime;

            if (IsFirstEvaluation)
            {
                StatusReason = $"Starting timer for {m_TimeToWait}";
                m_StartTime = m_UseFixedTime ? Time.fixedTime : Time.time;
            }
            else if (elapsedTime > m_TimeToWait)
            {
                NodeStatus originalStatus = (ChildNodes[0] as Node).Run();
                StatusReason = $"Timer complete - Child node status is: { originalStatus}";

                return originalStatus;
            }

            StatusReason = $"Timer is {elapsedTime} out of {m_TimeToWait}.";
            return NodeStatus.Running;
        }
    }
}
