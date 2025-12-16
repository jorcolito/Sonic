using UnityEngine;

public class SpriteNumberDisplay : MonoBehaviour
{
    public Sprite[] numberSprites; 
    public int totalDigits = 3;
    public GameObject digitPrefab; 
    public float digitSpacing = 0.08f; 

    private SpriteRenderer[] digitRenderers;

    void Awake()
    {
        InitializeDigits();
    }

    private void InitializeDigits()
    {
        digitRenderers = new SpriteRenderer[totalDigits];

        for (int i = 0; i < totalDigits; i++)
        {
            GameObject digit = Instantiate(digitPrefab, transform);

            digit.transform.localPosition = new Vector3(i * digitSpacing, 0, 0); 
            
            digitRenderers[i] = digit.GetComponent<SpriteRenderer>();

            if (digitRenderers[i] == null)
            {
                Debug.LogError("El Prefab del Dígito debe tener un componente SpriteRenderer.");
                return;
            }
        }
    }

    public void SetNumber(int number)
    {
        int maxNumber = (int)Mathf.Pow(10, totalDigits) - 1;
        number = Mathf.Clamp(number, 0, maxNumber);

        string numStr = number.ToString();
        numStr = numStr.PadLeft(totalDigits, '0');

        for (int i = 0; i < totalDigits; i++)
        {
            if (i < numStr.Length)
            {
                char digitChar = numStr[i];
                int digitValue = digitChar - '0'; 

                if (digitValue >= 0 && digitValue < numberSprites.Length)
                {
                    digitRenderers[i].sprite = numberSprites[digitValue];
                }
            }
        }
    }
}