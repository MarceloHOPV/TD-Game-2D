using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class AreaDeSelecao : MonoBehaviour
{
    [FormerlySerializedAs("bordaSelecao")]
    [SerializeField] private GameObject selectionBorder;
    [FormerlySerializedAs("indicadorAlcance")]
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private LayerMask selectableLayer;

    private static AreaDeSelecao currentlySelected;
    private bool isSelected;
    private Collider2D selectionCollider;
    private Turret turret;
    private SelectedTurretPanel selectedTurretPanel;

    void Awake()
    {
        selectionCollider = GetComponent<Collider2D>();
        turret = transform.parent.GetComponent<Turret>();
        selectedTurretPanel = GameObject.Find("Selected_Turret").GetComponent<SelectedTurretPanel>();

        CircleCollider2D rangeCollider = transform.parent.GetComponent<CircleCollider2D>();
        rangeIndicator.GetComponent<Circulo>().SetRadius(rangeCollider.radius);
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, selectableLayer);

        if (hit == selectionCollider)
        {
            if (isSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
        else if (isSelected)
        {
            Deselect();
        }
    }

    private void Select()
    {
        isSelected = true;
        currentlySelected = this;
        selectionBorder.SetActive(true);
        rangeIndicator.SetActive(true);
        selectedTurretPanel.Show(turret);
    }

    private void Deselect()
    {
        isSelected = false;

        if (currentlySelected == this)
        {
            currentlySelected = null;
        }

        selectionBorder.SetActive(false);
        rangeIndicator.SetActive(false);
        selectedTurretPanel.Hide();
    }
}
