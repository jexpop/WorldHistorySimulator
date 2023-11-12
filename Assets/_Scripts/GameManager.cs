using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameScene{
    MainMenu, // Main menu of the game
    MapEditor, // History editor
    MapSelect, // Select your polity
    MapPlay, // In the game, world map
    CityMap, // In the game, city view
    BattleMap // In the game, battle view
}

public class GameManager : Singleton<GameManager>
{

    public GameScene currentGameScene;


    void Start()
    {
        currentGameScene = GameScene.MapEditor;
        LoadScene(currentGameScene);
    }

    public void LoadScene(GameScene gameScene)
    {
        // Dynamic load of the scene with the GameScene parameter
        SceneManager.LoadScene(gameScene.ToString()+"Scene");
    }

}
