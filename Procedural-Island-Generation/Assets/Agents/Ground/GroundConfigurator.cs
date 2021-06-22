using Runtime;
using Tools.UI;
using Tools;
using UnityEngine;

namespace Agents
{
    /// <summary>
    /// Configurator for GroundGenerator with UI and editor controls
    /// </summary>
    public class GroundConfigurator : ConfiguratorBase
    {
        public MeshFilter groundMeshFilter;
        public MeshCollider groundMeshCollider;
        public RectTransform leftPanel;
        public bool constantSeed = false;
        public GroundGenerator.Config config = new GroundGenerator.Config();

        private const int minYSize = 1;
        private const int maxYSize = 10;
        private const float minCellSize = 0.3f;
        private const float maxCellSize = 1;
        private const int minNoiseFrequency = 1;
        private const int maxNoiseFrequency = 8;

        private Mesh groundMesh;

        private void Awake()
        {
            Generate();
            SetupSkyboxAndPalette();

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Ground Height", minYSize, maxYSize, (int) config.groundSize.y, value =>
                {
                    config.groundSize.y = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Cell Size", minCellSize, maxCellSize, config.cellSize, value =>
                {
                    config.cellSize = value;
                    Generate();
                });

            InstantiateControl<SliderControl>(leftPanel)
                .Initialize("Noise Frequency", minNoiseFrequency, maxNoiseFrequency, (int) config.noiseFrequency, value =>
                {
                    config.noiseFrequency = value;
                    Generate();
                });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate", () => Generate());
        }

        private void Update()
        {
            UpdateSkybox();
        }

        public void Generate(bool randomizeConfig = true)
        {
            if (constantSeed)
            {
                Random.InitState(0);
            }

            if (randomizeConfig)
            {
                GeneratePalette();

                config.gradient = ColorE.Gradient(from: GetMainColorHSV(), to: GetSecondaryColorHSV());
            }

            var draft = GroundGenerator.GroundDraft(config);
            draft.Move(Vector3.left*config.groundSize.x/2 + Vector3.back*config.groundSize.z/2);
            AssignDraftToMeshFilter(draft, groundMeshFilter, ref groundMesh);
            groundMeshCollider.sharedMesh = groundMesh;
        }
    }
}
