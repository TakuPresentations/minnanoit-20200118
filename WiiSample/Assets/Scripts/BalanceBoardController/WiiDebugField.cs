using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WiiDebugField : MonoBehaviour
{
    [SerializeField] private Text consoleText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<string> stringList = new List<string>();
        stringList.Add("remoteCount:" + Wii.GetRemoteCount());
        for(int i = 0;i < Wii.GetRemoteCount(); ++i)
        {
            if (Wii.IsActive(i) && Wii.GetExpType(i) == 3)
            {
                Vector4 theBalanceBoard = Wii.GetBalanceBoard(i);
                Vector2 theCenter = Wii.GetCenterOfBalance(i);
                float topWeight = theBalanceBoard.x + theBalanceBoard.y;
                float bottomWeight = theBalanceBoard.z + theBalanceBoard.w;
                float topBottomDiff = topWeight - bottomWeight;
                stringList.Add("totalWeight:  " + Wii.GetTotalWeight(i) + " kg");
                stringList.Add("centerX:      " + theCenter.x);
                stringList.Add("centerY:      " + theCenter.y);
                stringList.Add("topRight:     " + theBalanceBoard.x + " kg");
                stringList.Add("topLeft:      " + theBalanceBoard.y + " kg");
                stringList.Add("bottomRight:  " + theBalanceBoard.z + " kg");
                stringList.Add("bottomLeft:   " + theBalanceBoard.w + " kg");
                stringList.Add("topWeight:    " + topWeight + " kg");
                stringList.Add("bottomWeight: " + bottomWeight + " kg");
                stringList.Add("topBottomDiff:" + topBottomDiff + " kg");
            }
        }
        consoleText.text = string.Join("\n", stringList);
    }

    public void OnForwardButtonClicked() {
        TaikoController.Instance.GoForward(1.0f);
    }

    public void OnBackButtonClicked()
    {
        TaikoController.Instance.MoveBack(1.0f);
    }

    public void OnStopButtonClicked()
    {
        TaikoController.Instance.StopTaiko();
    }
}
