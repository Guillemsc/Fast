using System;
using UnityEngine;

namespace Fast.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Fast/GameConfig", order = 1)]
    public class GameConfigAsset : ScriptableObject
    {
        [SerializeField] private string game_name = "";
        [SerializeField] private string game_version = "";
        [SerializeField] private GameStage game_stage = new GameStage();
    }
}
