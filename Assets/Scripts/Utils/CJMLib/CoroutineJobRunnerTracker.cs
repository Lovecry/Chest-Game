using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CJM
{
	public class CoroutineJobRunnerTracker 
	{
		string m_runnerId;
		int m_jobsCount;

		public event System.Action<int> OnJobsCountChange;

		public string runnerId { get { return m_runnerId; } }
		public int jobsCount { get { return m_jobsCount; } }

#if DEBUG_HTTP_CALLS
		public List<string> jobsId = new List<string>();
		public int anonJobsIds = 0;
#endif

		public CoroutineJobRunnerTracker(CoroutineJobRunner runner)
		{
			runner.OnDestroyed += OnRunnerDestroyed;
			m_runnerId = runner.ID;

			CoroutineJob.OnBroadcastJobStarted += HandleOnBroadcastJobStarted;
			CoroutineJob.OnBroadcastJobCompleted += HandleOnBroadcastJobCompleted;
			CoroutineJob.OnBroadcastJobKilled += HandleOnBroadcastJobKilled;
		}

		void HandleOnBroadcastJobStarted (CoroutineJob job)
		{
			if(job.RunnerID == runnerId)
			{
#if DEBUG_HTTP_CALLS
				if(!string.IsNullOrEmpty(job.JobID)) {
					jobsId.Add(job.JobID);
				}
				else {
					++anonJobsIds;
				}
#endif
				++m_jobsCount;
				if(OnJobsCountChange != null) {
					OnJobsCountChange(m_jobsCount);
				}
			}
		}

		void HandleOnBroadcastJobKilled (CoroutineJob job)
		{
			if(job.RunnerID == runnerId)
			{
				#if DEBUG_HTTP_CALLS
				if(!string.IsNullOrEmpty(job.JobID)) {
					jobsId.Remove(job.JobID);
				}
				else {
					--anonJobsIds;
				}
				#endif
				--m_jobsCount;
				if(OnJobsCountChange != null) {
					OnJobsCountChange(m_jobsCount);
				}
			}
		}

		void HandleOnBroadcastJobCompleted (CoroutineJob job)
		{
			if(job.RunnerID == runnerId && !job.Killed)
			{
#if DEBUG_HTTP_CALLS
				if(!string.IsNullOrEmpty(job.JobID)) {
					jobsId.Remove(job.JobID);
				}
				else {
					--anonJobsIds;
				}
#endif
				--m_jobsCount;
				if(OnJobsCountChange != null) {
					OnJobsCountChange(m_jobsCount);
				}
			}
		}

		void OnRunnerDestroyed(string id)
		{
			m_jobsCount = 0;
			if(OnJobsCountChange != null) {
				OnJobsCountChange(m_jobsCount);
			}
		}
	}
}
