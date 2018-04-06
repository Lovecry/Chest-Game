using UnityEngine;
using System.Collections;

namespace CJM
{
	public class CoroutineJob
	{
		bool running;
		bool paused;
		bool killed;
		IEnumerator job;
		string runnerId;
		string jobId;
		ulong jobUid;
		static ulong jobUIDMasterCounter = 0;

		public static event System.Action<CoroutineJob> OnBroadcastJobStarted;
		public static event System.Action<CoroutineJob> OnBroadcastJobCompleted;
		public static event System.Action<CoroutineJob> OnBroadcastJobKilled;
		public static event System.Action<CoroutineJob> OnBroadcastJobStartError;

		public event System.Action OnJobStarted;
		public event System.Action OnJobCompleted;
		public event System.Action OnJobKilled;
		public event System.Action OnJobPaused;
		public event System.Action OnJobUnpaused;
		public event System.Action OnJobStartError;

		void RaiseOnJobStarted() { if(OnJobStarted != null) OnJobStarted(); if(OnBroadcastJobStarted != null) OnBroadcastJobStarted(this); }
		void RaiseOnJobCompleted() { if(OnJobCompleted != null) OnJobCompleted(); if(OnBroadcastJobCompleted != null) OnBroadcastJobCompleted(this); }
		void RaiseOnJobKilled() { if(OnJobKilled != null) OnJobKilled(); if(OnBroadcastJobKilled != null) OnBroadcastJobKilled(this); }
		void RaiseOnJobPaused() { if(OnJobPaused != null) OnJobPaused(); }
		void RaiseOnJobUnpaused() { if(OnJobUnpaused != null) OnJobUnpaused(); }
		void RaiseOnJobStartError() { if(OnJobStartError != null) OnJobStartError(); if(OnBroadcastJobStartError != null) OnBroadcastJobStartError(this); }

		public CoroutineJob(IEnumerator coroutine) : this("", coroutine) {}
		public CoroutineJob(string id, IEnumerator coroutine)
		{
			jobUid = ++jobUIDMasterCounter;
			jobId = id;
			job = coroutine;
			running = false;
			killed = false;
			paused = false;
		}

		private IEnumerator Run()
		{
			RaiseOnJobStarted();

			yield return null;

			while(running)
			{
				if(paused) 
				{
					yield return null;
				}
				else 
				{
					if(job.MoveNext())
					{
						yield return job.Current;
					}
					else
					{
						running = false;
					}
				}
			}

			RaiseOnJobCompleted();
		}

		public void Start(string jobRunner)
		{
			Start(CoroutineJobManager.Instance.GetRunner(jobRunner, true));
		}

		public void Start(CoroutineJobRunner jobRunner)
		{
			if(!running)
			{
				if(jobRunner.Initialized)
				{
					running = true;
					killed = false;

					runnerId = jobRunner.ID;

					jobRunner.StartCoroutine(Run());
				}
				else
				{
					RaiseOnJobStartError();
				}
			}
		}

		public void Pause()
		{
			paused = true;
			RaiseOnJobPaused();
		}

		public void Unpause()
		{
			paused = false;
			RaiseOnJobUnpaused();
		}

		public void Kill()
		{
			if(running)
			{
				killed = true;
				running = false;
				paused = false;

				RaiseOnJobKilled();
			}
		}

		public bool Running
		{
			get {
				return running;
			}
		}

		public bool Paused
		{
			get {
				return paused;
			}
		}

		public bool Killed
		{
			get {
				return killed;
			}
		}

		public string RunnerID
		{
			get {
				return runnerId;
			}
		}

		public string JobID
		{
			get {
				return jobId;
			}
		}

		public ulong JobUID
		{
			get {
				return jobUid;
			}
		}

		#region static utilities
		public static CoroutineJob Create(IEnumerator coroutine)
		{
			return new CoroutineJob(coroutine);
		}

		/// <summary>
		/// Start the specified coroutine on persistent default runner.
		/// </summary>
		/// <param name="coroutine">Coroutine.</param>
		public static CoroutineJob Start(IEnumerator coroutine)
		{
			return CoroutineJob.Start(coroutine, true);
		}

		/// <summary>
		/// Start the specified coroutine on persistent or volatile default job runner.
		/// </summary>
		/// <param name="coroutine">Coroutine.</param>
		/// <param name="ddol">If set to <c>true</c> ddol.</param>
		public static CoroutineJob Start(IEnumerator coroutine, bool ddol)
		{
			return CoroutineJob.Start(coroutine, CoroutineJobManager.Instance.GetDefaultRunner(ddol));
		}

		/// <summary>
		/// Start the specified coroutine on specified job runner.
		/// </summary>
		/// <param name="coroutine">Coroutine.</param>
		/// <param name="jobRunner">Job runner.</param>
		public static CoroutineJob Start(IEnumerator coroutine, CoroutineJobRunner jobRunner)
		{
			CoroutineJob job = CoroutineJob.Create(coroutine);
			job.Start(jobRunner);
			return job;
		}

		/// <summary>
		/// Execs immediately a coroutine without yeilding.
		/// </summary>
		/// <param name="coroutine">Coroutine.</param>
		public static void ExecSyncCoroutine(IEnumerator coroutine)
		{
			if(coroutine != null)
			{
				while(coroutine.MoveNext());
			}
		}
		#endregion
	}
}