using System;

namespace Fast
{
    public class RequestProgress
    {
        public int progress_index = 0;
        public float progress_value = 0.0f;
    }

    public class Request<S, E>
    {
        private bool already_running = false;

        protected Callback<RequestProgress> on_progress_update = new Callback<RequestProgress>();

        public void UpdateProgress(int progress_index, float progress_value)
        {
            RequestProgress ret = new RequestProgress();
            ret.progress_index = progress_index;
            ret.progress_value = progress_value;

            on_progress_update.Invoke(ret);
        }

        public void RunRequest(Action<S> on_success, Action<E> on_fail)
        {
            if (!already_running)
            {
                RunRequestInternal(
                    delegate (S obj)
                    {
                        already_running = false;

                        if (on_success != null)
                            on_success.Invoke(obj);
                    }
                    , delegate (E obj)
                    {
                        already_running = false;

                        if (on_fail != null)
                            on_fail.Invoke(obj);
                    }
                    );
            }
        }

        public bool AlreadyRunning
        {
            get { return already_running; }
        }

        public Callback<RequestProgress> OnProgressUpdate
        {
            get { return on_progress_update; }
        }

        protected virtual void RunRequestInternal(Action<S> on_success, Action<E> on_fail)
        {

        }

    }
}

