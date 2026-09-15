using UnityEngine;
using UnityEngine.Serialization;

public class AreaDeSelecao : MonoBehaviour
{
    [FormerlySerializedAs("bordaSelecao")]
    [SerializeField] private GameObject selectionBorder;
    [FormerlySerializedAs("indicadorAlcance")]
    [SerializeField] private GameObject rangeIndicator;

    private static AreaDeSelecao currentlySelected;
    private bool isSelected;

    void Awake()
    {
        CircleCollider2D rangeCollider = GetComponentInParent<CircleCollider2D>();
        rangeIndicator.GetComponent<Circulo>().SetRadius(rangeCollider.radius);
    }

    void OnMouseDown()
    {
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            if (currentlySelected != null)
            {
                currentlySelected.Deselect();
            }

            Select();
        }
    }

    private void Select()
    {
        isSelected = true;
        currentlySelected = this;
        selectionBorder.SetActive(true);
        rangeIndicator.SetActive(true);
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
    }
}
