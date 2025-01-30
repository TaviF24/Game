using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Codice.Client.BaseCommands.Import.Commit;

public class PlayerUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI promptText;
    public GameObject assaultHUD, anticipationHUD;
    public GameObject finishScreen;

    private void Update()
    {
        if (DetectionManager.instance.concealment >= 100 || DetectionManager.instance.alreadyDetected)
        {
            if (DetectionManager.instance.concealmentHUDText != null)
            {
                StartCoroutine(DetectionManager.instance.ClearDetectedText());
            }
            return;
        }
    }
    public void ActivateAssaultHUD(bool value)
    {
        assaultHUD.SetActive(value);
    }

    public void ActivateAnticipationHUD(bool value)
    {
        anticipationHUD.SetActive(value);
    }

    public void UpdateText(string promptMessage)
	{
		promptText.text = promptMessage;
	}
}
