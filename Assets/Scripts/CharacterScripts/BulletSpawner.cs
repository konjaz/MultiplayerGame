using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    static BulletScript[] bulletsPool;
    public BulletScript[] bulletsPool2;
    const int NUMBER_OF_BULLETS_IN_POOL = 200;
    public static GameObject staticBulletPrefab;
    public GameObject bulletPrefab;
    public Transform bulletParent;

    public static BulletScript GetInactiveBullet()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
        {
            //Debug.Log("bullet " + i);
            if (bulletsPool[i].gameObject.activeSelf == false)
            {
                return bulletsPool[i];
            }
        }
        Debug.LogError("Error: BattleSystem  Not enought bullets prespawned to shoot with");
        return null;
    }
    void bulletPool()
    {
        bulletsPool = new BulletScript[NUMBER_OF_BULLETS_IN_POOL];
        for (int i = 0; i < NUMBER_OF_BULLETS_IN_POOL; i++)
        {
            GameObject bul = Network.Instantiate(bulletPrefab,Vector3.zero,Quaternion.identity, 0) as GameObject;
            bulletsPool[i] = bul.GetComponent<BulletScript>();
            bulletsPool[i].transform.parent = bulletParent;
            bulletsPool[i].name = bulletPrefab.name;
            bulletsPool[i].DeactivateBullet();
        }
        bulletsPool2 = bulletsPool;
    }
	// Use this for initialization
	void Start () {
        staticBulletPrefab = bulletPrefab.gameObject;
        bulletPool();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
