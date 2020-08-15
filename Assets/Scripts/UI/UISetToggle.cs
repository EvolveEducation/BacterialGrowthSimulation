using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetToggle : MonoBehaviour
{
    public Color selected;
    public Color deselected;
    public GameObject panelParent;
    public int intialOn;

    private Image[] tabs;
    private int on;
    private int lastChild;

    void Awake()
    {
        if (intialOn != -1)
        {
            on = intialOn;
            lastChild = on / 2;
        }
        
        tabs = GetComponentsInChildren<Image>();
        if (panelParent != null && intialOn != -1)
        {
            tabs[on].color = selected;
            SwitchTextColor(on);
        }
    }

    public void SwitchToggle(int i)
    {
        tabs[on].color = deselected;
        on = i;
        tabs[on].color = selected;
    }

    public void SwitchTextColor(int i)
    {
        tabs[on].transform.GetChild(0).GetComponentInChildren<Text>().color = Color.black;
        SwitchToggle(i);
        tabs[on].transform.GetChild(0).GetComponentInChildren<Text>().color = Color.white;
    }

    public void SetActivePanel(int i)
    {
        SwitchTextColor(i);

        switch (on)
        {
            case 0:
                panelParent.transform.GetChild(lastChild).transform.gameObject.SetActive(false);
                panelParent.transform.GetChild(0).transform.gameObject.SetActive(true);
                lastChild = 0;
                break;
            case 2:
                panelParent.transform.GetChild(lastChild).transform.gameObject.SetActive(false);
                panelParent.transform.GetChild(1).transform.gameObject.SetActive(true);
                lastChild = 1;
                break;
            case 4:
                panelParent.transform.GetChild(lastChild).transform.gameObject.SetActive(false);
                panelParent.transform.GetChild(2).transform.gameObject.SetActive(true);
                lastChild = 2;
                break;
        }
    }
}
