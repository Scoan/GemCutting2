using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GemStoneCatalog;
using Voxels;
using UnityEngine.UI;

public class CatalogPanel : MonoBehaviour {

    // TODO: Render layers are pretty limited (only 32, and 8 are reserved). Not gonna fit entire catalog
    //  Instead of putting the gems on layers, why not render them progressively?
    // Hide gems, on each frame manually unhide+render to RT + hide

    public GameObject gemPrefab;
    public GameObject camPrefab;

    private GameObject _panelObject;
    private Camera _camera;
    private GemStone _gem;
    public RawImage rawImageComponent;

    // Use this for initialization
    void Awake () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private int _idx;
    public int idx
    {
        get { return _idx; }
        set {
            _idx = value;
            if (_panelObject != null)
            {
                _panelObject.transform.position = new Vector3(0, -50 * (value + 1), 0);
            }
        }
    }

    public void Initialize()
    {
        _panelObject = new GameObject("Catalog Panel Dummy");

        // Create gem
        GameObject gemObject = Instantiate(gemPrefab);
        gemObject.layer = 8;
        _gem = gemObject.GetComponent<GemStone>();
        SetGem(GemTypes.AidennMask);

        // Create camera
        GameObject camObject = Instantiate(camPrefab);
        _camera = camObject.GetComponent<Camera>();
        // TODO: Refine render texture resolution
        _camera.targetTexture = new RenderTexture(250, 180, 16, RenderTextureFormat.RGB565);
        rawImageComponent.texture = _camera.targetTexture;


        gemObject.transform.SetParent(_panelObject.transform);
        camObject.transform.SetParent(_panelObject.transform);
    }

    public void SetGem(GemTypes gemType)
    {
        if (_gem == null)
            return;
        _gem.GemType = gemType;
    }

    public void OnVisibilityToggled(bool visible)
    {
        _camera.gameObject.SetActive(visible);
    }
}
