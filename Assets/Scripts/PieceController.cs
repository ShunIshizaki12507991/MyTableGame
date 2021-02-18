using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒を管理するクラス。
/// </summary>
public class PieceController : MonoBehaviour
{
	public bool m_IsKingPiece = false;
	public GameObject m_Cone;
	public int m_PositionX;
	public int m_PositionY;

	/// <summary>
	/// この駒を王将として選んだ際に呼ばれるメソッド。
	/// </summary>
	public void OnChooseAsKing()
	{
		m_IsKingPiece = true;
	}

	/// <summary>
	/// 王将に選択したうえで目印を表示するメソッド。
	/// </summary>
	public void OnChooseAsKingWithCone()
	{
		m_IsKingPiece = true;
		m_Cone.SetActive( true );
	}

	/// <summary>
	/// 王将を切り替えるメソッド。
	/// </summary>
	public void ResetKing()
	{
		m_IsKingPiece = false;
		m_Cone.SetActive( false );
	}

	/// <summary>
	/// 王将の目印を表示するメソッド。
	/// </summary>
	public void ShowCone()
	{
		m_Cone.SetActive( true );
	}
}
