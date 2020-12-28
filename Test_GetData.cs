using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tsinghua.HCI.IoThingsLab;



public class Test_GetData : MonoBehaviour
{
    protected Text l_DataText;
    protected Text r_DataText;
    // Start is called before the first frame update
    void Start()
    {
        l_DataText = transform.GetChild(0).GetComponent<Text>();
        r_DataText = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //l_DataText.text = GestureRecognizerItem.L_Data;
        //r_DataText.text = GestureRecognizerItem.R_Data;
        l_DataText.text = Get_VideoPath.hint_str;
        r_DataText.text = Cut_Video.hint_str;
    }
}
