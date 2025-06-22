using UnityEngine;

public class MobileControlsVisibility : MonoBehaviour
{
    private void Start()
    {
        var textsObj = GameObject.Find("Texts");
        if (textsObj == null) return;

        foreach (Transform child in textsObj.transform)
        {
            if (child.name.Contains("MobileInputs"))
            {
                child.gameObject.SetActive(Application.isMobilePlatform);
            }
        }
    }
}
