using UnityEngine;

namespace WUG.BehaviorTreeVisualizer
{
    public class AreItemsNearBy : Condition
    {
        private float m_DistanceToCheck;

        /// <summary>
        /// Checks to see if an item is a specific distance from the player
        /// </summary>
        /// <param name="player">GameObject that represents the starting position</param>
        /// <param name="maxDistance">Max distance from the player game object to scan for</param>
        public AreItemsNearBy(float maxDistance) : base($"Are Items within {maxDistance}f?") 
        { 
            m_DistanceToCheck = maxDistance; 
        }

        protected override void OnReset() { }

        protected override NodeStatus OnRun()
        {

            GameObject item = GameManager.Instance.GetClosestItem();

            if (item == null)
            {
                StatusReason = "No items near by";
                return NodeStatus.Failure;

            }
            else if (Vector3.Distance(item.transform.position, GameManager.Instance.NPC.transform.position) > m_DistanceToCheck)
            {
                StatusReason = $"No items within range of {m_DistanceToCheck} meters";
                return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }
    }
}