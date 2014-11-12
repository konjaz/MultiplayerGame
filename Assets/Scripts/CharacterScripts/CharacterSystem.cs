using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BattleSystem))]
[RequireComponent(typeof(MovementSystem))]
public class CharacterSystem : MonoBehaviour {
    MovementSystem moveSystem;
    BattleSystem battleSystem;
    public Animator animator;

    public float hitpointsLeft = 100;
    public float maxhitpoints = 100;
    bool isAlive = true;
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
    public void RestoreLife(float value) 
    {
        hitpointsLeft += value;
        if (hitpointsLeft > maxhitpoints) 
        {
            hitpointsLeft = maxhitpoints;
        }
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
    }
    public bool IsAlive()
    {
        return isAlive;
    }
    private void Death()
    {
//TODO!!
        //DeathAnimaion();
        isAlive = false;
        //DeactivateAllSubScripts();
        throw new System.NotImplementedException();
    }

    public void RestorePlayer()
    {
//TODO
        //ActivateAllSubScripts();
        //DisableDeathBool();
        isAlive = true;
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

    public void SetSpeed(float value)
    {
        animator.SetFloat(speedFloat, value);
    }
    #endregion
    #region Combat
    public void TargetWeaponAt(Vector3 target) 
    {
        battleSystem.TargetWeaponAtPoint(target);
    }
    public void DealDamage(CharacterSystem enemy) 
    {
        if (enemy.charAliment != charAliment && enemy.charAliment != CharacterAligment.Friendly)
        { 
            battleSystem.DealDamage(enemy);
        }
    }
    public void Shoot()
    {
        if (battleSystem.CanIShoot())
        {
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
}

public enum CharacterAligment 
{ 
    Player,
    Enemy,
    Friendly
}