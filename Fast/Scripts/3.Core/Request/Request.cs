using System;

namespace Fast
{
    public class RequestProgress
    {
        private int progress_index = 0;
        private float progress_value = 0.0f;

        public RequestProgress(int progress_index, float progress_value)
        {
            this.progress_index = progress_index;
            this.progress_value = progress_value;
        }

        public int ProgressIndex => progress_index;
        public float ProgressValue => progress_value;
    }

    public class Request<S, E>
    {
        private bool running = false;

        protected Callback<RequestProgress> on_progress_update = new Callback<RequestProgress>();

        public void RunRequest(Action<S> on_success, Action<E> on_fail)
        {
            if (!running)
            {
                RunRequestInternal(
                    delegate (S obj)
                    {
                        running = false;

                        if (on_success != null)
                            on_success.Invoke(obj);
                    }
                    , delegate (E obj)
                    {
                        running = false;

                        if (on_fail != null)
                            on_fail.Invoke(obj);
                    }
                    );
            }
        }

        public bool Running
        {
            get { return running; }
        }

        public Callback<RequestProgress> OnProgressUpdate
        {
            get { return on_progress_update; }
        }

        public void UpdateProgress(int progress_index, float progress_value)
        {
            RequestProgress ret = new RequestProgress(progress_index, progress_value);

            on_progress_update.Invoke(ret);
        }

        protected virtual void RunRequestInternal(Action<S> on_success, Action<E> on_fail)
        {

        }
    }
}

