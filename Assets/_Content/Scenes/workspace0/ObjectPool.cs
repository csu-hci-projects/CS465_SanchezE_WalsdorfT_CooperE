using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _expandBy = 5;

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                return child.gameObject;
            }
        }
        for (int i = 0; i < _expandBy; i++)
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
        }
        return GetObject();
    }
}
