using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Manager References
    public GameManager gameManager;
    public JobManager jobManager;
    #endregion

    #region Text References
    public Text money;
    public Text workers;
    public Text fish;
    public Text wood;
    public Text timber;
    public Text wool;
    public Text clothes;
    public Text potato;
    public Text schnapps;
    public Text gameOver;
    #endregion

    // Update is called once per frame
    void Update()
    {
        money.text = gameManager._money.ToString();
        workers.text = jobManager.GetNumOfWorkers().ToString();

        fish.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Fish].ToString();
        wood.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Wood].ToString();
        timber.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Planks].ToString();
        wool.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Wool].ToString();
        clothes.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Clothes].ToString();
        potato.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Potato].ToString();
        schnapps.text = gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Schnapps].ToString();

        gameOver.text = gameManager.gameOverText;
    }
}
