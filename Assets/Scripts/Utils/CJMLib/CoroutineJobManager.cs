using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CJM
{
	public class CoroutineJobManager
	{
		const string DEFAULT_VOLATILE_RUNNER_ID = "default_volatile";
		const string DEFAULT_PERSISTENT_RUNNER_ID = "default_persistent";

		Dictionary<string, CoroutineJobRunner> runners;

		static CoroutineJobManager instance;

		private CoroutineJobManager()
		{
			runners = new Dictionary<string, CoroutineJobRunner>();
		}

		public static CoroutineJobManager Instance
		{
			get {
				if(instance == null) {
					instance = new CoroutineJobManager();
				}
				return instance;
			}
		}

		public CoroutineJobRunner CreateRunner(string id, bool ddol)
		{
			if(!runners.ContainsKey(id))
			{
				GameObject runnerObj = new GameObject("cjr_" + id);
				if(ddol) {
					GameObject.DontDestroyOnLoad(runnerObj);
				}
				CoroutineJobRunner coroutineJobRunner = runnerObj.AddComponent<CoroutineJobRunner>();
				coroutineJobRunner.Init(id);
				runners.Add(id, coroutineJobRunner);
			}
			return runners[id];
		}

		public void DestroyRunner(string id)
		{
			if(runners.ContainsKey(id))
			{
				CoroutineJobRunner coroutineJobRunner = runners[id];
				GameObject.Destroy(coroutineJobRunner.gameObject);

				runners[id] = null;
				runners.Remove(id);
			}
		}

		public CoroutineJobRunner GetRunner(string id)
		{
			return GetRunner(id, false, false);
		}

		public CoroutineJobRunner GetRunner(string id, bool create)
		{
			return GetRunner(id, create, false);
		}

		public CoroutineJobRunner GetRunner(string id, bool create, bool ddol)
		{
			if(!runners.ContainsKey(id) && create) {
				CreateRunner(id, ddol);
			}
			return runners.ContainsKey(id) ? runners[id] : null;
		}

		public CoroutineJobRunner GetDefaultRunner(bool ddol)
		{
			return ddol ? GetDefaultPersistentRunner() : GetDefaultVolatileRunner();
		}

		public CoroutineJobRunner GetDefaultPersistentRunner()
		{
			return GetRunner(DEFAULT_PERSISTENT_RUNNER_ID, true, true);
		}

		public CoroutineJobRunner GetDefaultVolatileRunner()
		{
			return GetRunner(DEFAULT_VOLATILE_RUNNER_ID, true, false);
		}

		public CoroutineJobRunner persistentRunner { get { return GetDefaultPersistentRunner(); } }
		public CoroutineJobRunner volatileRunner { get { return GetDefaultVolatileRunner(); } }

		public bool RunnerExists(string id)
		{
			return runners.ContainsKey(id);
		}
	}
}