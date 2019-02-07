using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NVIDIA.Flex;

namespace NVIDIA.Flex
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Flex/Flex Cloth Actor and Particle getter")]

    // This just lets us grab ParticleData from FlexContainer

    public class ParticleGetter : FlexClothActor
    {
        FlexContainer.ParticleData _particleData;

        public FlexContainer.ParticleData particleData { get => _particleData; }

        protected override void OnFlexUpdate(FlexContainer.ParticleData _particleData)
        {
            this._particleData = _particleData;
            //print("got particle data");
            base.OnFlexUpdate(_particleData);
        }
    }
}
