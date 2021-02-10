using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
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
    /// 
    /// </summary>
    public void DisplayMovableMark()
    {
        m_Quad.SetActive(true);
        m_Quad.GetComponent<Renderer>().material = m_MovableMarkMaterial;
    }

    /// <summary>
    /// 
    /// </summary>
    public void HideMovableMark()
    {
        m_Quad.GetComponent<Renderer>().material = null;
        m_Quad.SetActive(false);
    }
}
