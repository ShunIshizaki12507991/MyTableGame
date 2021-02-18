using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面を管理するクラス。
/// </summary>
public class TitleController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 画面内の"CPU戦"ボタンを押下した際に呼ばれるメソッド。
	/// </summary>
	public void OnClickCPUGameButton()
	{
		SceneManager.LoadScene( "MainGame" );
	}

	/// <summary>
	///
	/// </summary>
	public void OnClickOnlineGameButton()
	{
		return;
	}
}
