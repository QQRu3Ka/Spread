using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameState>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SpreadResolver>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TurnManager>().AsSingle().NonLazy();

        Container.Bind<GameInputSystem>().AsSingle();
    }
}