using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 先攻か後攻かを表示するダイアログのクラス。
/// </summary>
public class GameStartDialogContoller : MonoBehaviour
{
	public Text m_Text;

	/// <summary>
	/// 先攻プレイヤーの数字で判別してTextのメッセージを変えるメソッド。
	/// </summary>
	/// <param name="startOwnTurn"></param>
	public void SetDialogMessage( int startPlayerNumber )
	{
		//Debug.Log("Change Text");
		if( startPlayerNumber == 1 )
		{
			m_Text.text = "あなたは<color=#ff0000>先攻</color>です。";
		}
		else if( startPlayerNumber == 2 )
		{
			m_Text.text = "あなたは<color=#0000ff>後攻</color>です。";
		}
	}

	/// <summary>
	/// ダイアログ内のボタンを押下した際に呼ばれるメソッド。
	/// </summary>
	public void OnClickOkButton()
	{
		GameController gc = GameObject.Find( "GameController" ).GetComponent<GameController>(); ;
		gc.OnSelectedKing();
		Destroy( this.gameObject );
		Destroy( this );
	}
}
