using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControler : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public Material notSelectedMaterial;
    public Material selectedMaterial;
    private bool canPlayerInteract;
    // Start is called before the first frame update
    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown() {
        _meshRenderer.material = selectedMaterial;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)){
            _meshRenderer.material = notSelectedMaterial;
        }
    }
}
