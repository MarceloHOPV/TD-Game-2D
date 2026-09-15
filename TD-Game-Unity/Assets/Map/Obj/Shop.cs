using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    public enum TipoDeTorre
    {
        Torre_01,
        Torre_02,
        Torre_03
    }

    [System.Serializable]
    public class OpcaoDeTorre
    {
        public TipoDeTorre tipo;
        public GameObject prefab;
    }

    [SerializeField] private OpcaoDeTorre[] torresDisponiveis;
    [SerializeField] private GameObject painelBotoes;
    [SerializeField] private LayerMask invalidPlacementLayer;

    private static readonly Color CorInvalida = new Color(1f, 0f, 0f, 0.35f);

    private Player player;
    private GameObject torreSendoColocada;
    private CircleCollider2D bodyCollider;
    private GameObject[] overlaysInvalidos;
    private bool isValidPlacement;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            painelBotoes.SetActive(!painelBotoes.activeSelf);
        }

        if (torreSendoColocada == null)
            return;

        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;
        torreSendoColocada.transform.position = posicaoMouse;

        AtualizarValidadeDaColocacao(posicaoMouse);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isValidPlacement)
            {
                ConfirmarColocacao();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelarColocacao();
        }
    }

    private void AtualizarValidadeDaColocacao(Vector3 posicao)
    {
        isValidPlacement = !Physics2D.OverlapCircle(posicao, bodyCollider.radius, invalidPlacementLayer);

        foreach (GameObject overlay in overlaysInvalidos)
        {
            overlay.SetActive(!isValidPlacement);
        }
    }

    public float GetCost(TipoDeTorre tipo)
    {
        OpcaoDeTorre opcao = System.Array.Find(torresDisponiveis, t => t.tipo == tipo);
        return opcao.prefab.GetComponent<Turret>().GetCost();
    }

    public bool CanAfford(TipoDeTorre tipo)
    {
        return player.GetMoney() >= GetCost(tipo);
    }

    public void ComprarTorre(TipoDeTorre tipo)
    {
        if (torreSendoColocada != null)
        {
            CancelarColocacao();
        }

        OpcaoDeTorre opcao = System.Array.Find(torresDisponiveis, t => t.tipo == tipo);

        if (opcao == null)
        {
            Debug.LogError($"Nenhum prefab configurado pra {tipo}.");
            return;
        }

        float custo = opcao.prefab.GetComponent<Turret>().GetCost();

        if (!player.SpendMoney(custo))
        {
            return;
        }

        torreSendoColocada = Instantiate(opcao.prefab);
        torreSendoColocada.GetComponent<Turret>().enabled = false;
        torreSendoColocada.GetComponentInChildren<AreaDeSelecao>().enabled = false;

        bodyCollider = torreSendoColocada.transform.Find("Body").GetComponent<CircleCollider2D>();
        bodyCollider.enabled = false;

        CriarOverlaysInvalidos();
    }

    private void CriarOverlaysInvalidos()
    {
        SpriteRenderer[] spritesOriginais = torreSendoColocada.GetComponentsInChildren<SpriteRenderer>();
        overlaysInvalidos = new GameObject[spritesOriginais.Length];

        for (int i = 0; i < spritesOriginais.Length; i++)
        {
            SpriteRenderer original = spritesOriginais[i];

            GameObject overlay = new GameObject("OverlayInvalido");
            overlay.transform.SetParent(original.transform, false);

            SpriteRenderer overlayRenderer = overlay.AddComponent<SpriteRenderer>();
            overlayRenderer.sprite = original.sprite;
            overlayRenderer.color = CorInvalida;
            overlayRenderer.sortingLayerID = original.sortingLayerID;
            overlayRenderer.sortingOrder = original.sortingOrder + 1;

            overlay.SetActive(false);
            overlaysInvalidos[i] = overlay;
        }
    }

    private void ConfirmarColocacao()
    {
        torreSendoColocada.GetComponent<Turret>().enabled = true;
        torreSendoColocada.GetComponentInChildren<AreaDeSelecao>().enabled = true;
        bodyCollider.enabled = true;

        foreach (GameObject overlay in overlaysInvalidos)
        {
            Destroy(overlay);
        }

        torreSendoColocada = null;
    }

    private void CancelarColocacao()
    {
        Destroy(torreSendoColocada);
        torreSendoColocada = null;
    }
}
