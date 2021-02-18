using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マス一つ一つを管理するクラス。
/// </summary>
public class Cell : MonoBehaviour
{
	public GameObject m_Quad;
	public Material m_MovableMarkMaterial;
	public int m_PositionX;
	public int m_PositionY;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 可動領域を表す色付きのQuadを表示するメソッド。
	/// </summary>
	public void DisplayMovableMark()
	{
		m_Quad.SetActive( true );
		m_Quad.GetComponent<Renderer>().material = m_MovableMarkMaterial;
	}

	/// <summary>
	/// 可動領域を表す色付きのQuadを非表示にするメソッド。
	/// </summary>
	public void HideMovableMark()
	{
		m_Quad.GetComponent<Renderer>().material = null;
		m_Quad.SetActive( false );
	}
}
