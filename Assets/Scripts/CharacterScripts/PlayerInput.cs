using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
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
        if (networkView.isMine)
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
        //MousePosition;
        
	}
    internal void active()
    {
        throw new System.NotImplementedException();
    }
}
