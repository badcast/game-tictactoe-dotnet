using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace CAZ
{
	class AsyncOperation : IDisposable
	{

        private static int _ids;
		private DispatcherPriority _priority;
		private IEnumerator rator;
		private bool _isAsync;
		private int __id;

		private void CallBack()
		{
			if (IsDisposed || !IsAsync)
				return;

			_isAsync = this.Enumerator.MoveNext();
		}

		private void BeginHandle()
		{
			if (IsDisposed)
				throw new ObjectDisposedException("AsyncOperation");

			_isAsync = true;
			void begin()
			{
				Dispatcher.BeginInvoke((Action)_async, _priority);
			}
			void _async()
			{
				CallBack();

				if (IsDisposed)
					return;

				if (IsAsync)
					begin();
			};

			begin();
		}

		public AsyncOperation()
		{

		}

        public class Retrace
		{
		    public bool doBreak;
		}

        /// <summary>
        /// Создает асинхронную задачу (зависит для текущего скрина)
        /// </summary>
        public static AsyncOperation CreateAsync(IEnumerator enumerator, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
		{
			var s = new AsyncOperation(enumerator, dispatcherPriority, true);
			return s;
		}
        /// <summary>
        /// Создает асинхронную задачу (зависит для текущего скрина)
        /// </summary>
        public static AsyncOperation CreateAsync(Action<Retrace> enter, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
		{
			IEnumerator rator()
			{
				Retrace trace = new Retrace();
				trace.doBreak = false;
				while (!trace.doBreak)
				{
					enter(trace);
					yield return null;
				}
			}
			return CreateAsync(rator());

		}
        public static AsyncOperation CreateAsyncNotDependency(IEnumerator rator, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
		{
			return new AsyncOperation(rator, dispatcherPriority, false);
		}
        public static AsyncOperation CreateAsyncNotDependency(Action<Retrace> enter, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
		{
			IEnumerator rator()
			{
				Retrace trace = new Retrace();
				trace.doBreak = false;
				while (!trace.doBreak)
				{
					enter(trace);
					yield return null;
				}
			}
			return new AsyncOperation(rator(), dispatcherPriority, false);
		}

	
		public Dispatcher Dispatcher{ get; }
		public IEnumerator Enumerator{ get { if (IsDisposed) throw new ObjectDisposedException("AsyncOperation"); return rator; } }
			/// <summary>
			/// Состояние асинхронности
			/// </summary>
		public bool IsAsync{ get { if (IsDisposed) throw new ObjectDisposedException("AsyncOperation"); return _isAsync; } }
			/// <summary>
			/// Идентификатор
			/// </summary>
		public int id{ get { if (IsDisposed) throw new ObjectDisposedException("AsyncOperation"); return __id; } }
		public bool IsDisposed{ get; private set; }
		public bool IsDependency{ get; }
			public AsyncOperation(IEnumerator enumerator, DispatcherPriority dispatcherPriority, bool screenDependency)
		{
			if ((this.rator = enumerator) == null)
			{
				throw new ArgumentNullException();
			}

			this._priority = dispatcherPriority;

			if (this.IsDependency = screenDependency)
			{
				GameEngine.Current.AddRemoveHandler(this);
			}
			this.Dispatcher = Dispatcher.CurrentDispatcher;
			this.__id = _ids++;
		}

        /// <summary>
        /// Синхронно выполняет операцию
        /// </summary>
        public void Invoke()
		{
			if (IsDisposed)
				throw new ObjectDisposedException("AsyncOperation");
			if (!IsAsync)
				throw new Exception("Метод не запущен");

			CallBack();
		}
        public void Start()
		{
			if (IsAsync)
				throw new InvalidOperationException("Процесс уже выполняется. Вызовите метод Stop()");
			BeginHandle();
		}
        public void Stop()
		{
			_isAsync = false;
		}


        public void Dispose()
		{
			if (IsDisposed)
				return;
			Stop();
			__id = -1;
			rator = null;
			IsDisposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
