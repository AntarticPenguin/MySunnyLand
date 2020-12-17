using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : SingletonMonoBehaviour<ScoreUI>
{
	#region Variables
	private TextMeshProUGUI _scoreText;
	#endregion

	#region Unity Methods
	// Start is called before the first frame update
	void Start()
    {
		_scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

	#endregion

	#region Methods
	public void UpdateScore(int score)
	{
		_scoreText.SetText("Score: " + score.ToString("D6"));
	}
	#endregion
}
