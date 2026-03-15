using UnityEngine;
using System.Collections.Generic;

public class PowerPanelManager : MonoBehaviour
{
    [SerializeField] private List<PanelFlicker> flickerPanels;

    public void RestorePower()
    {
        foreach (PanelFlicker panel in flickerPanels)
        {
            if (panel != null)
                panel.StopFlicker();
        }
    }
}