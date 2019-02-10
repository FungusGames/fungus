/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    // <summary>
    /// Get or Set a property of a AudioSource component
    /// </summary>
    [CommandInfo("AudioSource",
                 "Property",
                 "Get or Set a property of a AudioSource component")]
    [AddComponentMenu("")]
    public class AudioSourceProperty : BaseVariableProperty
    {
		//generated property
        public enum Property 
        { 
            Volume, 
            Pitch, 
            Time, 
            TimeSamples, 
            IsPlaying, 
            IsVirtual, 
            Loop, 
            IgnoreListenerVolume, 
            PlayOnAwake, 
            IgnoreListenerPause, 
            PanStereo, 
            SpatialBlend, 
            Spatialize, 
            SpatializePostEffects, 
            ReverbZoneMix, 
            BypassEffects, 
            BypassListenerEffects, 
            BypassReverbZones, 
            DopplerLevel, 
            Spread, 
            Priority, 
            Mute, 
            MinDistance, 
            MaxDistance, 
        }

		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        [VariableProperty(typeof(AudioSourceVariable))]
        protected AudioSourceVariable audioSourceVar;

        [SerializeField]
        [VariableProperty(typeof(FloatVariable),
                          typeof(IntegerVariable),
                          typeof(BooleanVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var iof = inOutVar as FloatVariable;
            var ioi = inOutVar as IntegerVariable;
            var iob = inOutVar as BooleanVariable;


            var target = audioSourceVar.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.Volume:
                            iof.Value = target.volume;
                            break;
                        case Property.Pitch:
                            iof.Value = target.pitch;
                            break;
                        case Property.Time:
                            iof.Value = target.time;
                            break;
                        case Property.TimeSamples:
                            ioi.Value = target.timeSamples;
                            break;
                        case Property.IsPlaying:
                            iob.Value = target.isPlaying;
                            break;
                        case Property.IsVirtual:
                            iob.Value = target.isVirtual;
                            break;
                        case Property.Loop:
                            iob.Value = target.loop;
                            break;
                        case Property.IgnoreListenerVolume:
                            iob.Value = target.ignoreListenerVolume;
                            break;
                        case Property.PlayOnAwake:
                            iob.Value = target.playOnAwake;
                            break;
                        case Property.IgnoreListenerPause:
                            iob.Value = target.ignoreListenerPause;
                            break;
                        case Property.PanStereo:
                            iof.Value = target.panStereo;
                            break;
                        case Property.SpatialBlend:
                            iof.Value = target.spatialBlend;
                            break;
                        case Property.Spatialize:
                            iob.Value = target.spatialize;
                            break;
                        case Property.SpatializePostEffects:
                            iob.Value = target.spatializePostEffects;
                            break;
                        case Property.ReverbZoneMix:
                            iof.Value = target.reverbZoneMix;
                            break;
                        case Property.BypassEffects:
                            iob.Value = target.bypassEffects;
                            break;
                        case Property.BypassListenerEffects:
                            iob.Value = target.bypassListenerEffects;
                            break;
                        case Property.BypassReverbZones:
                            iob.Value = target.bypassReverbZones;
                            break;
                        case Property.DopplerLevel:
                            iof.Value = target.dopplerLevel;
                            break;
                        case Property.Spread:
                            iof.Value = target.spread;
                            break;
                        case Property.Priority:
                            ioi.Value = target.priority;
                            break;
                        case Property.Mute:
                            iob.Value = target.mute;
                            break;
                        case Property.MinDistance:
                            iof.Value = target.minDistance;
                            break;
                        case Property.MaxDistance:
                            iof.Value = target.maxDistance;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        case Property.Volume:
                            target.volume = iof.Value;
                            break;
                        case Property.Pitch:
                            target.pitch = iof.Value;
                            break;
                        case Property.Time:
                            target.time = iof.Value;
                            break;
                        case Property.TimeSamples:
                            target.timeSamples = ioi.Value;
                            break;
                        case Property.Loop:
                            target.loop = iob.Value;
                            break;
                        case Property.IgnoreListenerVolume:
                            target.ignoreListenerVolume = iob.Value;
                            break;
                        case Property.PlayOnAwake:
                            target.playOnAwake = iob.Value;
                            break;
                        case Property.IgnoreListenerPause:
                            target.ignoreListenerPause = iob.Value;
                            break;
                        case Property.PanStereo:
                            target.panStereo = iof.Value;
                            break;
                        case Property.SpatialBlend:
                            target.spatialBlend = iof.Value;
                            break;
                        case Property.Spatialize:
                            target.spatialize = iob.Value;
                            break;
                        case Property.SpatializePostEffects:
                            target.spatializePostEffects = iob.Value;
                            break;
                        case Property.ReverbZoneMix:
                            target.reverbZoneMix = iof.Value;
                            break;
                        case Property.BypassEffects:
                            target.bypassEffects = iob.Value;
                            break;
                        case Property.BypassListenerEffects:
                            target.bypassListenerEffects = iob.Value;
                            break;
                        case Property.BypassReverbZones:
                            target.bypassReverbZones = iob.Value;
                            break;
                        case Property.DopplerLevel:
                            target.dopplerLevel = iof.Value;
                            break;
                        case Property.Spread:
                            target.spread = iof.Value;
                            break;
                        case Property.Priority:
                            target.priority = ioi.Value;
                            break;
                        case Property.Mute:
                            target.mute = iob.Value;
                            break;
                        case Property.MinDistance:
                            target.minDistance = iof.Value;
                            break;
                        case Property.MaxDistance:
                            target.maxDistance = iof.Value;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (audioSourceVar == null)
            {
                return "Error: no audioSourceVar set";
            }
            if (inOutVar == null)
            {
                return "Error: no variable set to push or pull data to or from";
            }

            return getOrSet.ToString() + " " + property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            if (audioSourceVar == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}