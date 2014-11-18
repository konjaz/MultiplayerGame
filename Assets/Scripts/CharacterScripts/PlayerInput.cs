using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public CharacterSystem charSystem;

    Camera cam;

    public bool MouseTargetingActive = true;
    void Awake() {
        cam = Camera.main;
        if (!charSystem) 
        {
            if (!(charSystem = GetComponent<CharacterSystem>())) 
            {
                Debug.LogError("Blad <PlayerInput>: Brak CharacterSystem");
            }
        }
	}

    Ray mousePositionRay;
    RaycastHit mousePositionRaycastHit;
	void FixedUpdate () {

        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal"); // unused
        charSystem.Move(verticalMovement,horizontalMovement);

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
        //MousePosition;
        
	}
    internal void active()
    {
        throw new System.NotImplementedException();
    }
}
