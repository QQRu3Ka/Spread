using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    public ColorConfig ColorConfig;
    public PlayersConfig PlayersConfig;
    public MapsConfig MapsConfig;
    public override void InstallBindings()
    {
        Container.BindInstances(ColorConfig, PlayersConfig, MapsConfig);
    }
}