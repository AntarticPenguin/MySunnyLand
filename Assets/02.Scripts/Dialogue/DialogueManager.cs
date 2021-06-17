using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
{
	public Text _nameText;
	public Text _dialogueText;

	private Queue<string> _sentences;
	private Animator _animator;
	private int _isOpenAnimHash;

	#region Unity Methods
	private void Start()
	{
		_sentences = new Queue<string>();
		_animator = GetComponent<Animator>();
		_isOpenAnimHash = Animator.StringToHash("IsOpen");
	}
	#endregion

	public void StartDialogue(Dialogue dialogue)
	{
		GameManager.Instance.Player.IsTalking = true;

		_animator.SetBool(_isOpenAnimHash, true);

		_sentences.Clear();

		_nameText.text = dialogue._name;

		foreach(string sentence in dialogue._senetences)
		{
			_sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(_sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = _sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentenceCoroutine(sentence));
	}

	private IEnumerator TypeSentenceCoroutine(string sentence)
	{
		_dialogueText.text = "";
		foreach(char letter in sentence.ToCharArray())
		{
			_dialogueText.text += letter;
			yield return null;
		}
	}

	private void EndDialogue()
	{
		GameManager.Instance.Player.IsTalking = false;

		_animator.SetBool(_isOpenAnimHash, false);
	}
}
