using TMPro;
using UnityEngine;

public class UIUpgradeHoverInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TitleText;
    [SerializeField] private TextMeshProUGUI _DescriptionText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToggleHoverInfo(bool toggle, string messageTitle, string messageDescription)
    {
        gameObject.SetActive(toggle);
        
        if (!toggle) return;
        _TitleText.text = messageTitle;
        _DescriptionText.text = messageDescription;
    }
}