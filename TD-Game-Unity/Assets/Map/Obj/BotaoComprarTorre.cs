using TMPro;
using UnityEngine;

public class BotaoComprarTorre : MonoBehaviour
{
    [SerializeField] private Shop gameManager;
    [SerializeField] private Shop.TipoDeTorre tipo;
    [SerializeField] private TextMeshProUGUI costText;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        costText.text = gameManager.GetCost(tipo).ToString();
    }

    void Update()
    {
        bool canAfford = gameManager.CanAfford(tipo);
        canvasGroup.alpha = canAfford ? 1f : 0.6f;
        canvasGroup.interactable = canAfford;
    }

    public void Comprar()
    {
        gameManager.ComprarTorre(tipo);
        transform.parent.gameObject.SetActive(false);
    }
}
