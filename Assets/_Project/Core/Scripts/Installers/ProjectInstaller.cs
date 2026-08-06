using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MapHolder>().AsSingle();

        Container.Bind<Player>().AsSingle().NonLazy();
        Container.Bind<PlayerStorage>().AsSingle().NonLazy();
    }
}