using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class enemyPath : MonoBehaviour
    {
        [SerializeField] private Transform[] m_wayPoint;
        [SerializeField] private Color m_color;
        [SerializeField] private bool Show;

        public Transform[] WayPoint => m_wayPoint; //khai bao de tra ve mang cac waypoint

        private void OnDrawGizmos()
        {
            if (!Show)
                return;
            if (m_wayPoint != null && m_wayPoint.Length > 1)
            {
                Gizmos.color = m_color; // doi mau cho duong thang
                //vong lap for de ve cac duong thang noi cac diem,  nối điểm i đến i +1
                for (int i = 0; i < m_wayPoint.Length - 1; i++)
                {
                    Transform from = m_wayPoint[i];
                    Transform to = m_wayPoint[i + 1];
                    
                    //de ve tu diem from den to
                    Gizmos.DrawLine(from.position, to.position);
                }

                //noi not 2 diem dau voi cuoi
                Gizmos.DrawLine(m_wayPoint[0].position, m_wayPoint[m_wayPoint.Length - 1].position);
            }
        }
    }
}
