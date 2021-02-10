using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PieceController : MonoBehaviour
{
    public bool m_IsKingPiece = false;
    public GameObject m_Cone;
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
    /// 
    /// </summary>
    public void OnChooseAsKing()
    {
        m_IsKingPiece = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnChooseAsKingWithCone()
    {
        m_IsKingPiece = true;
        m_Cone.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetKing()
    {
        m_IsKingPiece = false;
        m_Cone.SetActive(false);
    }
}
