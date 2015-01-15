using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour {
    CharacterSystem source;
    Vector3 direction;
    public float timeToSelfDestruct = 5;
    float selfDestructTimer;
    public void BulletSetUP(CharacterSystem source,Vector3 position, Vector3 direction, float force)
    {
        this.source = source;
        this.direction = direction;
        transform.position = position;
        gameObject.SetActive(true);
        rigidbody.velocity = direction * force;
        transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, direction) * Mathf.Sign(-direction.x));
        selfDestructTimer = Time.time + timeToSelfDestruct;

        //PlayAudioSource;
        //Shoot!
    }
	
    void Awake() {
        selfDestructTimer = Time.time;
	}
	
    void FixedUpdate () {
        if (selfDestructTimer < Time.time) 
        {
            DeactivateBullet();
        }
        else 
        { 
            Vector3 velocity = rigidbody.velocity;
            velocity.z = 0;

            transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, velocity) * Mathf.Sign(-velocity.x));
        }
	}

    public void OnTriggerEnter(Collider col) 
    { 
        CharacterSystem enemyHitted;
        if(col.gameObject != source.gameObject)
        {
            if (!col.isTrigger) // by się upewnić że nie uderzyliśmy jakiegos triggera 
            {
                if (enemyHitted = col.GetComponent<CharacterSystem>()) //Aby upewnić się że uderzyliśmy postać z skryptem CharacterSystem
                {
                    source.DealDamage(enemyHitted);
                    source.rigidbody.AddForce(direction * rigidbody.mass);
                    DeactivateBullet();
                    Debug.Log("Hitted :" + enemyHitted.name );
                    //enemyHitted.DecreaseLife();
                }
                else 
                {
                    DeactivateBullet();
                }
            }
        }
    }

    public void DeactivateBullet() 
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
    //#region Syncing PlayerCharacters
    //private float lastSynchronizationTime = 0f;
    //private float syncDelay = 1f;
    //private float syncTime = 0f;
    //private Vector3 syncStartPosition = Vector3.zero;
    //private Vector3 syncEndPosition = Vector3.zero;
    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        stream.SendNext(rigidbody.position);
    //    }
    //    else
    //    {
    //        //rigidbody.position = (Vector3)stream.ReceiveNext();
    //        syncEndPosition = (Vector3)stream.ReceiveNext();
    //        syncStartPosition = rigidbody.position;

    //        syncTime = 0f;
    //        syncDelay = Time.time - lastSynchronizationTime;
    //        lastSynchronizationTime = Time.time;
    //    }
    //}

    //private void SyncedMovement()
    //{
    //    syncTime += Time.deltaTime;
    //    Vector3 vec = syncEndPosition - rigidbody.position;
    //    //charSystem.SetSpeed(vec.x);
    //    if (vec.sqrMagnitude < 4)
    //    {
    //        rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    //    }
    //    else
    //    {
    //        rigidbody.position = syncEndPosition;
    //    }

    //}
    //#endregion
}
