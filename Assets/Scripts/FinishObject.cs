using UnityEngine;

public class FinishObject : MonoBehaviour
{
    // Ссылка на генератор мира, чтобы сказать ему, что мы исчезли
    public InfiniteWorld worldManager;

    void OnDestroy()
    {
        if (worldManager != null)
        {
            worldManager.isFinishInWorld = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ФИНИШ! ТЫ НАШЕЛ ВЫХОД");
            // Тут можно включить экран "Победа" или просто закрыть игру
            Application.Quit(); // Закроет игру (работает в билде)
        }
    }
}