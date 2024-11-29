using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float interactDistance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

	// Start is called before the first frame update
	void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
		playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
	}

    // Update is called once per frame
    void Update()
    {
		// if not looking at anything interactable, clear the prompt
		playerUI.UpdateText(string.Empty);

        // raycast from the center of camera, forward
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * interactDistance);
        RaycastHit hitInfo; // store information about the object hit by the ray
        if (Physics.Raycast(ray, out hitInfo, interactDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
			{
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
				playerUI.UpdateText(interactable.promptMessage);
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
			}
		}
    }
}