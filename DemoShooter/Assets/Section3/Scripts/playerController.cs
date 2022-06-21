using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class playerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed; //kieu so
        [SerializeField] private projectileController m_projectile;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float nextFire;

        //khai bao hp
        [SerializeField] private int hp;
        private int currentHp;

        private float tempNextFire;
        private spawnManager m_spawnManager;
        // Start is called before the first frame update
        void Start()
        {
            currentHp = hp; //vi k tao ham Init nen khai bao o day
            m_spawnManager = FindObjectOfType<spawnManager>();
        }

        // Update is called once per frame
        void Update()
        {
            //bam phim de di chuyen trai phai - len xuong
            float horizontal = Input.GetAxisRaw("Horizontal"); //trai phai
            float vertical = Input.GetAxisRaw("Vertical"); //len xuong
            Vector2 direction = new Vector2(horizontal, vertical);
            //hàm thêm lực để di chuyển dc, độ lớn lực di chuyển = direction x tgian cho 1 lần update (1frame) * moveSpeed
            transform.Translate(direction * Time.deltaTime * moveSpeed); 

            //bam phim space de ban dc
            if(Input.GetKey(KeyCode.Space))
            {
                if (tempNextFire <= 0)
                {
                    fire();
                    tempNextFire = nextFire;
                } //reset ve gia tri mac dinh cho lan fire tiep theo
            }

            tempNextFire -= Time.deltaTime;
        }

        private void fire()
        {
            //projectileController projectile = Instantiate(m_projectile, firePoint.position, Quaternion.identity, null);
           projectileController projectile = m_spawnManager.SpawnPlayerProjectile(firePoint.position);
           projectile.Fire();
        }

        public void Hit(int damage)
        {
            currentHp -= damage;
            if (currentHp <= 0)
                Destroy(gameObject);
        }
    }
}
