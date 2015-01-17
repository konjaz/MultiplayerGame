using UnityEngine;
using System.Collections;

public class PlayerInput : Photon.MonoBehaviour
{
    public CharacterSystem charSystem;

    Camera cam;

    public bool MouseTargetingActive = true;
    void Awake() {
        if (!charSystem) 
        {
            if (!(charSystem = GetComponent<CharacterSystem>())) 
            {
                Debug.LogError("Blad <PlayerInput>: Brak CharacterSystem");
            }
        }
	}
    void Start() 
    {
        cam = Camera.main;
    }
    Ray mousePositionRay;
    RaycastHit mousePositionRaycastHit;
	void Update () {
        if (photonView.isMine)
        {
            if (charSystem.IsAlive())
            {
                PlayerInputs();
            }
        }
        else
        {
            SyncedMovement();
        }
        //MousePosition;
	}

    private void PlayerInputs()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal"); // unused
        charSystem.Move(verticalMovement, horizontalMovement);

        if (Input.GetButton("Jump"))
        {
            charSystem.Jump();
        }
        if (Input.GetButton("Fire"))
        {
            charSystem.Shoot();
            //charSystem.Jump();
        }

        if (MouseTargetingActive)
        {
            mousePositionRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mousePositionRay, out mousePositionRaycastHit, 100f))
            {
                charSystem.TargetWeaponAt(mousePositionRaycastHit.point);
            }
        }
    }
    #region Syncing PlayerCharacters
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 1f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rigidbody.position);
        }
        else
        {
            //rigidbody.position = (Vector3)stream.ReceiveNext();
            syncEndPosition = (Vector3)stream.ReceiveNext();
            syncStartPosition = rigidbody.position;

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        Vector3 vec = syncEndPosition - rigidbody.position;
        charSystem.SetSpeed(vec.x);
        if (vec.sqrMagnitude < 4)
        {
            rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        }
        else 
        {
            syncStartPosition = syncEndPosition;
            rigidbody.position = syncEndPosition;
        }
        
    }
    #endregion
}
