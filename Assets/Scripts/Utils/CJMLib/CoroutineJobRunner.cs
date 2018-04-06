using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CJM
{
	// coroutine jobs runner
	public class CoroutineJobRunner : MonoBehaviour
	{
		bool initialized = false;
		string runnerId = string.Empty;
		Dictionary<ulong, CoroutineJob> jobs;
		uint runningJobs = 0; // debug purposes

		public System.Action<string> OnDestroyed;

		public void Init(string id)
		{
			Assert.Test(!string.IsNullOrEmpty(id), "ASSERT CoroutineJobRunner.Init: id cannot be null or empty!");
			if(!initialized && !string.IsNullOrEmpty(id)) {
				jobs = new Dictionary<ulong, CoroutineJob>();
				runnerId = id;
				CoroutineJob.OnBroadcastJobStarted += HandleOnBroadcastJobStarted;
				CoroutineJob.OnBroadcastJobCompleted += HandleOnBroadcastJobCompleted;
				initialized = true;
			}
		}

		void HandleOnBroadcastJobStarted (CoroutineJob job)
		{
			if(job.RunnerID == runnerId && !jobs.ContainsKey(job.JobUID))
			{
				jobs.Add(job.JobUID, job);
				++runningJobs;
			}
		}

		void HandleOnBroadcastJobCompleted (CoroutineJob job)
		{
			if(job.RunnerID == runnerId && jobs.ContainsKey(job.JobUID))
			{
				jobs.Remove(job.JobUID);
				--runningJobs;
			}
		}

		public string ID
		{
			get {
				return runnerId;
			}
		}

		public bool Initialized
		{
			get {
				return initialized;
			}
		}

		public void Flush()
		{
			Dictionary<ulong, CoroutineJob>.Enumerator enumerator = jobs.GetEnumerator();
			while(enumerator.MoveNext())
			{
				enumerator.Current.Value.Kill();
			}
			enumerator.Dispose();
		}

		public void OnDestroy()
		{
			CoroutineJob.OnBroadcastJobStarted += HandleOnBroadcastJobStarted;
			CoroutineJob.OnBroadcastJobCompleted += HandleOnBroadcastJobCompleted;

			if(OnDestroyed != null)
				OnDestroyed(runnerId);
		}
	}
}