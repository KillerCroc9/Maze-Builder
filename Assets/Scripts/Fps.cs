using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Fps : MonoBehaviour
{
    private float count;
    public TextMeshProUGUI text;

    private IEnumerator Start()
    {
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            yield return new WaitForSeconds(0.1f);
            text.text = count+" ";
        }
    }

  
}