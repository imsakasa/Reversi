using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	[SerializeField, Range(0, 1), Tooltip("マスタ音量")] float m_Volume = 1f;
	[SerializeField, Range(0, 1), Tooltip("BGMの音量")] float m_BgmVolume = 1f;
	[SerializeField, Range(0, 1), Tooltip("SEの音量")] float m_SeVolume = 1f;

	AudioClip[] m_Bgm;
	AudioClip[] m_Se;

	Dictionary<string, int> m_BgmIndex = new Dictionary<string, int>();
	Dictionary<string, int> m_SeIndex = new Dictionary<string, int>();

	AudioSource m_BgmAudioSource;
	AudioSource m_SeAudioSource;

	public float Volume
	{
		set
		{
			m_Volume = Mathf.Clamp01(value);
			m_BgmAudioSource.volume = m_BgmVolume * m_Volume;
			m_SeAudioSource.volume = m_SeVolume * m_Volume;
		}

		get
		{
			return m_Volume;
		}
	}

	public float BgmVolume
	{
		set
		{
			m_BgmVolume = Mathf.Clamp01(value);
			m_BgmAudioSource.volume = m_BgmVolume * m_Volume;
		}

		get
		{
			return m_BgmVolume;
		}
	}

	public float SeVolume
	{
		set
		{
			m_SeVolume = Mathf.Clamp01(value);
			m_SeAudioSource.volume = m_SeVolume * m_Volume;
		}

		get
		{
			return m_SeVolume;
		}
	}

	void Start()
	{
		if (this != I)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		m_BgmAudioSource = gameObject.AddComponent<AudioSource>();
		m_SeAudioSource = gameObject.AddComponent<AudioSource>();

		m_Bgm = Resources.LoadAll<AudioClip>("Audio/BGM");
		m_Se = Resources.LoadAll<AudioClip>("Audio/SE");

		for (int i = 0; i < m_Bgm.Length; i++)
		{
			m_BgmIndex.Add(m_Bgm[i].name, i);
		}

		for (int i = 0; i < m_Se.Length; i++)
		{
			m_SeIndex.Add(m_Se[i].name, i);
		}
	}

	public int GetBgmIndex(string name)
	{
		if (m_BgmIndex.ContainsKey(name))
		{
			return m_BgmIndex[name];
		}
		else
		{
			Debug.LogError("指定された名前のBGMファイルが存在しません。");
			return 0;
		}
	}

	public int GetSeIndex(string name)
	{
		if (m_SeIndex.ContainsKey(name))
		{
			return m_SeIndex[name];
		}
		else
		{
			Debug.LogError("指定された名前のSEファイルが存在しません。");
			return 0;
		}
	}

	public void PlayBgm(int index)
	{
		index = Mathf.Clamp(index, 0, m_Bgm.Length);

		m_BgmAudioSource.clip = m_Bgm[index];
		m_BgmAudioSource.loop = true;
		m_BgmAudioSource.volume = BgmVolume * Volume;
		m_BgmAudioSource.Play();
	}

	public void PlayBgmByName(string name)
	{
		PlayBgm(GetBgmIndex(name));
	}

	public void StopBgm()
	{
		m_BgmAudioSource.Stop();
		m_BgmAudioSource.clip = null;
	}

	public void PlaySe(int index)
	{
		index = Mathf.Clamp(index, 0, m_Se.Length);
		m_SeAudioSource.PlayOneShot(m_Se[index], SeVolume * Volume);
	}

	public void PlaySeByName(string name)
	{
		PlaySe(GetSeIndex(name));
	}

	public void StopSe()
	{
		m_SeAudioSource.Stop();
		m_SeAudioSource.clip = null;
	}
}
