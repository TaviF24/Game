using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
	public override void OnInspectorGUI()
	{
		Interactable interactable = (Interactable)target;
		if (target.GetType() == typeof(EventOnlyInteractable))
		{
			interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
			EditorGUILayout.HelpBox("EventOnlyInteract cand ONLY use UnityEvents.", MessageType.Info);
			if (interactable.GetComponent<InteractionEvent>() == null)
			{
				interactable.useEvents = true;
				interactable.gameObject.AddComponent<InteractionEvent>();
			}
		}
		else
		{
			base.OnInspectorGUI();
			if (interactable.useEvents)
			{
				// add InteractionEvent component if we are using the componenet
				if (interactable.GetComponent<InteractionEvent>() == null)
				{
					interactable.gameObject.AddComponent<InteractionEvent>();
				}
			}
			else
			{
				// remove InteractionEvent component if we are not using the componenet
				if (interactable.GetComponent<InteractionEvent>() != null)
				{
					DestroyImmediate(interactable.GetComponent<InteractionEvent>());
				}
			}
		}
	}
}
