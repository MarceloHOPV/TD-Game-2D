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

    private GameObject torreSendoColocada;

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

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ConfirmarColocacao();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelarColocacao();
        }
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

        torreSendoColocada = Instantiate(opcao.prefab);
        torreSendoColocada.GetComponent<Turret>().enabled = false;
    }

    private void ConfirmarColocacao()
    {
        torreSendoColocada.GetComponent<Turret>().enabled = true;
        torreSendoColocada = null;
    }

    private void CancelarColocacao()
    {
        Destroy(torreSendoColocada);
        torreSendoColocada = null;
    }
}
