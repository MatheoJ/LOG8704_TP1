using UnityEngine;
using UnityEngine.UI;

public class BarreProgression : MonoBehaviour
{
    public Image fillImage;  // R�f�rence � l'image de remplissage
    public float currentValue = 0f;  // Valeur actuelle
    public float maxValue = 1f;    // Valeur maximale

    private void Start()
    {
        // Initialiser la barre de progression � la valeur actuelle
        UpdateProgressBar();
    }

    public void SetValue(float value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue); // Assurez-vous que la valeur est born�e
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        float fillAmount = currentValue / maxValue; // Calculer la proportion du remplissage
        fillImage.fillAmount = fillAmount;
    }
    
}