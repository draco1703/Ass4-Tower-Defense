
using UnityEngine;
using UnityEngine.EventSystems;


public class Node : MonoBehaviour
{


    public Color hoverColor;
    public Color notEnoughMoney;
    public Color cantbuild;
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurrentBlueprint turrentBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    //private bool walkable = true;
    private Renderer rend;
    private Color startColor;
    public KnowNode knowNode;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        knowNode = GetComponent<KnowNode>();
    }

    public Vector3 GetBuildPos()
    {
        return transform.position + positionOffset;
    }

    private void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (turret != null)
        {
            BuildManager.instance.SelectNode(this);
            return;
        }
        if (!BuildManager.instance.CanBuild)
            return;

        if (!knowNode.Buildable)
            return;

       

      
        BuildTurret(BuildManager.instance.GetTurretToBuild());
        Pathfinding.instace.giveNode(this);
                
    }


    void BuildTurret(TurrentBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject _turrent = Instantiate(blueprint.prefab, GetBuildPos(), transform.rotation);
        turret = _turrent;
        //Build Effect
        GameObject effect = Instantiate(BuildManager.instance.buildEffect, GetBuildPos(), transform.rotation);
        Destroy(effect, 5f);

        turrentBlueprint = blueprint;
        knowNode.boolUpdatate(false);
    }
    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turrentBlueprint.upgradeCost)
        {
            return;
        }

        PlayerStats.Money -= turrentBlueprint.upgradeCost;

        Destroy(turret);

        GameObject _turrent = Instantiate(turrentBlueprint.upgradedPrefab, GetBuildPos(), transform.rotation);
        turret = _turrent;

        //Opgrade effect
        GameObject effect = Instantiate(BuildManager.instance.opgradeEffect, GetBuildPos(), transform.rotation);
        Destroy(effect, 5f);

        isUpgraded = true;
    }
    public void SellTurret()
    {
        PlayerStats.Money += turrentBlueprint.GetSellAmount();
        //Sell effect
        GameObject effect = Instantiate(BuildManager.instance.sellEffect, GetBuildPos(), transform.rotation);
        Destroy(effect, 5f);

        Destroy(turret);
        knowNode.boolUpdatate(true);
        turrentBlueprint = null;
        knowNode.boolUpdatate(true);
    }

    public void Refund()
    {
       
        PlayerStats.Money += turrentBlueprint.cost;
        Destroy(turret);
        turret = null;
        knowNode.boolUpdatate(true);
    }
    private void OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!BuildManager.instance.CanBuild)
            return;

        rend.enabled = true;
        if (knowNode.Buildable)
        {
            if (BuildManager.instance.HasMoney)
            {

                rend.material.color = hoverColor;
            }
            else
            {
                rend.material.color = notEnoughMoney;
            }
        }
        else
        {
            rend.material.color = cantbuild;
        }

    }

    private void OnMouseExit()
    {
        rend.enabled = false;
    }
}
