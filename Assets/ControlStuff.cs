using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStuff : MonoBehaviour
{
	private Rigidbody2D rb;
	private string _device;

	AudioClip microphoneInput;
	bool microphoneInitialized;
	public float sensitivity;
	public bool flapped;
	private AudioSource source;

	float[] samples = new float[256];
	
	private void Awake()
	{
		//init microphone input
		if (Microphone.devices.Length>0){
			microphoneInput = Microphone.Start(Microphone.devices[0],true,5,44100);
			source = GetComponent<AudioSource>();
			source.clip = microphoneInput;
			microphoneInitialized = true;
			source.loop = true;
			source.Play();

		}
		
	}

	// Start is called before the first frame update
	void Start()
	{
		// source = GetComponent<AudioSource>();
		// listener = GetComponent<AudioListener>();
		// listener.
		// _device = Microphone.devices[0];
		// source.clip = Microphone.Start(_device, true, 1, 44100);
		// source.loop = true;
		// while (!(Microphone.GetPosition(null) > 0)) { }
		//
		// source.Play();
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{

		//get mic volume
		int dec = 128;
		float[] waveData = new float[dec];
		int micPosition = Microphone.GetPosition(null)-(dec+1); // null means the first microphone
		microphoneInput.GetData(waveData, micPosition);
		source.GetSpectrumData(samples, 0, FFTWindow.Hamming);
		Debug.DrawLine(new Vector3(0,0), new Vector3(0, 10));
		for (int i = 1; i < samples.Length - 1; i++)
		{
			Debug.DrawLine(new Vector3(i - 1, samples[i] + 10, 0), new Vector3(i, samples[i + 1] + 10, 0), Color.red);
			Debug.DrawLine(new Vector3(i - 1, Mathf.Log(samples[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(samples[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), samples[i - 1] - 10, 1), new Vector3(Mathf.Log(i), samples[i] - 10, 1), Color.green);
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(samples[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(samples[i]), 3), Color.blue);
		}
		
		float max = 0;
		int maxIdx = 0;
		for (var i = 0; i < samples.Length; i++)
		{
			var sample = samples[i];
			if (sample > max)
			{
				max = sample;
				maxIdx = i;
			}
		}


		// Getting a peak on the last 128 samples
		float levelMax = 0;
		for (int i = 0; i < dec; i++) {
			float wavePeak = Math.Abs(waveData[i]);
			if (levelMax < wavePeak) {
				levelMax = wavePeak;
			}
		}

		if (levelMax > 0.05)
		{
			Debug.Log(maxIdx);
		}

		// float level = Mathf.Sqrt(levelMax);

		// int totalDistance = 0;
		// int distanceEntires = 0;
		// int distanceBetweenPeaks = 0;
		// float oldWave = 0.0f;
		//
		// bool increasing = false;
		//
		// for (int i = 0; i < dec; i++) {
		// 	float wavePeak = waveData[i];
		// 	if (oldWave >= 0 && wavePeak <= 0)
		// 	{
		// 		totalDistance += distanceBetweenPeaks;
		// 		distanceEntires++;
		// 		distanceBetweenPeaks = 0;
		// 	}
		//
		// 	distanceBetweenPeaks++;
		// 	oldWave = wavePeak;
		// }
		//
		// if (level > 0.1)
		// {
		// 	Debug.Log("DistancePeaks: " + totalDistance / (float)distanceEntires);
		// 	Debug.Log(level);
		// }
	}

	private void FixedUpdate()
	{
		rb.AddForce(rb.transform.up * 2);
		rb.AddTorque(Input.GetAxis("Horizontal") * -2);
	}
}