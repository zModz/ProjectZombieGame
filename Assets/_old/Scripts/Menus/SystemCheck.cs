using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Text.RegularExpressions;
using UnityEngine;

public class SystemCheck : MonoBehaviour
{
    public ErrorMsgBehaviour errorMsg;

    // Start is called before the first frame update
    void Start()
    {
        errorMsg.ErrorMsg("CPU TOO WEAK", "Your CPU may be too weak to run this game", true);
        //CheckSystem();
    }

    public bool CheckSystem()
    {
        //Intel
        string rxIntel = @"([INTEL(R) CORE(TM) i]+[5|7|9]+[-]+[1-9][10|11]?)(\d{3})?(\d{2}[A-Z]\d)?([(A-Z)]{1})?([\s[:ascii:]]{0,99})?";
        string CPU = SystemInfo.processorType;
        int CPU_cores = SystemInfo.processorCount;
        //bool CheckCPU;

        Match mIntel = Regex.Match(CPU, rxIntel, RegexOptions.IgnoreCase);
        if (CPU_cores >= 2)
        {
            //CheckCPU = true;
        }
        else
        {
            errorMsg.ErrorMsg("CPU TOO WEAK", "Your CPU may be too weak to run this game", true);
            //CheckCPU = false;
        }

        return true;
    }
}
