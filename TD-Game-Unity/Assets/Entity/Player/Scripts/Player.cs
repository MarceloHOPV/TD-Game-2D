using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Money = 30f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddMoney(float amount)
    {
        Money += amount;
    }

    public float GetMoney()
    {
        return Money;
    }

}
