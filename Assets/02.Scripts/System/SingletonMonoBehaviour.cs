using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
	private static bool _isInitialized = false;

    public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType(typeof(T)) as T;
				if(_instance == null)
				{
					GameObject go = new GameObject(typeof(T).Name, typeof(T));
					_instance = go.GetComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (!_isInitialized)
		{
			_instance = this as T;
			_isInitialized = true;
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			_isInitialized = false;
		}
	}

	public bool CheckDuplicationInScene()
	{
		if(_isInitialized)
		{
			//씬에 중복배치된 싱글턴 오브젝트가 있을 경우 삭제
			Destroy(gameObject);
			return true;
		}

		return false;
	}

	public void DontDestroy()
	{
		if (Application.isPlaying)
		{
			//부모오브젝트가 있으면 부모 오브젝트를 DontDestroy
			if (transform.parent != null && transform.root != null)
			{
				DontDestroyOnLoad(transform.root.gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
			}
		}
	}
}
