using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    // Start is called before the first frame update
    public string textToPass;
    private TMP_Text m_TextComponent;
    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.text = textToPass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
