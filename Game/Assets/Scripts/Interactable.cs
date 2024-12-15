using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	// add or remove an InteractionEvent component to this gameobject
	public bool useEvents;

	// message to display when player looks at interactable
    public string promptMessage;

	// template function to be overridden by child classes
	protected virtual void Interact() {	}

	// function to be called from player
	public void BaseInteract()
	{
		if (useEvents)
		{
			GetComponent<InteractionEvent>().OnInteract.Invoke();
		}
		Interact();
	}
}
