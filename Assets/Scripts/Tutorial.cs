using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //Public
    public int currentPanel;
    public Text title;
    public Text explanation;
    public Button next;
    public Button back;
    public Dropdown dropdown;
    public GameObject tutorialPanel;
    public GameObject dishPanel;

    private readonly string[][] information = new string[][]
    {
        new string[]{ "Tutorial Information", "\tThis tutorial will explain the functionality of the program. To navigate the tutorial, either click the \"Back\" or \"Next\"" +
            " buttons to iteratively go through each section or click the center button currently labeled \"Introduction\" to chose a specific entry." +
	        "\n\tIf you think we left out or could have explained a specfic topic more, please submit an issue on our gitlab page." +"" +
            "\n\n\tWe hope our program encourages you to continue learning about microbiology and helps deepen your understanding of the topic as a whole." },
        new string[]{ "Left Panel", "\tThe left panel contains two main components: the \"Begin Simulation\" button and the log panel.\n"+
                      "\n\tThe \"Begin Simulation\" button does excatly what is stated, it begins the simulation.\n" + 
                      "\n\tThe log panel prints out all information a user may find interesting as it is happening. This includes the number of new and dead cells generated, old enviornment settings and more." },
        new string[]{ "Center Panel", "\tThe Center Panel also has two features: a visual of the petridish and the tutorial. As you know the tutorial is a text guide " +
            "detailing information about the program as a whole. \n\tOn the otherhand, the petridish view is a visual represenation of the eColi after growth. Additionally" +
            " you can move the petridish around in the scene by clicking anywhere in the center panel and dragging your mouse. This is benefical if you can't quite see what a specific colony looks like." },
        new string[]{ "Right Panel", "\tThe Right Panel contains the Graph, Quiz, and Settings. The latter of which has it's own page in this tutorial. You can navigate this panel by clicking your desired tab at the top.\n"+
                      "\tThe Graph represents cell growth over time, where cell population is the y-axis and time is the x-axis. After petridish generatation you can click on points in the graph to view the dish at specific times." +
                      "\n\tThe Quiz lets teachers guide students without having to attend to them directly. A quiz can be created on our webpage under the \"Create Quiz\" tab. After creation a student needs to download the quiz and" +
            " then click browse in the quiz tab. This opens a file dilog; select the quiz in your system and click open to begin." },
        new string[]{ "Settings", "\tThe Settings tab, let's the use control the enviornmental elements the grow their eColi in. The following are sliders, grab the point or click somewhere on the bar to change it. By default the variables are in their neutral states.\n"+
                "\t\"Colony Count\" is the number of colonies that will form initially, you can think of this as number of starting cells.\n" +"\t\"Trial Length\" represents how much time (hours:minutes) the bacteria is allowed to grow for.\n" +
                "\t\"Temperature\" is the ambient temperature the bacteria is grown in.\n" + "\t\"Light Exposure\" represents the type of light wavelength the bacteria is grown under.\n" + "\t\"Bleach\" if checked the solution will include bleach." },
        new string[]{ "Footer", "\tThe Footer contains nothing on application start, but as the user simulates different trials it will populate with representations of that trial, each is named \"Trial n : cellCount\". \n\tThe user can click that representation" +
            " to load the old trial into the dish view. The log panel will populate with all the different settings for thaat trial. This is helpfully when trying to view how affecting one or two variables may change the outcome of the eColi poulation." }
    };

    void Start()
    {
        currentPanel = 0;
        back.onClick.AddListener(delegate { currentPanel--; UpdatePanel(); });
        next.onClick.AddListener(delegate { currentPanel++; UpdatePanel(); });
        dropdown.onValueChanged.AddListener(delegate { currentPanel=dropdown.value; UpdatePanel(); });
    }
    
    public void SwitchTutorial()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        dishPanel.SetActive(!dishPanel.activeSelf);
    }

    private void UpdatePanel()
    {
        back.interactable = currentPanel != 0;
        next.interactable = currentPanel != 5;
        dropdown.value = currentPanel;
        title.text = information[currentPanel][0];
        explanation.text = information[currentPanel][1];
    }
}
