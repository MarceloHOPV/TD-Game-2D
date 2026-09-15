using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTurretPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image turretImage;
    [SerializeField] private float sellRefundPercentage = 0.7f;

    private Turret selectedTurret;

    void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(Turret turret)
    {
        selectedTurret = turret;
        nameText.text = turret.GetTurretName();
        turretImage.sprite = turret.GetComponentInChildren<SpriteRenderer>().sprite;
        panel.SetActive(true);
    }

    public void Hide()
    {
        selectedTurret = null;
        panel.SetActive(false);
    }

    public void Sell()
    {
        if (selectedTurret == null)
            return;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.AddMoney(selectedTurret.GetCost() * sellRefundPercentage);

        Destroy(selectedTurret.gameObject);
        Hide();
    }
}
