using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace WUG.BehaviorTreeVisualizer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public NonPlayerCharacter NPC { get; private set; }
        private List<GameObject> m_Waypoints = new List<GameObject>();
        private List<GameObject> m_Items = new List<GameObject>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();
            m_Items = GameObject.FindGameObjectsWithTag("Item").ToList();

            m_Waypoints = m_Waypoints.Shuffle();

            NPC = FindObjectOfType<NonPlayerCharacter>();
        }

        public GameObject GetClosestItem()
        {
            return m_Items.OrderBy(x => Vector3.Distance(x.transform.position, NPC.transform.position)).FirstOrDefault();
        }

        public void PickupItem(GameObject item)
        {
            m_Items.Remove(item);

            Destroy(item);
        }

        public GameObject GetNextWayPoint()
        {
            if (m_Waypoints != null && m_Waypoints.Count > 0)
            {
                GameObject nextWayPoint = m_Waypoints[0];
                m_Waypoints.RemoveAt(0);

                return nextWayPoint;
            }

            return null;
        }


    }
}
