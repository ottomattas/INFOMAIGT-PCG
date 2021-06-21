using UnityEngine;

public class GroundSkyBoxGenerator : ConfiguratorBase
{
    private const float generateInterval = 5;

    private float nextGenerateTime;

    private void Awake()
    {
        GeneratePalette();
        SetupSkyboxAndPalette();
    }

    private void Update()
    {
        if (Time.time > nextGenerateTime)
        {
            nextGenerateTime = Time.time + generateInterval;
            GeneratePalette();
        }

        UpdateSkybox();
    }
}
