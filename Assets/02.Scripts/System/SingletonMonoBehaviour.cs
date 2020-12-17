using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
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
		_instance = this as T;
		if(Application.isPlaying)
		{
			//부모오브젝트가 있으면 부모 오브젝트를 DontDestroy
			if(transform.parent != null && transform.root != null)
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
