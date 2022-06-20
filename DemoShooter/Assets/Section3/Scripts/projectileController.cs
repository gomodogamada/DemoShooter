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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // truyen vao
            transform.Translate(direction * Time.deltaTime * moveSpeed);
        }

        public void Fire()
        {
            Destroy(gameObject, 6f);
        }

        private void OnTriggerEnter2D(Collider2D collision) //va cham nhung xuyen qua object
        {
            Debug.Log(collision.gameObject.name); //hien ten cua object vua va cham

            if (collision.gameObject.CompareTag("Enemy")) //xu ly va cham = tag
            {
                Destroy(gameObject);

                enemyController enemy;
                //hàm TryGetComponent sẽ trả về enemyController nếu nó tìm thấy từ gameObject nó va chạm có script này
                collision.gameObject.TryGetComponent(out enemy);
                enemy.Hit(m_Damage);
            }

            if (collision.gameObject.CompareTag("Player")) //xu ly va cham = tag
            {
                Destroy(gameObject);

                playerController player;
                //hàm TryGetComponent sẽ trả về enemyController nếu nó tìm thấy từ gameObject nó va chạm có script này
                collision.gameObject.TryGetComponent(out player);
                player.Hit(m_Damage);
            }
        }
    }
}
