using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UISetToggle : MonoBehaviour
{
    public Color selected;
    public Color deselected;
    public GameObject panelParent;

    private Image[] tabs;
    private int on;
    private int lastChild;

    // Start is called before the first frame update
    void Start()
    {
        on = 0;
        lastChild = 0;
        tabs = GetComponentsInChildren<Image>();
        tabs[on].color = selected;
    }
    
    public void SetActive(int i)
    {
        tabs[on].color = deselected;
        on = i;
        tabs[on].color = selected;

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
