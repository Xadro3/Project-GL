using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem alpha;
    public ParticleSystem beta;
    public ParticleSystem gamma;

    private void Start()
    {
        TurnMaster.AlphaDamageParticleEvent += HandleAlphaRadiation;
        TurnMaster.BetaDamageParticleEvent += HandleBetaRadiation;
        TurnMaster.GammaDamageParticleEvent += HandleGammaRadiation;   
    }

    private void OnDisable()
    {
        TurnMaster.AlphaDamageParticleEvent -= HandleAlphaRadiation;
        TurnMaster.BetaDamageParticleEvent -= HandleBetaRadiation;
        TurnMaster.GammaDamageParticleEvent -= HandleGammaRadiation;
    }


    private void Update()//Ganze update ist nur für Testzwecke drin, kann also deleted werden wenn die Eventtrigger implementiert wurden.
    {
//       if (Input.GetKeyDown(KeyCode.A))
//       {
//           alpha.Play();
//       }
//
//       if (Input.GetKeyDown(KeyCode.B))
//       {
//           beta.Play();
//       }
//
//       if (Input.GetKeyDown(KeyCode.G))
//       {
//           gamma.Play();
//       }
    }

    void HandleAlphaRadiation()
    {
        alpha.Play();
    }

    void HandleBetaRadiation()
    {
        beta.Play();
    }
    void HandleGammaRadiation()
    {
        gamma.Play();
    }

}
