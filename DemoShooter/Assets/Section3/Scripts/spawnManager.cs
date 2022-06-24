using Section3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{

    //tạo pooling object để thay thế Instantiate và destroy Object, tránh rác bộ nhớ
    [System.Serializable]
    public class EnemiesPool
    {
        public enemyController prefab;
        public List<enemyController> in_activeObjs;
        public List<enemyController> activeObjs;

        //ham thay the Instantiate
        public enemyController Spawn(Vector3 position, Transform parent)
        {
            if (in_activeObjs.Count == 0)
            {
                enemyController newObj = GameObject.Instantiate(prefab, parent);
                newObj.transform.position = position;
                activeObjs.Add(newObj);
                return newObj;
            }
            else //neu list co enemy thi ta sd luon enemy trong list thay vi tao ra 1 object moi
            {
                enemyController oldObj = in_activeObjs[0];
                oldObj.gameObject.SetActive(true);
                oldObj.transform.SetParent(parent);
                oldObj.transform.position = position;
                activeObjs.Add(oldObj);
                in_activeObjs.RemoveAt(0);
                return oldObj;
            }
        }

        //ham thay the Destroy
        public void Release(enemyController obj)
        {
            //neu obj nay ton tai trong list active thi xoa no roi them no ve in_active
            //roi xoa no o tren scene
            if (activeObjs.Contains(obj))
            {
                activeObjs.Remove(obj);
                in_activeObjs.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class ProjectilePool
    {
        public projectileController prefab;
        public List<projectileController> in_activeObjs;
        public List<projectileController> activeObjs;

        //ham thay the Instantiate
        public projectileController Spawn(Vector3 position, Transform parent)
        {
            if (in_activeObjs.Count == 0)
            {
                projectileController newObj = GameObject.Instantiate(prefab, parent);
                newObj.transform.position = position;
                activeObjs.Add(newObj);
                return newObj;
            }
            else //neu list co enemy thi ta sd luon enemy trong list thay vi tao ra 1 object moi
            {
                projectileController oldObj = in_activeObjs[0];
                oldObj.gameObject.SetActive(true);
                oldObj.transform.SetParent(parent);
                oldObj.transform.position = position;
                activeObjs.Add(oldObj);
                in_activeObjs.RemoveAt(0);
                return oldObj;
            }
        }

        //ham thay the Destroy
        public void Release(projectileController obj)
        {
            //neu obj nay ton tai trong list active thi xoa no roi them no ve in_active
            //roi xoa no o tren scene
            if (activeObjs.Contains(obj))
            {
                activeObjs.Remove(obj);
                in_activeObjs.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
    }

    public class spawnManager : MonoBehaviour
    {
        [SerializeField] private bool m_active; //hoãn cho đến khi 1 đk cụ thể dc kích hoạt
        //[SerializeField] private enemyController enemyPrefab;
        [SerializeField] private EnemiesPool m_enemiesPool;
        [SerializeField] private ProjectilePool m_playerProjectilePool;
        [SerializeField] private ProjectilePool m_enemyProjectilePool;
        [SerializeField] private int minTotalEnemy; //so luong enemy muon spawn ra man hinh
        [SerializeField] private int maxTotalEnemy;

        [SerializeField] private float enemySpawnInterval; //tgian cach nhau
        [SerializeField] private enemyPath[] m_path;

        [SerializeField] private int m_totalGroups;
        // Start is called before the first frame update
        void Start()
        {
            //de goi phai truyen vao startcoroutine
            //StartCoroutine(IETestCoroutine());
            StartCoroutine(IESpawnGroups(m_totalGroups));
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator IESpawnGroups(int pGroups)
        {
            for (int i = 0; i < pGroups; i++)
            {
                int totalEnemies = Random.Range(minTotalEnemy, maxTotalEnemy);
                enemyPath path = m_path[Random.Range(0, m_path.Length)];
                yield return StartCoroutine(IEspawn(totalEnemies, path));
                yield return new WaitForSeconds(3f);

            }
        }

        private IEnumerator IEspawn(int totalEnemies, enemyPath path)
        {
            //int totalEnemy = Random.Range(minTotalEnemy, maxTotalEnemy);

            for (int i = 0; i < totalEnemies; i++)
            {
                yield return new WaitUntil(() => m_active);
                yield return new WaitForSeconds(enemySpawnInterval);
                enemyController enemy = m_enemiesPool.Spawn(path.WayPoint[0].position, transform);
                //enemyPath path = m_path[Random.Range(0, m_path.Length)];

                enemy.Init(path.WayPoint);
            }
        }

        //ham trung gian de enemyController goi dc ham Release tu day sang do
        public void ReleaseEnemy(enemyController obj) //tham so truyen vao la enemyController
        {
            m_enemiesPool.Release(obj);
        }

        //voi enemy projectile
        public projectileController SpawnEnemyProjectile(Vector3 position)
        {
            projectileController obj = m_enemyProjectilePool.Spawn(position, transform);
            obj.SetFromPlayer(false);
            return obj;
        }

        public void ReleaseEnemyProjectile(projectileController projectile) //tham so truyen vao la enemyController
        {
            m_enemyProjectilePool.Release(projectile);
        }

        //voi player projectile
        public projectileController SpawnPlayerProjectile(Vector3 position)
        {
            projectileController obj = m_playerProjectilePool.Spawn(position, transform);
            obj.SetFromPlayer(true                                 );
            return obj;
        }

        public void ReleasePlayerProjectile(projectileController projectile) //tham so truyen vao la enemyController
        {
            m_playerProjectilePool.Release(projectile);
        }
    }
}
