using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BattleSystem))]
[RequireComponent(typeof(MovementSystem))]
public class CharacterSystem : Photon.MonoBehaviour
{
    MovementSystem moveSystem;
    BattleSystem battleSystem;
    public Animator animator;

    public float hitpointsLeft = 100;
    public float maxhitpoints = 100;
    public Transform Graphic_HealthBar;
    //bool isAlive = true;
    public CharacterAligment charAliment = CharacterAligment.Enemy;
    #region Getters Setters
    public float GetHitpointsLeft() 
    {
        return hitpointsLeft;
    }
    public float CurrentToMaxHealthRatio() 
    {
        return hitpointsLeft / maxhitpoints;
    }
    public bool IsAlive() 
    {
        if (hitpointsLeft > 0) 
        {
            return true;
        }
        return false;
    }
    void SetHitPoints(float value) 
    {
        if (value < maxhitpoints)
        {
            hitpointsLeft = value;
        }
        else 
        {
            hitpointsLeft = maxhitpoints;
        }
        
    }
    public void RestoreLife(float value) 
    {
        hitpointsLeft += value;
        if (hitpointsLeft > maxhitpoints) 
        {
            hitpointsLeft = maxhitpoints;
        }

        Graphic_HealthBar.localScale = new Vector3(CurrentToMaxHealthRatio(),Graphic_HealthBar.localScale.y,Graphic_HealthBar.localScale.y);
    }
    public void DecreaseLife(float value) 
    {
//TODO!!
        //GotHurtSound();
        //GotHurtAnimation();
        hitpointsLeft -= value;
        if (hitpointsLeft <= 0)
        {
            hitpointsLeft = 0;
            Death();
        }
        Graphic_HealthBar.localScale = new Vector3(CurrentToMaxHealthRatio(), Graphic_HealthBar.localScale.y, Graphic_HealthBar.localScale.y);
    }
    //public bool IsAlive()
    //{
    //    return isAlive;
    //}
    private void Death()
    {
//TODO!!
        //DeathAnimaion();
        SetAlive(false);
        collider.enabled = false;
        //isAlive = false;
        //DeactivateAllSubScripts();
        //throw new System.NotImplementedException();
    }

    public void RestorePlayer()
    {
//TODO
        //ActivateAllSubScripts();
        //DisableDeathBool();
        //isAlive = true;
        collider.enabled = true;
        SetAlive(true);
        hitpointsLeft = maxhitpoints;
    }
    #endregion
    void Awake() {
        if (!(moveSystem = GetComponent<MovementSystem>()))
        {
            Debug.LogError("Blad <CharacterSystem>: Brak MovementSystem");
        }
        if (!(battleSystem = GetComponent<BattleSystem>()))
        {
            Debug.LogError("Blad <CharacterSystem>: Brak BattleSystem");
        }
        //animator = GetComponentInChildren<Animator>();
	}
    

    #region AnimationSystem
    public static int speedFloat = Animator.StringToHash("speed");
    public static int aliveBool = Animator.StringToHash("alive");
    public void SetSpeed(float value)
    {
        animator.SetFloat(speedFloat, value);
    }
    public void SetAlive(bool value) 
    {
        animator.SetBool(aliveBool,value);
    }
    #endregion
    #region Combat
    [RPC]
    public void OnAwakeRPC(byte myParameter)
    {
        Debug.Log("RPC: 'OnAwakeRPC' Parameter: " + myParameter + " PhotonView: " + this.photonView);
        BulletScript bullet = battleSystem.GetBullet();
        bullet.BulletSetUP(this, battleSystem.GetBulletSpawningPoint(), battleSystem.GetBulletsDirection(), battleSystem.GetForce());
    }
    public void TargetWeaponAt(Vector3 target) 
    {
        battleSystem.TargetWeaponAtPoint(target);
    }
    public void DealDamage(CharacterSystem enemy) 
    {
        if (enemy.charAliment != CharacterAligment.Friendly)
        { 
            battleSystem.DealDamage(enemy);
        }
    }
    public void Shoot()
    {
        if (battleSystem.CanIShoot())
        {
            photonView.RPC("OnAwakeRPC", PhotonTargets.Others, (byte)1);
            BulletScript bullet = battleSystem.GetBullet();
            bullet.BulletSetUP(this, battleSystem.GetBulletSpawningPoint(), battleSystem.GetBulletsDirection(), battleSystem.GetForce());
        }
    }
    #endregion
    #region Locomotion functions
    public void Move(float vertical, float horizontal) 
    {
        //Debug.Log(horizontal);
        SetSpeed(horizontal);
        moveSystem.Move(vertical,horizontal);
    }
    public void Jump() 
    {
        moveSystem.Jump();
    }
    #endregion

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(GetHitpointsLeft());
        }
        else
        {
            SetHitPoints((float)stream.ReceiveNext());
            
        }
    }
}

public enum CharacterAligment 
{ 
    Player,
    Enemy,
    Friendly
}