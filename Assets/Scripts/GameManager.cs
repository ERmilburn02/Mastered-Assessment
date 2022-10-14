using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int m_ColorR = 0;
    private int m_ColorG = 0;
    private int m_ColorB = 0;

    private ulong m_BasePrice = 250000;

    private AssetReferenceGameObject m_CurrentCar = null;
    private GameObject m_CurrentCarGO = null;

    private bool m_ExtraNav = false;
    private bool m_ExtraHeatedSeats = false;

    [Header("Customiser")]
    [SerializeField] private TMP_Text m_ColorRedText = null;
    [SerializeField] private TMP_Text m_ColorGreenText = null;
    [SerializeField] private TMP_Text m_ColorBlueText = null;

    [SerializeField] private Slider m_ColorRedSlider = null;
    [SerializeField] private Slider m_ColorGreenSlider = null;
    [SerializeField] private Slider m_ColorBlueSlider = null;

    [SerializeField] private TMP_Text m_PriceText = null;

    [Header("Panels")]
    [SerializeField] private GameObject m_ModelPanel = null;
    [SerializeField] private GameObject m_ConfigurePanel = null;
    [SerializeField] private GameObject m_LoadingScreen = null;

    [Header("World GameObjects")]
    [SerializeField] private GameObject m_CarPlatform = null;

    [Header("Prefabs")]
    [SerializeField] private List<AssetReferenceGameObject> m_Cars = new List<AssetReferenceGameObject>();
    
    private void Start()
    {
        UpdateText();
    }

    public void OnColorRedUpdate(float newValue)
    {
        m_ColorR = (int)newValue;
        UpdateCarColor();
        UpdateText();
    }

    public void OnColorGreenUpdate(float newValue)
    {
        m_ColorG = (int)newValue;
        UpdateCarColor();
        UpdateText();
    }

    public void OnColorBlueUpdate(float newValue)
    {
        m_ColorB = (int)newValue;
        UpdateCarColor();
        UpdateText();
    }

    public void OnBuyButtonClicked()
    {

    }

    public void OnCancelButtonClicked()
    {
        m_ColorRedSlider.value = 0;
        m_ColorGreenSlider.value = 0;
        m_ColorBlueSlider.value = 0;
        UnloadCar();
        m_CarPlatform.SetActive(false);
        m_ConfigurePanel.SetActive(false);
        m_ModelPanel.SetActive(true);
    }

    public void OnConfigureButtonClicked(int index)
    {
        m_ModelPanel.SetActive(false);
        m_LoadingScreen.SetActive(true);
        LoadCar(index);
    }

    public void OnSetExtraNav(bool value)
    {
        m_ExtraNav = value;
        UpdateText();
    }

    public void OnSetExtraHeatedSeats(bool value)
    {
        m_ExtraHeatedSeats = value;
        UpdateText();
    }

    private void UpdateText()
    {
        m_ColorRedText.text = m_ColorR.ToString();
        m_ColorGreenText.text = m_ColorG.ToString();
        m_ColorBlueText.text = m_ColorB.ToString();

        ulong totalPrice = 0;
        totalPrice += m_BasePrice;
        totalPrice += (ulong)(m_ExtraNav ? 1000 : 0);
        totalPrice += (ulong)(m_ExtraHeatedSeats ? 1000 : 0);

        m_PriceText.text = string.Format("£{0:N0}", totalPrice);
    }

    private void UpdateCarColor()
    {
        CarCustomiser.Instance.SetCarColor(new Vector3((float)(m_ColorR / 255.0f), (float)(m_ColorG / 255.0f), (float)(m_ColorB / 255.0f)));
    }

    private void UnloadCar()
    {
        if (m_CurrentCarGO != null)
        {
            Destroy(m_CurrentCarGO);
            m_CurrentCarGO = null;
        }

        if (m_CurrentCar != null)
        {
            m_CurrentCar.ReleaseAsset();
            m_CurrentCar = null;
        }
    }

    private void LoadCar(int index)
    {
        var car = m_Cars[index];

        switch (index)
        {
            case 0:
                m_BasePrice = 250000;
                break;
            case 1:
                m_BasePrice = 100000;
                break;
            default:
                break;
        }

        UpdateText();

        car.LoadAssetAsync().Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                m_CurrentCarGO = (GameObject)Instantiate(car.Asset, m_CarPlatform.transform);
                m_CurrentCar = car;
                LoadCarCompleted();
            }
        };
    }

    private void LoadCarCompleted()
    {
        m_LoadingScreen.SetActive(false);
        m_CarPlatform.SetActive(true);
        m_ConfigurePanel.SetActive(true);
    }
}
