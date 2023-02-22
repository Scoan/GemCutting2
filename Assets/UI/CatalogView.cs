using System.Collections;
using System.Collections.Generic;
using GemCutting;
using UnityEngine;

public class CatalogView : MonoBehaviour
{
    // TODO: 
    //      - Controls to hide/show view
    //      - Controls to scroll through catalog
    //      - Controls to compare gem
    //      - Controls to rotate gem, or to jump into selected catalog gem for closer viewing

    public GameObject panelTemplate;
    public int curIdx = 0;

    private List<CatalogPanel> panels = new List<CatalogPanel>();
    private bool _inputEnabled = true;

    // Use this for initialization
    void Start () {
        InitializePanels();
        // For ease of development the view starts visible
        ToggleVisibility();
    }
	
	// Update is called once per frame
	void Update () {
        if (_inputEnabled)
            ProcessInput();
    }

    void InitializePanels()
    {
        if (panelTemplate == null)
            return;
        // TODO: Instantiate enough panels to fill the view
        // TODO: Instantiate panels w/ available gems
        for (int idx = 0; idx < 3; idx++)
        {
            GameObject res = Instantiate(panelTemplate, this.transform);
            CatalogPanel panelScript = res.GetComponent<CatalogPanel>();
            panels.Add(panelScript);
            panelScript.idx = idx;
        }
        ConfigurePanels(curIdx);
    }

    void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleVisibility();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            curIdx += 1;
            ConfigurePanels(curIdx);
        }
    }

    void ToggleVisibility()
    {
        // TODO: Instead of toggling visibility immediately, slide view off-screen smoothly
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        bool newVisibility = canvasGroup.alpha == 1 ? false : true;

        canvasGroup.alpha = newVisibility ? 1 : 0;
        foreach (CatalogPanel panel in panels)
        {
            panel.OnVisibilityToggled(newVisibility);
        }
    }

    void ConfigurePanels(int startingIdx)
    {
        for (int idx = 0; idx < panels.Count; idx++)
        {
            panels[idx].SetGem(startingIdx + (GemTypes)idx + 1);
        }
    }
}
