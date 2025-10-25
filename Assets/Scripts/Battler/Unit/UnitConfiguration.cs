using AutoBattler;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfiguration", menuName = "AutoBattler/UnitConfiguration")]
public class UnitConfiguration : ScriptableObject
{
    [SerializeField]
    private UnitType unitType;

    [BoxGroup("Interface Settings")]
    [PreviewField(70)]
    [LabelText("Interface Icon Preview")]
    [SerializeField]
    private Sprite interfaceIcon;

    [BoxGroup("Interface Settings")]
    [LabelText("Interface Description Key")]
    [TextArea(2, 4)]
    [SerializeField]
    private string interfaceDescriptionKey;

    public UnitType UnitType => unitType;
    public string InterfaceDescriptionKey => interfaceDescriptionKey;
    public Sprite InterfaceIcon => interfaceIcon;
}