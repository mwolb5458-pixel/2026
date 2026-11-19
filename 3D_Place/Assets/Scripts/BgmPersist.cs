using UnityEngine;

public class BgmPersist : MonoBehaviour
{
    private static BgmPersist instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}