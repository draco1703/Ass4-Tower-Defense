
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager instance;

    public GameObject buildEffect;
    public GameObject opgradeEffect;
    public GameObject sellEffect;

    private TurrentBlueprint turrenTobuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    private void Awake()
    {
        instance = this;
    }


    //public GameObject standardTurrentPrefab;


    //public GameObject missileLaunvherPrefab;

    public bool CanBuild { get { return turrenTobuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turrenTobuild.cost; } }

    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turrenTobuild = null;

        nodeUI.SetTarget(node);
    }
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }
    public void SelectTurretToBuild(TurrentBlueprint turrent)
    {
        turrenTobuild = turrent;
        DeselectNode();
    }
    public TurrentBlueprint GetTurretToBuild()
    {
        return turrenTobuild;
    }
}

