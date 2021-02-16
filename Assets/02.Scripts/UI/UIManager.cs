using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	#region Variables
	public PlayerDataObject _playerDataObject;

	public Text _scoreText;
	public Text _hpText;
	public Text _lifeText;
	#endregion

	protected override void Awake()
	{
		base.Awake();

		_playerDataObject.OnChangedScore += UpdateScore;
		_playerDataObject.OnChangedHp += UpdateHp;
		_playerDataObject.OnChangedLife += UpdateLife;

		InitUI();
	}

	private void InitUI()
	{
		UpdateScore(_playerDataObject.TotalScore);
		UpdateHp(_playerDataObject.Hp);
		UpdateLife(_playerDataObject.Life);
	}

	private void UpdateScore(int score)
	{
		_scoreText.text = string.Format("Score: " + score.ToString("D6"));
	}

	private void UpdateHp(int hp)
	{
		if (hp < 0)
			hp = 0;
		_hpText.text = "Hp: " + hp.ToString();
	}

	private void UpdateLife(int life)
	{
		_lifeText.text = "Life: " + life.ToString();
	}
}
