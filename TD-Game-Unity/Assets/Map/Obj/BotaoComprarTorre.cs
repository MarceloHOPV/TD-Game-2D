using UnityEngine;

public class BotaoComprarTorre : MonoBehaviour
{
    [SerializeField] private Shop gameManager;
    [SerializeField] private Shop.TipoDeTorre tipo;

    public void Comprar()
    {
        gameManager.ComprarTorre(tipo);
        transform.parent.gameObject.SetActive(false);
    }
}
