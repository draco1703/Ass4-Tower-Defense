
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurrentBlueprint standardTurrent;
    public TurrentBlueprint missileTurrent;
    public TurrentBlueprint laserBeamer;
    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurrent()
    {
        //buildManager.SetTurrentBuild(buildManager.standardTurrentPrefab);
        buildManager.SelectTurretToBuild(standardTurrent);
    }
    public void SelectMissileLauncher()
    {
        buildManager.SelectTurretToBuild(missileTurrent);
        //buildManager.SetTurrentBuild(buildManager.missileLaunvherPrefab);
    }
    public void SelectLaserBeamer()
    {
        buildManager.SelectTurretToBuild(laserBeamer);
    }

}
