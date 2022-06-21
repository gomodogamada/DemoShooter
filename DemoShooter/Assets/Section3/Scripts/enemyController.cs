using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class enemyController : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private Transform[] m_wayPoints;

        private int m_currentWayPoint;
        private bool m_Active;

        [SerializeField] private projectileController m_projectile;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float minNextFire;
        [SerializeField] private float maxNextFire;
        private float tempNextFire;

        //khai bao hp
        [SerializeField] private int hp;
        private int currentHp;

        private spawnManager m_spawnManager;

        // Start is called before the first frame update
        void Start()
        {
            //ham nay se duyet tat ca obj o tren scene roi lay ra spawnManager
            m_spawnManager = FindObjectOfType<spawnManager>();
        }

        // Update is called once per frame
        void Update()
        {
            //active = true moi dc
            if (!m_Active)
                return;

            //way point tiep theo = point hien tai + 1
            int nextWayPoint = m_currentWayPoint + 1;
            if (nextWayPoint > m_wayPoints.Length - 1) //neu point tiep theo > mảng waypoint
                nextWayPoint = 0;

            //de di chuyen enemy den point, 
            // truyen vao tham so: 1: vi tri hien tai cua enemy, 2: vi tri diem dich(nextpoint), 3: speed
            transform.position = Vector3.MoveTowards(transform.position, m_wayPoints[nextWayPoint].position, m_moveSpeed * Time.deltaTime);
            if (transform.position == m_wayPoints[nextWayPoint].position)
                m_currentWayPoint = nextWayPoint;

            Vector3 direction = m_wayPoints[nextWayPoint].position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            //ban dan
            if (tempNextFire <= 0)
            {
                fire();
                tempNextFire = Random.Range(minNextFire, maxNextFire);
            } //reset ve gia tri mac dinh cho lan fire tiep theo

            tempNextFire -= Time.deltaTime;
        }

        public void Init(Transform[] wayPoints)
        {
            m_wayPoints = wayPoints;
            m_Active = true; //luc nay enemy moi hoat dong
            transform.position = wayPoints[0].position;
            tempNextFire = Random.Range(minNextFire, maxNextFire);
            currentHp = hp;
        }

        private void fire()
        {
            //projectileController projectile = Instantiate(m_projectile, firePoint.position, Quaternion.identity, null);
            projectileController projectile = m_spawnManager.SpawnEnemyProjectile(firePoint.position);
            projectile.Fire();
        }

        public void Hit(int damage)
        {
            currentHp -= damage;
            if (currentHp <= 0)
                //Destroy(gameObject);
                m_spawnManager.ReleaseEnemy(this); //truyen vao chinh no
        }
    }
}
