using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class projectileController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 direction; //huong di chuyen

        [SerializeField] private int m_Damage;

        private spawnManager m_spawnManager;
        private float m_lifeTime;
        private bool m_fromPlayer;

        // Start is called before the first frame update
        void Start()
        {
            m_spawnManager = FindObjectOfType<spawnManager>();
        }

        // Update is called once per frame
        void Update()
        {
            // truyen vao
            transform.Translate(direction * Time.deltaTime * moveSpeed);

            if (m_lifeTime <= 0)
                Release();
            m_lifeTime -= Time.deltaTime;
            //gia tra cua no se bi tru di 1 khoang Time.deltaTime o moi 1 frame, tính từ khi viên đạn đó xuất hiện
            //cho đến khi giá trị của nó về 0 thì ta gọi hàm Release kia
        }

        public void Fire()
        {
            //Destroy(gameObject, 6f);
            m_lifeTime = 8f;
        }

        //ham goi dan, neu dan tu player thi ReleasePlayerProjectile, nguoc lai
        private void Release()
        {
            if (m_fromPlayer)
                m_spawnManager.ReleasePlayerProjectile(this);
            else
                m_spawnManager.ReleaseEnemyProjectile(this);
        }

        public void SetFromPlayer(bool value)
        {
            m_fromPlayer = value;
        }

        private void OnTriggerEnter2D(Collider2D collision) //va cham nhung xuyen qua object
        {
            Debug.Log(collision.gameObject.name); //hien ten cua object vua va cham

            if (collision.gameObject.CompareTag("Enemy")) //xu ly va cham = tag
            {
                Release();
                enemyController enemy;
                //hàm TryGetComponent sẽ trả về enemyController nếu nó tìm thấy từ gameObject nó va chạm có script này
                collision.gameObject.TryGetComponent(out enemy);
                if (enemy != null)
                    enemy.Hit(m_Damage);
            }

            if (collision.gameObject.CompareTag("Player")) //xu ly va cham = tag
            {
                Release();
                playerController player;
                collision.gameObject.TryGetComponent(out player);
                if (player != null)
                    player.Hit(m_Damage);
            }
        }
    }
}
