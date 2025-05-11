using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject uiRoot;
    public GameObject GunCamera;

    private bool isVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isVisible = !isVisible;
            uiRoot.SetActive(isVisible);
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            isVisible = !isVisible;
            GunCamera.SetActive(isVisible);
            uiRoot.SetActive(isVisible);
        }
    }
}
