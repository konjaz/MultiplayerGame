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
        //else 
        //{
        //    SyncedMovement();
        //}
        //MousePosition;
	}

    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //        stream.SendNext(rigidbody.position);
    //    else
    //        rigidbody.position = (Vector3)stream.ReceiveNext();
    //}
    #region Networking
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
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
            rigidbody.position = (Vector3)stream.ReceiveNext();
            //syncEndPosition = (Vector3)stream.ReceiveNext();
            //syncStartPosition = rigidbody.position;

            //syncTime = 0f;
            //syncDelay = Time.time - lastSynchronizationTime;
            //lastSynchronizationTime = Time.time;
        }
    }

    //private void SyncedMovement()
    //{
    //    syncTime += Time.deltaTime;
    //    rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    //}
    #endregion
    //internal void active()
    //{
    //    throw new System.NotImplementedException();
    //}
}
