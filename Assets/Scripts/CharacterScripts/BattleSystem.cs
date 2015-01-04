using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour {
    public Transform weaponPivot;
    public Transform bulletsSpawningPoint;
    //public Transform bulletHolster;
    public float baseDamage = 5;
    public float damageModifier = 1;
    public float bulletStartingForce = 20;
    public float spaceInBetweenShoots = 1f;
    float shootTimer;

    public BulletScript GetBullet()
    {
        shootTimer = Time.time + spaceInBetweenShoots;
        return (Network.Instantiate(BulletSpawner.staticBulletPrefab,Vector3.zero,Quaternion.identity, 0) as GameObject).GetComponent<BulletScript>();
    }
    public bool CanIShoot() 
    {
        if (Time.time > shootTimer)
        {
            return true;
        }
        return false;
    }
    internal float GetForce()
    {
        return bulletStartingForce;
    }
    void Awake () {
	}
	
	void Update () {
    
    }

    public Vector3 GetBulletSpawningPoint() 
    {
        return bulletsSpawningPoint.position;
    }
    public Vector3 GetBulletsDirection() 
    {
        Vector3 tempVec = (GetBulletSpawningPoint() - weaponPivot.position);
        tempVec.z = 0;
        return tempVec.normalized;
    }
    public void TargetWeaponAtPoint(Vector3 target) 
    {
        Vector3 vectorToTarget = target - transform.position;
        vectorToTarget.z = 0;
        float angle = Vector3.Angle(Vector3.up, vectorToTarget);
        if (vectorToTarget.x != 0)
        {
            weaponPivot.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Sign(-vectorToTarget.x));
            weaponPivot.transform.localScale = new Vector3(Mathf.Abs(weaponPivot.transform.localScale.x)*  Mathf.Sign(vectorToTarget.x),weaponPivot.transform.localScale.y,weaponPivot.transform.localScale.z);
        }
        //}
        //else 
        //{
        //    weaponPivot.transform.rotation = Quaternion.Euler(0, 0, -angle);
        //}
    }

    public void DealDamage(CharacterSystem enemy) 
    {
        enemy.DecreaseLife(GetDamage());
    }
    public float GetDamage() 
    {
        return baseDamage * damageModifier;
    }

}
