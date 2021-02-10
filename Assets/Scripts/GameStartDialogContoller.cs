using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GameStartDialogContoller : MonoBehaviour
{
    public Text m_Text;

    /// <summary>
    /// 自分の初めての手番によって先攻後攻を判別してTextのメッセージを変えるメソッド。
    /// </summary>
    /// <param name="startOwnTurn"></param>
    public void SetDialogMessage(int startOwnTurn)
    {
        //Debug.Log("Change Text");
        if (startOwnTurn == 1)
        {
            m_Text.text = "あなたは<color=#ff0000>先攻</color>です。";
        }
        else if (startOwnTurn == 2)
        {
            m_Text.text = "あなたは<color=#0000ff>後攻</color>です。";
        }
    }

    public void OnClickOkButton()
    {
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>(); ;
        gc.OnSelectedKing();
        Destroy(this.gameObject);
        Destroy(this);
    }
}
