using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
internal class Metronome_procedural : MonoBehaviour, IMetronome
{
    private int sampleRate;
    private double nextBeatSample;
    private double samplesPerBeat;

    [Header("Metronome settings")]
    [SerializeField] private float bpm = 120f;
    [SerializeField] private int beatGrouping = 4;
    private bool isRunning;
    private int currentBeat;

    [Header("Sine wave settings")]
    [SerializeField] [Range(20, 20000)] private float clickPitchInHz_Loud = 440f;
    [SerializeField] [Range(20, 20000)] private float clickPitchInHz_Soft = 440f;
    [SerializeField] [Range(0, 1)] private float clickAmplitude_Loud = 1f;
    [SerializeField] [Range(0, 1)] private float clickAmplitude_Soft = .7f;
    [SerializeField] [Range(1, 10)] private float clickDurationInMs = 5f;
    private float amplitude = 0f;
    private float amplitudeDecayRate;
    private float phase = 0f;
    private float phaseIncrement_Loud;
    private float phaseIncrement_Soft;

    private void Start() => InitializeMetronome();
    
    private void InitializeMetronome()
    {
        sampleRate = AudioSettings.outputSampleRate;
        samplesPerBeat = GetSamplesPerBeat();
        amplitudeDecayRate = GetAmplitudeDecayRate();

        phaseIncrement_Loud = clickPitchInHz_Loud / sampleRate;
        phaseIncrement_Soft = clickPitchInHz_Soft / sampleRate;

        nextBeatSample = GetCurrentSample();
        isRunning = true;
    }

    private void OnAudioFilterRead(float[] sampleStream, int channels)
    {
        if (!isRunning) return;

        int samplesInStream = sampleStream.Length / channels;
        double currentSample = GetCurrentSample();

        for (int sample = 0; sample < samplesInStream; sample++)
        {
            // update phase and amplitude
            if (currentSample + sample >= nextBeatSample)
            {
                currentBeat = (currentBeat % beatGrouping) + 1;
                amplitude = currentBeat == 1 ? clickAmplitude_Loud : clickAmplitude_Soft;
                phase = 0f;

                nextBeatSample += samplesPerBeat;

                TriggerBeatPlayed(new BeatPlayedEventArgs(
                    beat: currentBeat,
                    dspTime: SampleToDspTime(currentSample + sample)));
            }
            else
            {
                amplitude *= amplitudeDecayRate;
                phase += currentBeat == 1 ? phaseIncrement_Loud : phaseIncrement_Soft;
            }

            //calculate value
            float value = Mathf.Sin(phase * 2 * Mathf.PI) * amplitude;

            //write value to stream
            for (int channel = 0; channel < channels; channel++)
            {
                int i = sample * channels + channel;
                sampleStream[i] = value;
            }
        }
    }

    private double GetCurrentSample() => AudioSettings.dspTime * sampleRate;
    private double SampleToDspTime(double sample) => sample / sampleRate;
    private double GetSamplesPerBeat() => sampleRate * 60 / bpm;

    private float GetAmplitudeDecayRate()
    {
        float decaySamples = clickDurationInMs / 1000 * sampleRate;
        float decayRate = 1 - (1 / decaySamples);
        decayRate = Mathf.Clamp(decayRate, 0, .9999f); //safety

        return decayRate;
    }

    private void TriggerBeatPlayed(BeatPlayedEventArgs args) => EventManager.Instance.BeatPlayed(this, args);
}
