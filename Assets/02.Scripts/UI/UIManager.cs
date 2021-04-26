using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	#region Variables
	public PlayerDataObject _playerDataObject;

	public Text _scoreText;
	public Text _lifeText;
	public Text _gameOverText;
	public GameObject _hpObject;

	public Sprite _filledHeart;
	public Sprite _emptyHeart;

	private Image[] _hpSprites;
	#endregion

	private void Start()
	{
		InitUI();
	}

	private void OnEnable()
	{
		_playerDataObject.OnChangedScore += UpdateScore;
		_playerDataObject.OnChangedHp += UpdateHp;
		_playerDataObject.OnChangedLife += UpdateLife;
	}

	private void OnDisable()
	{
		_playerDataObject.OnChangedScore -= UpdateScore;
		_playerDataObject.OnChangedHp -= UpdateHp;
		_playerDataObject.OnChangedLife -= UpdateLife;
	}

	private void InitUI()
	{
		_hpSprites = _hpObject.GetComponentsInChildren<Image>();
		for(int i = _playerDataObject.CurrentMaxHp; i < _playerDataObject.MaxHpLimit; i++)
		{
			_hpSprites[i].gameObject.SetActive(false);
		}

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
		for (int i = 0; i < _playerDataObject.CurrentMaxHp; i++)
		{
			if(i < hp)
			{
				_hpSprites[i].sprite = _filledHeart;
			}
			else
			{
				_hpSprites[i].sprite = _emptyHeart;
			}
		}
	}

	private void UpdateLife(int life)
	{
		_lifeText.text = "x " + life.ToString();
	}

	public void ActiveGameOverUI(bool active)
	{
		_gameOverText.gameObject.SetActive(active);
	}


}
