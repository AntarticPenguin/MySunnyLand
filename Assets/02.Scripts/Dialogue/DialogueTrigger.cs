using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
	public Dialogue _dialogue;

	#region Unity Methods
	#endregion

	#region Methods
	public void TriggerDialogue()
	{
		DialogueManager.Instance.StartDialogue(_dialogue);
	}

	public void Interact()
	{
		TriggerDialogue();
	}

	#endregion
}
