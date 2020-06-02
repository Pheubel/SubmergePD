using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Valve.VR;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] private MenuSection[] _sections;

    [SerializeField] private float _offsetDegree = 0f;
    [SerializeField] private float _spacingDegree;

    [SerializeField] MenuSectionHoverEvent _hoverChanged;

    protected Vector2 TouchPosition { get; private set; } = Vector2.zero;
    protected MenuSection SelectedSection { get; private set; }

    // Update is called once per frame
    protected virtual void Update()
    {
#if UNITY_EDITOR
        Assert.IsTrue(IsValid(), "Total covered degrees cannot exceep 360 degrees and must be larger than 0.");
#endif

        Vector2 direction = Vector2.zero + TouchPosition;
        float rotation = (GetDegree(direction) + _offsetDegree) % 360f;

        var section = GetSection(rotation);

        if (SelectedSection != section)
        {
            SelectedSection = section;
            _hoverChanged?.Invoke(section);
        }
    }

    private MenuSection GetSection(float rotation)
    {
        float remainingRotation = rotation + (_spacingDegree / 2);

        for (int i = 0; i < _sections.Length; i++)
        {
            remainingRotation -= (_sections[i].Size + _spacingDegree);

            if (remainingRotation <= 0)
                return _sections[i];
        }

        return null;
    }

    private float GetDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        if (value < 0)
            value += 360.0f;

        return value;
    }

    public void SetTouchPosition(Vector2 axis)
    {
        TouchPosition = axis;
    }

    public void HandlePressed()
    {
        SelectedSection?.SectionSelected?.Invoke();
    }

#if UNITY_EDITOR
    private bool IsValid()
    {
        float totalDegrees = 0;

        for (int i = 0; i < _sections.Length; i++)
        {
            totalDegrees += _sections[0].Size + _spacingDegree;
        }

        return totalDegrees >= 0 && totalDegrees <= 360f;
    }
#endif
}

[Serializable]
public class MenuSection
{
    public GameObject SectionObject => _sectionObject;
    public float Size => _size;
    public UnityEvent SectionSelected => _sectionSelected;

    [SerializeField] private GameObject _sectionObject;
    [SerializeField] private float _size = 40f;
    [SerializeField] private UnityEvent _sectionSelected;
}

[Serializable]
public class MenuSectionHoverEvent : UnityEvent<MenuSection> { }
