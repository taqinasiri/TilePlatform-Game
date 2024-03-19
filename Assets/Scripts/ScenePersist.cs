using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        int numberFoScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if(numberFoScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}