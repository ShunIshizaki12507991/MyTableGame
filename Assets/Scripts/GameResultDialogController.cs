using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム終了時にプレイヤーの勝敗を表示するダイアログのクラス。
/// </summary>
public class GameResultDialogController : MonoBehaviour
{
	public Text m_Text;

	/// <summary>
	/// 先攻プレイヤーの数字で判別してダイアログのメッセージをセットするメソッド。
	/// </summary>
	/// <param name="winnerPlayerNumber"></param>
	public void SetDialogMessage( int winnerPlayerNumber )
	{
		Debug.Log( "Set Text" );

		if( winnerPlayerNumber == 1 )
		{
			m_Text.text = "あなたの<color=#ff0000>勝利</color>です。";
		}
		else if( winnerPlayerNumber == 2 )
		{
			m_Text.text = "あなたの<color=#0000ff>負け</color>です...";
		}
	}

	/// <summary>
	/// "もう一戦"ボタンを押下した際に呼ばれるメソッド。
	/// </summary>
	public void OnClickRepeatButton()
	{
		SceneManager.LoadScene( SceneManager.GetActiveScene().name );
		Destroy( this.gameObject );
		Destroy( this );
	}

	/// <summary>
	/// "タイトルに戻る"ボタンを押下した際に呼ばれるメソッド。
	/// </summary>
	public void OnClickBackButton()
	{
		SceneManager.LoadScene( "TitleScene" );
		Destroy( this.gameObject );
		Destroy( this );
	}
}
